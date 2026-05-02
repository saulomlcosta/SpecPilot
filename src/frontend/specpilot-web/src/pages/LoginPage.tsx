import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { Notice } from '../components/Notice';
import { useAuth } from '../contexts/AuthContext';
import { loginSchema, type LoginFormValues } from '../schemas/loginSchema';
import { getErrorMessage } from '../utils/errorMessage';

export function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();
  const [apiError, setApiError] = useState<string | null>(null);
  const redirectPath =
    (location.state as { from?: { pathname?: string } } | null)?.from?.pathname || '/projects';
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      password: ''
    }
  });

  const onSubmit = async (values: LoginFormValues) => {
    setApiError(null);
    try {
      await login(values);
      navigate(redirectPath, { replace: true });
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel realizar login. Tente novamente.'));
    }
  };

  return (
    <Card
      title="Entrar no SpecPilot AI"
      description="Informe suas credenciais para continuar no fluxo do MVP."
    >
      {redirectPath !== '/projects' && (
        <div className="mb-4">
          <Notice title="Autenticacao necessaria">
            Faca login para acessar a rota protegida solicitada e continuar de onde voce parou.
          </Notice>
        </div>
      )}

      <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
        <Input label="Email" placeholder="saulo@example.com" disabled={isSubmitting} {...register('email')} />
        <FormError message={errors.email?.message} />

        <Input label="Senha" placeholder="12345678" type="password" disabled={isSubmitting} {...register('password')} />
        <FormError message={errors.password?.message} />

        <Button className="w-full" type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Entrando...' : 'Entrar'}
        </Button>

        <FormError message={apiError ?? undefined} />
      </form>

      <p className="mt-6 text-sm text-slate-600">
        Ainda nao tem conta?{' '}
        <Link className="font-semibold text-brand-700" to="/register">
          Criar cadastro
        </Link>
      </p>
    </Card>
  );
}
