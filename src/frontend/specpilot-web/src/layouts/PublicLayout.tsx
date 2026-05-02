import { Link, Outlet, useLocation } from 'react-router-dom';

export function PublicLayout() {
  const location = useLocation();

  return (
    <div className="min-h-screen px-4 py-8 sm:px-6">
      <div className="mx-auto grid max-w-6xl gap-8 lg:grid-cols-[1.2fr_0.8fr]">
        <section className="rounded-[2rem] bg-brand-900 px-8 py-10 text-white shadow-soft">
          <p className="text-xs font-semibold uppercase tracking-[0.28em] text-brand-200">
            SpecPilot AI
          </p>
          <h1 className="mt-6 max-w-xl font-serif text-4xl leading-tight sm:text-5xl">
            Transforme ideias vagas em uma base de documentacao tecnica inicial.
          </h1>
          <p className="mt-6 max-w-2xl text-sm leading-7 text-brand-100">
            Entre ou crie sua conta para seguir o fluxo principal do MVP: criar projeto, responder
            perguntas de refinamento e revisar o documento tecnico gerado.
          </p>

          <div className="mt-10 grid gap-4 sm:grid-cols-2">
            <div className="rounded-3xl bg-white/10 p-5">
              <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-100">
                Fluxo do MVP
              </p>
              <p className="mt-3 text-sm leading-6 text-brand-50">
                Cadastro, login, projetos, perguntas de refinamento, respostas e documento tecnico.
              </p>
            </div>
            <div className="rounded-3xl bg-white/10 p-5">
              <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-100">
                Backend pronto
              </p>
              <p className="mt-3 text-sm leading-6 text-brand-50">
                API em `localhost:8080`, `FakeAiService` por padrao e contratos HTTP documentados.
              </p>
            </div>
          </div>
        </section>

        <section className="flex flex-col justify-between rounded-[2rem] bg-white/85 p-6 shadow-soft ring-1 ring-white/70">
          <nav className="mb-6 flex gap-3 text-sm">
            <Link
              className={location.pathname === '/login' ? 'font-semibold text-brand-700' : 'text-slate-500'}
              to="/login"
            >
              Login
            </Link>
            <Link
              className={location.pathname === '/register' ? 'font-semibold text-brand-700' : 'text-slate-500'}
              to="/register"
            >
              Cadastro
            </Link>
          </nav>
          <Outlet />
        </section>
      </div>
    </div>
  );
}
