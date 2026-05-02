import type {
  CreateProjectRequest,
  ProjectResponse,
  UpdateProjectRequest
} from '../types/api';
import { apiRequest } from './httpClient';

export function listProjects(): Promise<ProjectResponse[]> {
  return apiRequest<ProjectResponse[]>('/api/projects');
}

export function getProjectById(id: string): Promise<ProjectResponse> {
  return apiRequest<ProjectResponse>(`/api/projects/${id}`);
}

export function createProject(payload: CreateProjectRequest): Promise<ProjectResponse> {
  return apiRequest<ProjectResponse>('/api/projects', {
    method: 'POST',
    body: JSON.stringify(payload)
  });
}

export function updateProject(id: string, payload: UpdateProjectRequest): Promise<ProjectResponse> {
  return apiRequest<ProjectResponse>(`/api/projects/${id}`, {
    method: 'PUT',
    body: JSON.stringify(payload)
  });
}

export function deleteProject(id: string): Promise<void> {
  return apiRequest<void>(`/api/projects/${id}`, {
    method: 'DELETE'
  });
}
