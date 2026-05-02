import { fireEvent, screen, waitFor } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';
import { CreateProjectPage } from '../CreateProjectPage';
import { renderWithProviders } from '../../test/test-utils';

const createProjectMock = vi.fn();

vi.mock('../../services/projects', () => ({
  createProject: (...args: unknown[]) => createProjectMock(...args)
}));

describe('CreateProjectPage', () => {
  it('valida campos obrigatorios', async () => {
    renderWithProviders(<CreateProjectPage />, { route: '/projects/new', path: '/projects/new' });

    fireEvent.click(screen.getByRole('button', { name: /Criar projeto/i }));

    expect(await screen.findByText('Informe um nome com pelo menos 3 caracteres.')).toBeInTheDocument();
    expect(await screen.findByText('Descreva melhor a ideia inicial.')).toBeInTheDocument();
    expect(await screen.findByText('Informe um objetivo claro para o projeto.')).toBeInTheDocument();
    expect(await screen.findByText('Informe o publico-alvo do projeto.')).toBeInTheDocument();
  });

  it('nao envia status no payload', async () => {
    createProjectMock.mockResolvedValueOnce({
      id: 'project-1',
      name: 'Projeto',
      initialDescription: 'Descricao inicial com tamanho suficiente.',
      goal: 'Objetivo claro',
      targetAudience: 'Publico alvo',
      status: 'Draft',
      createdAt: new Date().toISOString(),
      updatedAt: null
    });

    renderWithProviders(<CreateProjectPage />, { route: '/projects/new', path: '/projects/new' });

    fireEvent.change(screen.getByLabelText(/Nome do projeto/i), { target: { value: 'Projeto MVP' } });
    fireEvent.change(screen.getByLabelText(/Descricao inicial/i), {
      target: { value: 'Descricao inicial com tamanho suficiente.' }
    });
    fireEvent.change(screen.getByLabelText(/Objetivo/i), { target: { value: 'Objetivo do projeto' } });
    fireEvent.change(screen.getByLabelText(/Publico-alvo/i), { target: { value: 'Equipe do produto' } });
    fireEvent.click(screen.getByRole('button', { name: /Criar projeto/i }));

    await waitFor(() => expect(createProjectMock).toHaveBeenCalledTimes(1));

    const payload = createProjectMock.mock.calls[0][0] as Record<string, unknown>;
    expect(payload).toMatchObject({
      name: 'Projeto MVP',
      initialDescription: 'Descricao inicial com tamanho suficiente.',
      goal: 'Objetivo do projeto',
      targetAudience: 'Equipe do produto'
    });
    expect(payload).not.toHaveProperty('status');
  });
});
