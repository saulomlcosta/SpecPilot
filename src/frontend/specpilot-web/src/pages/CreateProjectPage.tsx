import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { Textarea } from '../components/Textarea';
import { createProjectSchema, type CreateProjectFormValues } from '../schemas/createProjectSchema';
import { createProject } from '../services/projects';
import { getErrorMessage } from '../utils/errorMessage';

export function CreateProjectPage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [apiError, setApiError] = useState<string | null>(null);
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

  const createProjectMutation = useMutation({
    mutationFn: createProject,
    onSuccess: (project) => {
      queryClient.invalidateQueries({ queryKey: ['projects'] });
      navigate(`/projects/${project.id}`, { replace: true });
    }
  });

  const onSubmit = async (values: CreateProjectFormValues) => {
    setApiError(null);
    try {
      await createProjectMutation.mutateAsync(values);
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel criar o projeto. Tente novamente.'));
    }
  };

  return (
    <Card
      title="Novo projeto"
      description="Preencha os dados para criar um novo projeto no MVP."
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

        <Button type="submit" disabled={isSubmitting || createProjectMutation.isPending}>
          {isSubmitting || createProjectMutation.isPending ? 'Criando projeto...' : 'Criar projeto'}
        </Button>

        <FormError message={apiError ?? undefined} />
      </form>
    </Card>
  );
}
