import { Link, useParams } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';
import { EmptyState } from '../components/EmptyState';

export function ProjectDocumentPage() {
  const { id } = useParams();

  return (
    <>
      <Card
        title="Documento tecnico do projeto"
        description="O backend ja expoe `GET /api/projects/{id}/document`. Esta pagina prepara a estrutura visual para receber overview, requisitos, casos de uso e riscos."
      >
        <div className="grid gap-4 md:grid-cols-2">
          <div className="rounded-3xl bg-brand-50 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-700">Projeto atual</p>
            <p className="mt-3 text-sm text-brand-900">{id ?? 'Sem identificador'}</p>
          </div>
          <div className="rounded-3xl bg-stone-100 p-5">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-slate-500">Origem dos dados</p>
            <p className="mt-3 text-sm text-slate-700">GET /api/projects/{id}/document</p>
          </div>
        </div>
      </Card>

      <EmptyState
        title="Documento ainda nao renderizado"
        description="A estrutura visual esta pronta para receber os dados reais assim que o fluxo de consumo da API for implementado no frontend."
      >
        <Link to={`/projects/${id}`}>
          <Button variant="secondary">Voltar ao projeto</Button>
        </Link>
      </EmptyState>
    </>
  );
}
