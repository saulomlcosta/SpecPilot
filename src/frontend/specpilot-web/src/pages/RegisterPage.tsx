import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { registerSchema, type RegisterFormValues } from '../schemas/registerSchema';

export function RegisterPage() {
  const [submitted, setSubmitted] = useState(false);
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

  const onSubmit = async () => {
    setSubmitted(true);
  };

  return (
    <Card
      title="Criar conta"
      description="A tela prepara o cadastro do usuario e ja valida os campos essenciais do contrato da API."
    >
      <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
        <Input label="Nome" placeholder="Saulo" {...register('name')} />
        <FormError message={errors.name?.message} />

        <Input label="Email" placeholder="saulo@example.com" {...register('email')} />
        <FormError message={errors.email?.message} />

        <Input label="Senha" placeholder="12345678" type="password" {...register('password')} />
        <FormError message={errors.password?.message} />

        <Button className="w-full" type="submit" variant="secondary" disabled={isSubmitting}>
          Criar conta
        </Button>
      </form>

      {submitted && (
        <p className="mt-4 rounded-2xl bg-accent-50 px-4 py-3 text-sm text-accent-600">
          Esqueleto pronto. O envio real para `POST /api/auth/register` fica para a proxima etapa.
        </p>
      )}

      <p className="mt-6 text-sm text-slate-600">
        Ja tem conta?{' '}
        <Link className="font-semibold text-brand-700" to="/login">
          Fazer login
        </Link>
      </p>
    </Card>
  );
}
