import { fireEvent, screen } from '@testing-library/react';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { RegisterPage } from '../RegisterPage';
import { renderWithProviders } from '../../test/test-utils';

const registerMock = vi.fn();

vi.mock('../../contexts/AuthContext', () => ({
  useAuth: () => ({
    register: registerMock
  })
}));

describe('RegisterPage', () => {
  beforeEach(() => {
    registerMock.mockReset();
  });

  it('renderiza campos nome, email e senha', () => {
    renderWithProviders(<RegisterPage />, { route: '/register', path: '/register' });

    expect(screen.getByLabelText(/Nome/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Email/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Senha/i)).toBeInTheDocument();
  });

  it('valida campos obrigatorios', async () => {
    renderWithProviders(<RegisterPage />, { route: '/register', path: '/register' });

    fireEvent.click(screen.getByRole('button', { name: /Criar conta/i }));

    expect(await screen.findByText('Informe um nome com pelo menos 2 caracteres.')).toBeInTheDocument();
    expect(await screen.findByText('Informe um email valido.')).toBeInTheDocument();
    expect(await screen.findByText('A senha deve ter pelo menos 8 caracteres.')).toBeInTheDocument();
  });

  it('valida email invalido', async () => {
    renderWithProviders(<RegisterPage />, { route: '/register', path: '/register' });

    fireEvent.change(screen.getByLabelText(/Nome/i), { target: { value: 'Saulo' } });
    fireEvent.change(screen.getByLabelText(/Email/i), { target: { value: 'email-invalido' } });
    fireEvent.change(screen.getByLabelText(/Senha/i), { target: { value: '12345678' } });
    fireEvent.click(screen.getByRole('button', { name: /Criar conta/i }));

    expect(await screen.findByText('Informe um email valido.')).toBeInTheDocument();
  });
});
