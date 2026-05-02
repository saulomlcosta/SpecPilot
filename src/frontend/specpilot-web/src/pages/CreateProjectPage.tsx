import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { Textarea } from '../components/Textarea';
import { createProjectSchema, type CreateProjectFormValues } from '../schemas/createProjectSchema';

export function CreateProjectPage() {
  const [submitted, setSubmitted] = useState(false);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<CreateProjectFormValues>({
    resolver: zodResolver(createProjectSchema),
    defaultValues: {
      name: '',
      initialDescription: '',
      goal: '',
      targetAudience: ''
    }
  });

  const onSubmit = async () => {
    setSubmitted(true);
  };

  return (
    <Card
      title="Novo projeto"
      description="A interface inicial ja segue o contrato do backend para criacao de projeto, sem enviar dados reais nesta etapa."
    >
      <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
        <Input label="Nome do projeto" placeholder="Sistema de clinica" {...register('name')} />
        <FormError message={errors.name?.message} />

        <Textarea
          label="Descricao inicial"
          placeholder="Quero um sistema para agendamento, prontuario e notificacoes."
          {...register('initialDescription')}
        />
        <FormError message={errors.initialDescription?.message} />

        <Textarea label="Objetivo" placeholder="Organizar o atendimento da clinica." {...register('goal')} />
        <FormError message={errors.goal?.message} />

        <Input
          label="Publico-alvo"
          placeholder="Equipe administrativa e medica"
          {...register('targetAudience')}
        />
        <FormError message={errors.targetAudience?.message} />

        <Button type="submit" disabled={isSubmitting}>
          Salvar estrutura
        </Button>
      </form>

      {submitted && (
        <p className="mt-4 rounded-2xl bg-brand-50 px-4 py-3 text-sm text-brand-800">
          Estrutura do formulario pronta. O envio real para `POST /api/projects` sera conectado depois.
        </p>
      )}
    </Card>
  );
}
