import type { PropsWithChildren } from 'react';

interface EmptyStateProps {
  title: string;
  description: string;
}

export function EmptyState({ title, description, children }: PropsWithChildren<EmptyStateProps>) {
  return (
    <div className="rounded-3xl border border-dashed border-slate-300 bg-white/70 p-8 text-center">
      <h3 className="text-lg font-semibold text-slate-900">{title}</h3>
      <p className="mx-auto mt-3 max-w-xl text-sm leading-6 text-slate-600">{description}</p>
      {children && <div className="mt-5">{children}</div>}
    </div>
  );
}
