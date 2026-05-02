import { Link, useParams } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';

export function ProjectDetailsPage() {
  const { id } = useParams();

  return (
    <>
      <Card
        title="Detalhes do projeto"
        description="Esta pagina vai concentrar a visualizacao do projeto, o disparo da geracao de perguntas e o acompanhamento do status do fluxo."
      >
        <div className="grid gap-4 md:grid-cols-3">
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Projeto</p>
            <p className="mt-3 text-sm text-slate-800">{id ?? 'Sem identificador'}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Status inicial</p>
            <p className="mt-3 text-sm text-slate-800">Draft</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Proxima etapa</p>
            <p className="mt-3 text-sm text-slate-800">Conectar leitura do projeto e a geracao de perguntas.</p>
          </div>
        </div>
      </Card>

      <EmptyState
        title="Fluxo de refinamento ainda nao conectado"
        description="O backend ja suporta geracao e resposta das perguntas. Nesta etapa, a pagina serve como estrutura de navegacao para a implementacao funcional futura."
      >
        <Link to={`/projects/${id}/document`}>
          <Button variant="ghost">Ver pagina do documento</Button>
        </Link>
      </EmptyState>
    </>
  );
}
