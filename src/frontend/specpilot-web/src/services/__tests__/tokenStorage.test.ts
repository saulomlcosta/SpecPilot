import { describe, expect, it } from 'vitest';
import { clearStoredAuthToken, getStoredAuthToken, setStoredAuthToken } from '../tokenStorage';

describe('tokenStorage', () => {
  it('salva, recupera e remove token', () => {
    clearStoredAuthToken();
    expect(getStoredAuthToken()).toBeNull();

    setStoredAuthToken('jwt-token');
    expect(getStoredAuthToken()).toBe('jwt-token');

    clearStoredAuthToken();
    expect(getStoredAuthToken()).toBeNull();
  });
});
