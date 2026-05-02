import type {
  AnswerQuestionsRequest,
  AnswerQuestionsResponse,
  CreateProjectRequest,
  GenerateQuestionsResponse,
  ProjectDocumentResponse,
  ProjectQuestionsResponse,
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

export function generateQuestions(projectId: string): Promise<GenerateQuestionsResponse> {
  return apiRequest<GenerateQuestionsResponse>(`/api/projects/${projectId}/generate-questions`, {
    method: 'POST'
  });
}

export function getQuestions(projectId: string): Promise<ProjectQuestionsResponse> {
  return apiRequest<ProjectQuestionsResponse>(`/api/projects/${projectId}/questions`);
}

export function answerQuestions(
  projectId: string,
  payload: AnswerQuestionsRequest
): Promise<AnswerQuestionsResponse> {
  return apiRequest<AnswerQuestionsResponse>(`/api/projects/${projectId}/questions/answers`, {
    method: 'PUT',
    body: JSON.stringify(payload)
  });
}

export function generateDocument(projectId: string): Promise<ProjectDocumentResponse> {
  return apiRequest<ProjectDocumentResponse>(`/api/projects/${projectId}/generate-document`, {
    method: 'POST'
  });
}

export function getDocument(projectId: string): Promise<ProjectDocumentResponse> {
  return apiRequest<ProjectDocumentResponse>(`/api/projects/${projectId}/document`);
}
