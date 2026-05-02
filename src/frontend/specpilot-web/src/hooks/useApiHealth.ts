import { useQuery } from '@tanstack/react-query';
import { getApiHealth } from '../services/health';

export function useApiHealth() {
  return useQuery({
    queryKey: ['api-health'],
    queryFn: getApiHealth,
    staleTime: 30_000
  });
}
