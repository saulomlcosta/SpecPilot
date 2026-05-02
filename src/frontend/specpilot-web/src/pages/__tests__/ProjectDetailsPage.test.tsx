import { screen } from '@testing-library/react';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ProjectDetailsPage } from '../ProjectDetailsPage';
import type { ProjectResponse } from '../../types/api';
import { renderWithProviders } from '../../test/test-utils';

const getProjectByIdMock = vi.fn();
const getQuestionsMock = vi.fn();

vi.mock('../../services/projects', () => ({
  getProjectById: (...args: unknown[]) => getProjectByIdMock(...args),
  getQuestions: (...args: unknown[]) => getQuestionsMock(...args),
  generateQuestions: vi.fn(),
  answerQuestions: vi.fn(),
  generateDocument: vi.fn(),
  updateProject: vi.fn(),
  deleteProject: vi.fn()
}));

function makeProject(status: ProjectResponse['status']): ProjectResponse {
  return {
    id: 'project-1',
    name: 'Projeto de teste',
    initialDescription: 'Descricao inicial',
    goal: 'Objetivo',
    targetAudience: 'Publico',
    status,
    createdAt: new Date().toISOString(),
    updatedAt: null
  };
}

describe('ProjectDetailsPage', () => {
  beforeEach(() => {
    getProjectByIdMock.mockReset();
    getQuestionsMock.mockReset();
  });

  it('renderiza dados principais do projeto', async () => {
    getProjectByIdMock.mockResolvedValueOnce(makeProject('Draft'));
    getQuestionsMock.mockResolvedValueOnce({ projectId: 'project-1', status: 'Draft', questions: [] });

    renderWithProviders(<ProjectDetailsPage />, { route: '/projects/project-1', path: '/projects/:id' });

    expect(await screen.findByText('Projeto de teste')).toBeInTheDocument();
    expect(screen.getByText('Rascunho')).toBeInTheDocument();
  });

  it('exibe acao correta quando status e Draft', async () => {
    getProjectByIdMock.mockResolvedValueOnce(makeProject('Draft'));

    renderWithProviders(<ProjectDetailsPage />, { route: '/projects/project-1', path: '/projects/:id' });

    expect(await screen.findByRole('button', { name: /Gerar perguntas de refinamento/i })).toBeInTheDocument();
  });

  it('exibe formulario de respostas quando status e QuestionsGenerated', async () => {
    getProjectByIdMock.mockResolvedValueOnce(makeProject('QuestionsGenerated'));
    getQuestionsMock.mockResolvedValueOnce({
      projectId: 'project-1',
      status: 'QuestionsGenerated',
      questions: [{ id: 'q1', order: 1, questionText: 'Pergunta 1', answer: null }]
    });

    renderWithProviders(<ProjectDetailsPage />, { route: '/projects/project-1', path: '/projects/:id' });

    expect(await screen.findByText('Responder perguntas de refinamento')).toBeInTheDocument();
    expect(await screen.findByText(/Pergunta 1/i)).toBeInTheDocument();
  });

  it('exibe acao de gerar documento quando status e QuestionsAnswered', async () => {
    getProjectByIdMock.mockResolvedValueOnce(makeProject('QuestionsAnswered'));

    renderWithProviders(<ProjectDetailsPage />, { route: '/projects/project-1', path: '/projects/:id' });

    expect(await screen.findByRole('button', { name: /Gerar documento/i })).toBeInTheDocument();
  });

  it('exibe link para documento quando status e DocumentGenerated', async () => {
    getProjectByIdMock.mockResolvedValueOnce(makeProject('DocumentGenerated'));

    renderWithProviders(<ProjectDetailsPage />, { route: '/projects/project-1', path: '/projects/:id' });

    const link = await screen.findByRole('link', { name: /Visualizar documento/i });
    expect(link).toHaveAttribute('href', '/projects/project-1/document');
  });
});
