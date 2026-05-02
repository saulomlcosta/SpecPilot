import type { ApiHealthResponse } from '../types/api';
import { getApiBaseUrl } from './httpClient';

export async function getApiHealth(): Promise<ApiHealthResponse> {
  const response = await fetch(`${getApiBaseUrl()}/health`);

  if (!response.ok) {
    throw new Error('Nao foi possivel verificar a disponibilidade da API.');
  }

  return {
    status: 'online',
    checkedAt: new Date().toISOString(),
    apiBaseUrl: getApiBaseUrl()
  };
}
