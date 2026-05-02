import { Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';
import { LoadingState } from '../components/LoadingState';
import { useApiHealth } from '../hooks/useApiHealth';

export function ProjectsPage() {
  const healthQuery = useApiHealth();

  return (
    <>
      <Card
        title="Area de projetos"
        description="Nesta etapa, a pagina organiza a navegacao do MVP e confirma a conectividade basica com o backend."
      >
        {healthQuery.isLoading && <LoadingState description="Verificando se a API do SpecPilot AI esta acessivel." />}

        {healthQuery.isSuccess && (
          <div className="grid gap-4 md:grid-cols-2">
            <div className="rounded-3xl bg-brand-50 p-5">
              <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-700">API pronta</p>
              <p className="mt-3 text-sm leading-6 text-brand-900">
                Conexao basica validada com `{healthQuery.data.apiBaseUrl}` em {new Date(healthQuery.data.checkedAt).toLocaleTimeString('pt-BR')}.
              </p>
            </div>
            <div className="rounded-3xl bg-stone-100 p-5">
              <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-600">Proximos fluxos</p>
              <p className="mt-3 text-sm leading-6 text-slate-700">
                Listagem de projetos, criacao, consulta do documento e passos de refinamento serao conectados nas proximas entregas.
              </p>
            </div>
          </div>
        )}

        {healthQuery.isError && (
          <EmptyState
            title="API indisponivel"
            description="O frontend foi iniciado, mas a verificacao do backend falhou. Confirme se o Docker Compose subiu a API em http://localhost:8080."
          />
        )}
      </Card>

      <EmptyState
        title="Nenhum projeto conectado ainda"
        description="A estrutura da pagina ja existe, mas a listagem real via GET /api/projects sera implementada em uma etapa funcional futura."
      >
        <Link to="/projects/new">
          <Button variant="primary">Criar primeiro projeto</Button>
        </Link>
      </EmptyState>
    </>
  );
}
