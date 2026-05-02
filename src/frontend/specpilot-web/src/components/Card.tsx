import type { PropsWithChildren } from 'react';
import { cn } from '../utils/cn';

interface CardProps {
  title?: string;
  description?: string;
  className?: string;
}

export function Card({ title, description, className, children }: PropsWithChildren<CardProps>) {
  return (
    <section className={cn('rounded-3xl bg-white/90 p-6 shadow-soft ring-1 ring-white/70', className)}>
      {(title || description) && (
        <header className="mb-5 space-y-2">
          {title && <h2 className="text-xl font-semibold text-slate-900">{title}</h2>}
          {description && <p className="text-sm leading-6 text-slate-600">{description}</p>}
        </header>
      )}
      {children}
    </section>
  );
}
