import { useQuery } from '@tanstack/react-query';
import { Link, useParams } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';
import { LoadingState } from '../components/LoadingState';
import { ApiError } from '../services/httpClient';
import { getDocument } from '../services/projects';
import { getErrorMessage } from '../utils/errorMessage';

export function ProjectDocumentPage() {
  const { id } = useParams();
  const projectId = id ?? '';

  const documentQuery = useQuery({
    queryKey: ['projects', projectId, 'document'],
    queryFn: () => getDocument(projectId),
    enabled: Boolean(projectId),
    retry: false
  });

  if (!projectId) {
    return (
      <EmptyState title="Projeto invalido" description="Nao foi possivel identificar o projeto solicitado.">
        <Link to="/projects">
          <Button variant="ghost">Voltar para projetos</Button>
        </Link>
      </EmptyState>
    );
  }

  if (documentQuery.isLoading) {
    return <LoadingState description="Carregando documento do projeto..." />;
  }

  if (documentQuery.isError) {
    const isNotFound = documentQuery.error instanceof ApiError && documentQuery.error.problem.status === 404;

    return (
      <EmptyState
        title={isNotFound ? 'Documento ainda nao gerado' : 'Falha ao carregar documento'}
        description={
          isNotFound
            ? 'Gere o documento no detalhe do projeto para visualizar esta pagina.'
            : getErrorMessage(documentQuery.error, 'Nao foi possivel carregar o documento agora.')
        }
      >
        <Link to={`/projects/${projectId}`}>
          <Button variant="secondary">Voltar ao projeto</Button>
        </Link>
      </EmptyState>
    );
  }

  const document = documentQuery.data;

  if (!document) {
    return <LoadingState description="Carregando documento do projeto..." />;
  }

  return (
    <>
      <Card title="Documento tecnico inicial" description="Resultado estruturado do fluxo de IA do MVP.">
        <div className="space-y-5">
          <section>
            <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">Visao geral</h3>
            <p className="mt-2 text-sm leading-6 text-slate-800">{document.overview}</p>
          </section>

          <section>
            <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
              Requisitos funcionais
            </h3>
            <ul className="mt-2 list-disc space-y-1 pl-5 text-sm leading-6 text-slate-800">
              {document.functionalRequirements.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </section>

          <section>
            <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">
              Requisitos nao funcionais
            </h3>
            <ul className="mt-2 list-disc space-y-1 pl-5 text-sm leading-6 text-slate-800">
              {document.nonFunctionalRequirements.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </section>

          <section>
            <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">Casos de uso</h3>
            <ul className="mt-2 list-disc space-y-1 pl-5 text-sm leading-6 text-slate-800">
              {document.useCases.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </section>

          <section>
            <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-slate-500">Riscos</h3>
            <ul className="mt-2 list-disc space-y-1 pl-5 text-sm leading-6 text-slate-800">
              {document.risks.map((item) => (
                <li key={item}>{item}</li>
              ))}
            </ul>
          </section>
        </div>
      </Card>

      <div>
        <Link to={`/projects/${projectId}`}>
          <Button variant="ghost">Voltar ao projeto</Button>
        </Link>
      </div>
    </>
  );
}
