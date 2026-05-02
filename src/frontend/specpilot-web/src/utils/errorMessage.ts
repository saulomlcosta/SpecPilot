import { ApiError } from '../services/httpClient';

function containsTechnicalDetails(message: string): boolean {
  const normalizedMessage = message.trim();

  return (
    normalizedMessage.includes('\n') ||
    normalizedMessage.includes('   at ') ||
    /exception/i.test(normalizedMessage) ||
    /system\./i.test(normalizedMessage) ||
    /stack trace/i.test(normalizedMessage) ||
    /<html/i.test(normalizedMessage) ||
    /failed to fetch/i.test(normalizedMessage)
  );
}

export function getErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof ApiError) {
    if (error.problem.status >= 500) {
      return fallback;
    }

    if (error.problem.detail && !containsTechnicalDetails(error.problem.detail)) {
      return error.problem.detail;
    }

    if (error.problem.title && !containsTechnicalDetails(error.problem.title)) {
      return error.problem.title;
    }
  }

  if (error instanceof Error && error.message && !containsTechnicalDetails(error.message)) {
    return error.message;
  }

  return fallback;
}
