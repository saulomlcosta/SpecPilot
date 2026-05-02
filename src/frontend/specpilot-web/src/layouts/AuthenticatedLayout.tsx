import { Link, NavLink, Outlet } from 'react-router-dom';
import { useApiHealth } from '../hooks/useApiHealth';

const navItems = [
  { to: '/projects', label: 'Projetos' },
  { to: '/projects/new', label: 'Novo projeto' }
];

export function AuthenticatedLayout() {
  const healthQuery = useApiHealth();

  return (
    <div className="min-h-screen px-4 py-6 sm:px-6">
      <div className="mx-auto max-w-6xl">
        <header className="mb-6 flex flex-col gap-4 rounded-[2rem] bg-white/90 p-5 shadow-soft ring-1 ring-white/70 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <Link to="/projects" className="text-xs font-semibold uppercase tracking-[0.28em] text-brand-700">
              SpecPilot AI
            </Link>
            <h1 className="mt-2 font-serif text-3xl text-slate-900">Esqueleto do frontend do MVP</h1>
            <p className="mt-2 text-sm text-slate-600">
              Navegacao inicial para o fluxo de autenticacao, projetos, refinamento e documento tecnico.
            </p>
          </div>

          <div className="rounded-2xl bg-stone-100 px-4 py-3 text-sm text-slate-600">
            API:{' '}
            {healthQuery.isSuccess ? (
              <span className="font-semibold text-brand-700">online</span>
            ) : healthQuery.isLoading ? (
              <span className="font-semibold text-slate-700">verificando</span>
            ) : (
              <span className="font-semibold text-rose-600">indisponivel</span>
            )}
          </div>
        </header>

        <div className="grid gap-6 lg:grid-cols-[240px_1fr]">
          <aside className="rounded-[2rem] bg-white/80 p-4 shadow-soft ring-1 ring-white/70">
            <nav className="space-y-2">
              {navItems.map((item) => (
                <NavLink
                  key={item.to}
                  to={item.to}
                  className={({ isActive }) =>
                    isActive
                      ? 'block rounded-2xl bg-brand-700 px-4 py-3 text-sm font-medium text-white'
                      : 'block rounded-2xl px-4 py-3 text-sm font-medium text-slate-700 transition hover:bg-stone-100'
                  }
                >
                  {item.label}
                </NavLink>
              ))}
            </nav>
          </aside>

          <main className="space-y-6">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}
