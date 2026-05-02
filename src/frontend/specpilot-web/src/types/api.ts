export interface ProblemDetails {
  title: string;
  detail: string;
  status: number;
  code?: string;
}

export interface UserResponse {
  id: string;
  name: string;
  email: string;
}

export interface AuthResponse {
  token: string;
  user: UserResponse;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export type ProjectStatus = 'Draft' | 'QuestionsGenerated' | 'QuestionsAnswered' | 'DocumentGenerated';

export interface CreateProjectRequest {
  name: string;
  initialDescription: string;
  goal: string;
  targetAudience: string;
}

export interface UpdateProjectRequest {
  name: string;
  initialDescription: string;
  goal: string;
  targetAudience: string;
}

export interface ProjectResponse {
  id: string;
  name: string;
  initialDescription: string;
  goal: string;
  targetAudience: string;
  status: ProjectStatus;
  createdAt: string;
  updatedAt: string | null;
}

export interface RefinementQuestionResponse {
  id: string;
  order: number;
  questionText: string;
  answer: string | null;
}

export type FunctionalRequirement = string;
export type NonFunctionalRequirement = string;
export type UseCase = string;
export type Risk = string;

export interface GenerateQuestionsResponse {
  projectId: string;
  status: ProjectStatus;
  questions: string[];
}

export interface ProjectQuestionsResponse {
  projectId: string;
  status: ProjectStatus;
  questions: RefinementQuestionResponse[];
}

export interface AnswerQuestionItem {
  questionId: string;
  answer: string;
}

export interface AnswerQuestionsRequest {
  answers: AnswerQuestionItem[];
}

export interface AnswerQuestionsResponse {
  projectId: string;
  status: ProjectStatus;
}

export interface ProjectDocumentResponse {
  projectId: string;
  status: ProjectStatus;
  overview: string;
  functionalRequirements: FunctionalRequirement[];
  nonFunctionalRequirements: NonFunctionalRequirement[];
  useCases: UseCase[];
  risks: Risk[];
}

export interface ApiHealthResponse {
  status: 'online';
  checkedAt: string;
  apiBaseUrl: string;
}
