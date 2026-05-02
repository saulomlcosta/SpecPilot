import { Navigate, Outlet } from 'react-router-dom';
import { LoadingState } from '../components/LoadingState';
import { useAuth } from '../contexts/AuthContext';

export function PublicOnlyRoute() {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <LoadingState title="Carregando autenticacao" description="Validando sessao atual." />;
  }

  if (isAuthenticated) {
    return <Navigate replace to="/projects" />;
  }

  return <Outlet />;
}
