interface LoadingStateProps {
  title?: string;
  description?: string;
}

export function LoadingState({
  title = 'Carregando contexto',
  description = 'A interface esta preparando os dados iniciais.'
}: LoadingStateProps) {
  return (
    <div className="rounded-3xl border border-dashed border-brand-300 bg-brand-50/80 p-6 text-brand-900">
      <p className="text-sm font-semibold uppercase tracking-[0.24em]">{title}</p>
      <p className="mt-3 text-sm leading-6 text-brand-800">{description}</p>
    </div>
  );
}
