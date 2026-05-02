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

  it('retorna fallback quando erro nao tem mensagem util', () => {
    expect(getErrorMessage(null, 'fallback')).toBe('fallback');
  });
});
