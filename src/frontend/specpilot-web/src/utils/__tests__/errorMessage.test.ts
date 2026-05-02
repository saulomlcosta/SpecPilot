import { describe, expect, it } from 'vitest';
import { ApiError } from '../../services/httpClient';
import { getErrorMessage } from '../errorMessage';

describe('getErrorMessage', () => {
  it('retorna detail de ApiError', () => {
    const error = new ApiError({
      title: 'Titulo',
      detail: 'Mensagem amigavel',
      status: 400
    });

    expect(getErrorMessage(error, 'fallback')).toBe('Mensagem amigavel');
  });

  it('usa fallback quando ApiError contem detalhe tecnico sensivel', () => {
    const error = new ApiError({
      title: 'Falha interna.',
      detail:
        'System.InvalidOperationException: erro inesperado\n   at SpecPilot.Api.Controllers.ProjectsController.Get()',
      status: 500
    });

    expect(getErrorMessage(error, 'Nao foi possivel concluir a operacao.')).toBe(
      'Nao foi possivel concluir a operacao.'
    );
  });

  it('retorna fallback quando erro nao tem mensagem util', () => {
    expect(getErrorMessage(null, 'fallback')).toBe('fallback');
  });
});
