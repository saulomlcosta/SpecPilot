import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { LoadingState } from '../components/LoadingState';
import { Textarea } from '../components/Textarea';
import { updateProjectSchema, type UpdateProjectFormValues } from '../schemas/updateProjectSchema';
import { ApiError } from '../services/httpClient';
import { deleteProject, getProjectById, updateProject } from '../services/projects';
import type { ProjectStatus } from '../types/api';
import { getErrorMessage } from '../utils/errorMessage';

const statusHints: Record<ProjectStatus, string> = {
  Draft: 'Projeto em rascunho. A geracao de perguntas sera conectada na proxima etapa.',
  QuestionsGenerated: 'Perguntas geradas. O formulario de respostas sera conectado na proxima etapa.',
  QuestionsAnswered: 'Perguntas respondidas. A geracao de documento sera conectada na proxima etapa.',
  DocumentGenerated: 'Documento gerado. Voce ja pode abrir a pagina de documento.'
};

const statusLabels: Record<ProjectStatus, string> = {
  Draft: 'Rascunho',
  QuestionsGenerated: 'Perguntas geradas',
  QuestionsAnswered: 'Perguntas respondidas',
  DocumentGenerated: 'Documento gerado'
};

export function ProjectDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const queryClient = useQueryClient();
  const [apiError, setApiError] = useState<string | null>(null);

  const projectId = useMemo(() => id ?? '', [id]);

  const projectQuery = useQuery({
    queryKey: ['projects', projectId],
    queryFn: () => getProjectById(projectId),
    enabled: Boolean(projectId),
    retry: false
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting }
  } = useForm<UpdateProjectFormValues>({
    resolver: zodResolver(updateProjectSchema),
    defaultValues: {
      name: '',
      initialDescription: '',
      goal: '',
      targetAudience: ''
    }
  });

  useEffect(() => {
    if (!projectQuery.data) {
      return;
    }

    reset({
      name: projectQuery.data.name,
      initialDescription: projectQuery.data.initialDescription,
      goal: projectQuery.data.goal,
      targetAudience: projectQuery.data.targetAudience
    });
  }, [projectQuery.data, reset]);

  const updateProjectMutation = useMutation({
    mutationFn: (values: UpdateProjectFormValues) => updateProject(projectId, values),
    onSuccess: (project) => {
      queryClient.setQueryData(['projects', projectId], project);
      queryClient.invalidateQueries({ queryKey: ['projects'] });
    }
  });

  const deleteProjectMutation = useMutation({
    mutationFn: () => deleteProject(projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects'] });
      navigate('/projects', { replace: true });
    }
  });

  const onSubmit = async (values: UpdateProjectFormValues) => {
    setApiError(null);
    try {
      await updateProjectMutation.mutateAsync(values);
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel atualizar o projeto. Tente novamente.'));
    }
  };

  const onDelete = async () => {
    const confirmed = window.confirm('Deseja realmente excluir este projeto?');
    if (!confirmed) {
      return;
    }

    setApiError(null);
    try {
      await deleteProjectMutation.mutateAsync();
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel excluir o projeto. Tente novamente.'));
    }
  };

  if (!projectId) {
    return (
      <EmptyState title="Projeto invalido" description="Nao foi possivel identificar o projeto solicitado.">
        <Link to="/projects">
          <Button variant="ghost">Voltar para projetos</Button>
        </Link>
      </EmptyState>
    );
  }

  if (projectQuery.isLoading) {
    return <LoadingState description="Carregando detalhes do projeto..." />;
  }

  if (projectQuery.isError) {
    const notFound =
      projectQuery.error instanceof ApiError &&
      projectQuery.error.problem.status === 404;

    return (
      <EmptyState
        title={notFound ? 'Projeto nao encontrado' : 'Falha ao carregar projeto'}
        description={
          notFound
            ? 'Este projeto nao existe ou nao pertence ao usuario autenticado.'
            : getErrorMessage(
                projectQuery.error,
                'Nao foi possivel carregar os detalhes do projeto agora.'
              )
        }
      >
        <Link to="/projects">
          <Button variant="ghost">Voltar para projetos</Button>
        </Link>
      </EmptyState>
    );
  }

  const project = projectQuery.data;

  if (!project) {
    return <LoadingState description="Carregando detalhes do projeto..." />;
  }

  return (
    <>
      <Card
        title="Detalhes do projeto"
        description="Dados do projeto e edicao dos campos permitidos pelo backend."
      >
        <div className="grid gap-4 md:grid-cols-2">
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Nome</p>
            <p className="mt-3 text-sm text-slate-800">{project.name}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Status</p>
            <p className="mt-3 text-sm text-slate-800">{statusLabels[project.status]}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Criado em</p>
            <p className="mt-3 text-sm text-slate-800">
              {new Date(project.createdAt).toLocaleString('pt-BR')}
            </p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">
              Ultima atualizacao
            </p>
            <p className="mt-3 text-sm text-slate-800">
              {project.updatedAt ? new Date(project.updatedAt).toLocaleString('pt-BR') : 'Sem atualizacao'}
            </p>
          </div>
        </div>

        <div className="mt-5 rounded-2xl bg-brand-50 p-4 text-sm text-brand-900">
          {statusHints[project.status]}
        </div>

        {project.status === 'DocumentGenerated' && (
          <div className="mt-4">
            <Link to={`/projects/${project.id}/document`}>
              <Button variant="ghost">Abrir documento</Button>
            </Link>
          </div>
        )}
      </Card>

      <Card title="Editar projeto" description="O status e controlado pelo backend e nao pode ser alterado manualmente.">
        <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
          <Input label="Nome do projeto" {...register('name')} />
          <FormError message={errors.name?.message} />

          <Textarea label="Descricao inicial" {...register('initialDescription')} />
          <FormError message={errors.initialDescription?.message} />

          <Textarea label="Objetivo" {...register('goal')} />
          <FormError message={errors.goal?.message} />

          <Input label="Publico-alvo" {...register('targetAudience')} />
          <FormError message={errors.targetAudience?.message} />

          <div className="flex flex-wrap gap-3">
            <Button type="submit" disabled={isSubmitting || updateProjectMutation.isPending}>
              {isSubmitting || updateProjectMutation.isPending ? 'Salvando...' : 'Salvar alteracoes'}
            </Button>
            <Button
              type="button"
              variant="ghost"
              onClick={() => reset()}
              disabled={updateProjectMutation.isPending}
            >
              Descartar edicao
            </Button>
          </div>
          <FormError message={apiError ?? undefined} />
        </form>
      </Card>

      <Card title="Zona de risco" description="Esta acao remove o projeto de forma permanente.">
        <Button
          type="button"
          variant="secondary"
          onClick={onDelete}
          disabled={deleteProjectMutation.isPending}
        >
          {deleteProjectMutation.isPending ? 'Excluindo...' : 'Excluir projeto'}
        </Button>
      </Card>
    </>
  );
}
