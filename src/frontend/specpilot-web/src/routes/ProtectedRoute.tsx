import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { LoadingState } from '../components/LoadingState';
import { useAuth } from '../contexts/AuthContext';

export function ProtectedRoute() {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <LoadingState title="Carregando autenticacao" description="Validando sessao atual." />;
  }

  if (!isAuthenticated) {
    return <Navigate replace to="/login" state={{ from: location }} />;
  }

  return <Outlet />;
}
