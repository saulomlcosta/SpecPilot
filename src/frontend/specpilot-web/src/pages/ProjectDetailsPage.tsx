import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';
import { FormError } from '../components/FormError';
import { Input } from '../components/Input';
import { LoadingState } from '../components/LoadingState';
import { Notice } from '../components/Notice';
import { Textarea } from '../components/Textarea';
import {
  answerQuestionsSchema,
  type AnswerQuestionsFormValues
} from '../schemas/answerQuestionsSchema';
import { updateProjectSchema, type UpdateProjectFormValues } from '../schemas/updateProjectSchema';
import { ApiError } from '../services/httpClient';
import {
  answerQuestions,
  deleteProject,
  generateDocument,
  generateQuestions,
  getProjectById,
  getQuestions,
  updateProject
} from '../services/projects';
import type { ProjectStatus } from '../types/api';
import { getErrorMessage } from '../utils/errorMessage';
import { getProjectStatusMeta } from '../utils/projectStatus';

const orderedStatuses: ProjectStatus[] = [
  'Draft',
  'QuestionsGenerated',
  'QuestionsAnswered',
  'DocumentGenerated'
];

export function ProjectDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const queryClient = useQueryClient();
  const [apiError, setApiError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const projectId = id ?? '';

  const projectQuery = useQuery({
    queryKey: ['projects', projectId],
    queryFn: () => getProjectById(projectId),
    enabled: Boolean(projectId),
    retry: false
  });

  const questionsQuery = useQuery({
    queryKey: ['projects', projectId, 'questions'],
    queryFn: () => getQuestions(projectId),
    enabled: Boolean(projectId) && projectQuery.data?.status === 'QuestionsGenerated',
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

  const answersForm = useForm<AnswerQuestionsFormValues>({
    resolver: zodResolver(answerQuestionsSchema),
    defaultValues: {
      answers: []
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

  useEffect(() => {
    if (!questionsQuery.data) {
      return;
    }

    answersForm.reset({
      answers: questionsQuery.data.questions.map((question) => ({
        questionId: question.id,
        answer: question.answer ?? ''
      }))
    });
  }, [answersForm, questionsQuery.data]);

  const updateProjectMutation = useMutation({
    mutationFn: (values: UpdateProjectFormValues) => updateProject(projectId, values),
    onSuccess: (project) => {
      queryClient.setQueryData(['projects', projectId], project);
      queryClient.invalidateQueries({ queryKey: ['projects'] });
      setSuccessMessage('Projeto atualizado com sucesso.');
    }
  });

  const deleteProjectMutation = useMutation({
    mutationFn: () => deleteProject(projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects'] });
      navigate('/projects', { replace: true });
    }
  });

  const generateQuestionsMutation = useMutation({
    mutationFn: () => generateQuestions(projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects', projectId] });
      queryClient.invalidateQueries({ queryKey: ['projects', projectId, 'questions'] });
      setSuccessMessage('Perguntas geradas. Agora responda todas para continuar.');
    }
  });

  const answerQuestionsMutation = useMutation({
    mutationFn: answerQuestions.bind(null, projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects', projectId] });
      queryClient.invalidateQueries({ queryKey: ['projects', projectId, 'questions'] });
      setSuccessMessage('Perguntas respondidas com sucesso.');
    }
  });

  const generateDocumentMutation = useMutation({
    mutationFn: () => generateDocument(projectId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects', projectId] });
      queryClient.invalidateQueries({ queryKey: ['projects', projectId, 'document'] });
      navigate(`/projects/${projectId}/document`, { state: { generated: true } });
    }
  });

  const clearFeedback = () => {
    setApiError(null);
    setSuccessMessage(null);
  };

  const onSubmit = async (values: UpdateProjectFormValues) => {
    clearFeedback();

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

    clearFeedback();

    try {
      await deleteProjectMutation.mutateAsync();
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel excluir o projeto. Tente novamente.'));
    }
  };

  const onGenerateQuestions = async () => {
    clearFeedback();

    try {
      await generateQuestionsMutation.mutateAsync();
    } catch (error) {
      setApiError(
        getErrorMessage(error, 'Nao foi possivel gerar perguntas de refinamento. Tente novamente.')
      );
    }
  };

  const onAnswerQuestions = async (values: AnswerQuestionsFormValues) => {
    clearFeedback();

    const hasEmptyAnswer = values.answers.some((item) => !item.answer.trim());
    if (hasEmptyAnswer) {
      setApiError('Responda todas as perguntas para continuar.');
      return;
    }

    try {
      await answerQuestionsMutation.mutateAsync({
        answers: values.answers.map((item) => ({
          questionId: item.questionId,
          answer: item.answer.trim()
        }))
      });
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel enviar as respostas. Tente novamente.'));
    }
  };

  const onGenerateDocument = async () => {
    clearFeedback();

    try {
      await generateDocumentMutation.mutateAsync();
    } catch (error) {
      setApiError(getErrorMessage(error, 'Nao foi possivel gerar o documento. Tente novamente.'));
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
    const notFound = projectQuery.error instanceof ApiError && projectQuery.error.problem.status === 404;

    return (
      <EmptyState
        title={notFound ? 'Projeto nao encontrado' : 'Falha ao carregar projeto'}
        description={
          notFound
            ? 'Este projeto nao existe ou nao pertence ao usuario autenticado.'
            : getErrorMessage(projectQuery.error, 'Nao foi possivel carregar os detalhes do projeto agora.')
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

  const currentStatus = getProjectStatusMeta(project.status);
  const isUpdatingProject = isSubmitting || updateProjectMutation.isPending;

  return (
    <>
      <Card
        title="Detalhes do projeto"
        description="Acompanhe o status atual e execute apenas a proxima acao compativel."
      >
        <div className="mb-5 flex flex-wrap gap-3">
          <Link to="/projects">
            <Button variant="ghost">Voltar para projetos</Button>
          </Link>
          {project.status === 'DocumentGenerated' && (
            <Link to={`/projects/${project.id}/document`}>
              <Button variant="secondary">Ir para o documento</Button>
            </Link>
          )}
        </div>

        <div className="grid gap-4 md:grid-cols-2">
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Nome</p>
            <p className="mt-3 text-sm text-slate-800">{project.name}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Status</p>
            <p className="mt-3 text-sm text-slate-800">{currentStatus.label}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Criado em</p>
            <p className="mt-3 text-sm text-slate-800">{new Date(project.createdAt).toLocaleString('pt-BR')}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Ultima atualizacao</p>
            <p className="mt-3 text-sm text-slate-800">
              {project.updatedAt ? new Date(project.updatedAt).toLocaleString('pt-BR') : 'Sem atualizacao'}
            </p>
          </div>
        </div>

        <div className="mt-5 rounded-2xl bg-brand-50 p-4 text-sm text-brand-900">
          <p>{currentStatus.hint}</p>
          <p className="mt-2 font-semibold">Proximo passo: {currentStatus.nextStep}</p>
        </div>

        <div className="mt-5 grid gap-3 md:grid-cols-2 xl:grid-cols-4">
          {orderedStatuses.map((status) => {
            const statusMeta = getProjectStatusMeta(status);
            const isCurrentStep = project.status === status;

            return (
              <div
                key={status}
                className={
                  isCurrentStep
                    ? 'rounded-2xl border border-brand-300 bg-brand-50 p-4'
                    : 'rounded-2xl border border-slate-200 bg-white p-4'
                }
              >
                <p className="text-xs font-semibold uppercase tracking-[0.2em] text-slate-500">Etapa</p>
                <p className="mt-2 text-sm font-semibold text-slate-900">{statusMeta.label}</p>
              </div>
            );
          })}
        </div>

        <div className="mt-4 flex flex-wrap gap-3">
          {project.status === 'Draft' && (
            <Button onClick={onGenerateQuestions} disabled={generateQuestionsMutation.isPending}>
              {generateQuestionsMutation.isPending ? 'Gerando perguntas...' : 'Gerar perguntas de refinamento'}
            </Button>
          )}

          {project.status === 'QuestionsAnswered' && (
            <Button onClick={onGenerateDocument} disabled={generateDocumentMutation.isPending}>
              {generateDocumentMutation.isPending ? 'Gerando documento...' : 'Gerar documento'}
            </Button>
          )}

          {project.status === 'DocumentGenerated' && (
            <Link to={`/projects/${project.id}/document`}>
              <Button variant="ghost">Visualizar documento</Button>
            </Link>
          )}
        </div>

        <FormError message={apiError ?? undefined} />
        {successMessage && <Notice variant="success">{successMessage}</Notice>}
      </Card>

      {project.status === 'QuestionsGenerated' && (
        <Card
          title="Responder perguntas de refinamento"
          description="Preencha todas as respostas para liberar a geracao do documento tecnico."
        >
          {questionsQuery.isLoading && <LoadingState description="Carregando perguntas geradas..." />}

          {questionsQuery.isError && (
            <FormError
              message={getErrorMessage(
                questionsQuery.error,
                'Nao foi possivel carregar as perguntas do projeto.'
              )}
            />
          )}

          {questionsQuery.isSuccess && questionsQuery.data.questions.length === 0 && (
            <EmptyState
              title="Perguntas ainda nao disponiveis"
              description="As perguntas ainda nao foram disponibilizadas. Atualize a pagina ou tente gerar novamente mais tarde."
            />
          )}

          {questionsQuery.isSuccess && questionsQuery.data.questions.length > 0 && (
            <form className="space-y-4" onSubmit={answersForm.handleSubmit(onAnswerQuestions)}>
              {questionsQuery.data.questions.map((question, index) => (
                <div key={question.id} className="space-y-2 rounded-2xl border border-slate-200 p-4">
                  <input type="hidden" {...answersForm.register(`answers.${index}.questionId`)} />
                  <p className="text-sm font-semibold text-slate-800">
                    {question.order}. {question.questionText}
                  </p>
                  <Textarea
                    label="Resposta"
                    rows={3}
                    disabled={answerQuestionsMutation.isPending}
                    {...answersForm.register(`answers.${index}.answer`)}
                  />
                  <FormError message={answersForm.formState.errors.answers?.[index]?.answer?.message} />
                </div>
              ))}

              <Button type="submit" disabled={answerQuestionsMutation.isPending}>
                {answerQuestionsMutation.isPending ? 'Enviando respostas...' : 'Enviar respostas'}
              </Button>

              <FormError message={apiError ?? undefined} />
            </form>
          )}
        </Card>
      )}

      <Card
        title="Editar projeto"
        description="O status e controlado pelo backend e nao pode ser alterado manualmente."
      >
        <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
          <Input label="Nome do projeto" disabled={isUpdatingProject} {...register('name')} />
          <FormError message={errors.name?.message} />

          <Textarea label="Descricao inicial" disabled={isUpdatingProject} {...register('initialDescription')} />
          <FormError message={errors.initialDescription?.message} />

          <Textarea label="Objetivo" disabled={isUpdatingProject} {...register('goal')} />
          <FormError message={errors.goal?.message} />

          <Input label="Publico-alvo" disabled={isUpdatingProject} {...register('targetAudience')} />
          <FormError message={errors.targetAudience?.message} />

          <div className="flex flex-wrap gap-3">
            <Button type="submit" disabled={isUpdatingProject}>
              {isUpdatingProject ? 'Salvando...' : 'Salvar alteracoes'}
            </Button>
            <Button type="button" variant="ghost" onClick={() => reset()} disabled={isUpdatingProject}>
              Descartar edicao
            </Button>
          </div>
          <FormError message={apiError ?? undefined} />
        </form>
      </Card>

      <Card title="Zona de risco" description="Esta acao remove o projeto de forma permanente.">
        <Button type="button" variant="secondary" onClick={onDelete} disabled={deleteProjectMutation.isPending}>
          {deleteProjectMutation.isPending ? 'Excluindo...' : 'Excluir projeto'}
        </Button>
      </Card>
    </>
  );
}
