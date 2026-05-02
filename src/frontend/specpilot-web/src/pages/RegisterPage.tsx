import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useNavigate } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { useAuth } from '../contexts/AuthContext';
import { registerSchema, type RegisterFormValues } from '../schemas/registerSchema';
import { getErrorMessage } from '../utils/errorMessage';

export function RegisterPage() {
  const navigate = useNavigate();
  const { register: registerUser } = useAuth();
  const [apiError, setApiError] = useState<string | null>(null);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      name: '',
      email: '',
      password: ''
    }
  });

  const onSubmit = async (values: RegisterFormValues) => {
    setApiError(null);
    try {
      await registerUser(values);
      navigate('/projects', { replace: true });
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel criar o cadastro. Tente novamente.'));
    }
  };

  return (
    <Card
      title="Criar conta"
      description="Crie sua conta para iniciar os projetos do SpecPilot AI."
    >
      <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
        <Input label="Nome" placeholder="Saulo" {...register('name')} />
        <FormError message={errors.name?.message} />

        <Input label="Email" placeholder="saulo@example.com" {...register('email')} />
        <FormError message={errors.email?.message} />

        <Input label="Senha" placeholder="12345678" type="password" {...register('password')} />
        <FormError message={errors.password?.message} />

        <Button className="w-full" type="submit" variant="secondary" disabled={isSubmitting}>
          {isSubmitting ? 'Criando conta...' : 'Criar conta'}
        </Button>

        <FormError message={apiError ?? undefined} />
      </form>

      <p className="mt-6 text-sm text-slate-600">
        Ja tem conta?{' '}
        <Link className="font-semibold text-brand-700" to="/login">
          Fazer login
        </Link>
      </p>
    </Card>
  );
}
