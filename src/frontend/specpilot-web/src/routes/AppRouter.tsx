import { Navigate, Route, Routes } from 'react-router-dom';
import { AuthenticatedLayout } from '../layouts/AuthenticatedLayout';
import { PublicLayout } from '../layouts/PublicLayout';
import { CreateProjectPage } from '../pages/CreateProjectPage';
import { LoginPage } from '../pages/LoginPage';
import { NotFoundPage } from '../pages/NotFoundPage';
import { ProjectDetailsPage } from '../pages/ProjectDetailsPage';
import { ProjectDocumentPage } from '../pages/ProjectDocumentPage';
import { ProjectsPage } from '../pages/ProjectsPage';
import { RegisterPage } from '../pages/RegisterPage';
import { ProtectedRoute } from './ProtectedRoute';
import { PublicOnlyRoute } from './PublicOnlyRoute';

export function AppRouter() {
  return (
    <Routes>
      <Route element={<PublicOnlyRoute />}>
        <Route element={<PublicLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>
      </Route>

      <Route element={<ProtectedRoute />}>
        <Route element={<AuthenticatedLayout />}>
          <Route path="/" element={<Navigate replace to="/projects" />} />
          <Route path="/projects" element={<ProjectsPage />} />
          <Route path="/projects/new" element={<CreateProjectPage />} />
          <Route path="/projects/:id" element={<ProjectDetailsPage />} />
          <Route path="/projects/:id/document" element={<ProjectDocumentPage />} />
        </Route>
      </Route>

      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}
