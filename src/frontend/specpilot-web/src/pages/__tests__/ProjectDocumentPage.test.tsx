import { screen } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';
import { ProjectDocumentPage } from '../ProjectDocumentPage';
import { ApiError } from '../../services/httpClient';
import { renderWithProviders } from '../../test/test-utils';

const getDocumentMock = vi.fn();

vi.mock('../../services/projects', () => ({
  getDocument: (...args: unknown[]) => getDocumentMock(...args)
}));

describe('ProjectDocumentPage', () => {
  it('renderiza secoes do documento', async () => {
    getDocumentMock.mockResolvedValueOnce({
      projectId: 'project-1',
      status: 'DocumentGenerated',
      overview: 'Visao geral teste',
      functionalRequirements: ['RF1'],
      nonFunctionalRequirements: ['RNF1'],
      useCases: ['UC1'],
      risks: ['Risco 1']
    });

    renderWithProviders(<ProjectDocumentPage />, {
      route: '/projects/project-1/document',
      path: '/projects/:id/document'
    });

    expect(await screen.findByText('Visao geral')).toBeInTheDocument();
    expect(screen.getByText('Requisitos funcionais')).toBeInTheDocument();
    expect(screen.getByText('Requisitos nao funcionais')).toBeInTheDocument();
    expect(screen.getByText('Casos de uso')).toBeInTheDocument();
    expect(screen.getByText('Riscos')).toBeInTheDocument();
  });

  it('exibe mensagem amigavel quando documento nao existe', async () => {
    getDocumentMock.mockRejectedValueOnce(
      new ApiError({
        title: 'Nao encontrado.',
        detail: 'Documento nao encontrado.',
        status: 404
      })
    );

    renderWithProviders(<ProjectDocumentPage />, {
      route: '/projects/project-1/document',
      path: '/projects/:id/document'
    });

    expect(await screen.findByText('Documento ainda nao gerado')).toBeInTheDocument();
  });
});
