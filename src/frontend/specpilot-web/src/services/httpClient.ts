import type { ProblemDetails } from '../types/api';
import { getStoredAuthToken } from './tokenStorage';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:8080';

export function getApiBaseUrl(): string {
  return API_BASE_URL;
}

export class ApiError extends Error {
  constructor(public readonly problem: ProblemDetails) {
    super(problem.detail || problem.title || 'Nao foi possivel concluir a requisicao.');
  }
}

export async function apiRequest<T>(path: string, init?: RequestInit): Promise<T> {
  const headers = new Headers(init?.headers);
  if (!(init?.body instanceof FormData)) {
    headers.set('Content-Type', 'application/json');
  }

  const token = getStoredAuthToken();
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers
  });

  if (!response.ok) {
    const fallbackProblem: ProblemDetails = {
      title: 'Falha na requisicao',
      detail: 'Nao foi possivel concluir a requisicao agora. Tente novamente.',
      status: response.status
    };

    let problem = fallbackProblem;

    try {
      const responseBody = (await response.json()) as ProblemDetails;
      problem = {
        ...fallbackProblem,
        ...responseBody
      };
    } catch {
      problem = fallbackProblem;
    }

    throw new ApiError(problem);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}
