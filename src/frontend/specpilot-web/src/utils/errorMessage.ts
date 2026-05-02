import { ApiError } from '../services/httpClient';

export function getErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof ApiError) {
    if (error.problem.detail) {
      return error.problem.detail;
    }

    if (error.problem.title) {
      return error.problem.title;
    }
  }

  if (error instanceof Error && error.message) {
    return error.message;
  }

  return fallback;
}
