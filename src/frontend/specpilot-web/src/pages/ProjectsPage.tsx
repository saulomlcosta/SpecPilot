import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';
import { FormError } from '../components/FormError';
import { LoadingState } from '../components/LoadingState';
import { listProjects } from '../services/projects';
import type { ProjectStatus } from '../types/api';
import { getErrorMessage } from '../utils/errorMessage';

const statusLabels: Record<ProjectStatus, string> = {
  Draft: 'Rascunho',
  QuestionsGenerated: 'Perguntas geradas',
  QuestionsAnswered: 'Perguntas respondidas',
  DocumentGenerated: 'Documento gerado'
};

export function ProjectsPage() {
  const projectsQuery = useQuery({
    queryKey: ['projects'],
    queryFn: listProjects
  });

  return (
    <>
      <Card
        title="Area de projetos"
        description="Lista de projetos do usuario autenticado."
      >
        <div className="mb-5 flex justify-end">
          <Link to="/projects/new">
            <Button>Criar projeto</Button>
          </Link>
        </div>

        {projectsQuery.isLoading && <LoadingState description="Carregando projetos..." />}

        {projectsQuery.isError && (
          <FormError
            message={getErrorMessage(
              projectsQuery.error,
              'Nao foi possivel carregar os projetos. Tente novamente.'
            )}
          />
        )}

        {projectsQuery.isSuccess && projectsQuery.data.length === 0 && (
          <EmptyState
            title="Nenhum projeto cadastrado"
            description="Crie seu primeiro projeto para iniciar o fluxo do MVP."
          >
            <Link to="/projects/new">
              <Button variant="primary">Criar primeiro projeto</Button>
            </Link>
          </EmptyState>
        )}

        {projectsQuery.isSuccess && projectsQuery.data.length > 0 && (
          <div className="space-y-4">
            {projectsQuery.data.map((project) => (
              <article key={project.id} className="rounded-3xl border border-slate-200 bg-white p-5">
                <div className="flex flex-wrap items-center justify-between gap-3">
                  <h3 className="text-lg font-semibold text-slate-900">{project.name}</h3>
                  <span className="rounded-full bg-brand-50 px-3 py-1 text-xs font-semibold text-brand-700">
                    {statusLabels[project.status]}
                  </span>
                </div>
                <p className="mt-3 text-sm text-slate-700">
                  <span className="font-semibold">Objetivo:</span> {project.goal}
                </p>
                <p className="mt-2 text-sm text-slate-700">
                  <span className="font-semibold">Publico-alvo:</span> {project.targetAudience}
                </p>
                <div className="mt-4">
                  <Link to={`/projects/${project.id}`}>
                    <Button variant="ghost">Ver detalhes</Button>
                  </Link>
                </div>
              </article>
            ))}
          </div>
        )}
      </Card>
    </>
  );
}
