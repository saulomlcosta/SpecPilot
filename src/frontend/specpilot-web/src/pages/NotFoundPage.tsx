import { Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Card } from '../components/Card';

export function NotFoundPage() {
  return (
    <div className="mx-auto max-w-xl py-16">
      <Card title="Pagina nao encontrada" description="A rota informada nao faz parte do esqueleto atual do frontend.">
        <Link to="/projects">
          <Button>Ir para projetos</Button>
        </Link>
      </Card>
    </div>
  );
}
