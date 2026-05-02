import type { PropsWithChildren } from 'react';
import { cn } from '../utils/cn';

type NoticeVariant = 'info' | 'success';

interface NoticeProps {
  title?: string;
  variant?: NoticeVariant;
}

const variants: Record<NoticeVariant, string> = {
  info: 'border-sky-200 bg-sky-50 text-sky-900',
  success: 'border-emerald-200 bg-emerald-50 text-emerald-900'
};

export function Notice({ children, title, variant = 'info' }: PropsWithChildren<NoticeProps>) {
  return (
    <div className={cn('rounded-2xl border px-4 py-3 text-sm', variants[variant])}>
      {title && <p className="font-semibold">{title}</p>}
      <p className={title ? 'mt-1 leading-6' : 'leading-6'}>{children}</p>
    </div>
  );
}
