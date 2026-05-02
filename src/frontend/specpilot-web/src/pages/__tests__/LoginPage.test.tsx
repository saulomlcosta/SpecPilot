import { fireEvent, screen, waitFor } from '@testing-library/react';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { LoginPage } from '../LoginPage';
import { ApiError } from '../../services/httpClient';
import { renderWithProviders } from '../../test/test-utils';

const loginMock = vi.fn();

vi.mock('../../contexts/AuthContext', () => ({
  useAuth: () => ({
    login: loginMock
  })
}));

describe('LoginPage', () => {
  beforeEach(() => {
    loginMock.mockReset();
  });

  it('renderiza campos de email e senha', () => {
    renderWithProviders(<LoginPage />, { route: '/login', path: '/login' });

    expect(screen.getByLabelText(/Email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Senha/i)).toBeInTheDocument();
  });

  it('valida campos obrigatorios', async () => {
    renderWithProviders(<LoginPage />, { route: '/login', path: '/login' });

    fireEvent.click(screen.getByRole('button', { name: /Entrar/i }));

    expect(await screen.findByText('Informe um email valido.')).toBeInTheDocument();
    expect(await screen.findByText('A senha deve ter pelo menos 8 caracteres.')).toBeInTheDocument();
  });

  it('exibe erro amigavel quando API retorna ProblemDetails', async () => {
    loginMock.mockRejectedValueOnce(
      new ApiError({
        title: 'Nao autorizado.',
        detail: 'Email ou senha invalidos.',
        status: 401
      })
    );

    renderWithProviders(<LoginPage />, { route: '/login', path: '/login' });

    fireEvent.change(screen.getByLabelText(/Email/i), { target: { value: 'user@example.com' } });
    fireEvent.change(screen.getByLabelText(/Senha/i), { target: { value: '12345678' } });
    fireEvent.click(screen.getByRole('button', { name: /Entrar/i }));

    await waitFor(() => {
      expect(screen.getByText('Email ou senha invalidos.')).toBeInTheDocument();
    });
  });
});
