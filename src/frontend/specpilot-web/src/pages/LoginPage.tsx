import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { loginSchema, type LoginFormValues } from '../schemas/loginSchema';

export function LoginPage() {
  const [submitted, setSubmitted] = useState(false);
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

  const onSubmit = async () => {
    setSubmitted(true);
  };

  return (
    <Card
      title="Entrar no SpecPilot AI"
      description="Este formulario ja usa React Hook Form e Zod, mas a integracao autentica com a API sera conectada no proximo passo."
    >
      <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
        <Input label="Email" placeholder="saulo@example.com" {...register('email')} />
        <FormError message={errors.email?.message} />

        <Input label="Senha" placeholder="12345678" type="password" {...register('password')} />
        <FormError message={errors.password?.message} />

        <Button className="w-full" type="submit" disabled={isSubmitting}>
          Continuar
        </Button>
      </form>

      {submitted && (
        <p className="mt-4 rounded-2xl bg-brand-50 px-4 py-3 text-sm text-brand-800">
          Estrutura pronta. O login funcional sera conectado ao backend em uma etapa posterior.
        </p>
      )}

      <p className="mt-6 text-sm text-slate-600">
        Ainda nao tem conta?{' '}
        <Link className="font-semibold text-brand-700" to="/register">
          Criar cadastro
        </Link>
      </p>
    </Card>
  );
}
