import type { AuthResponse, LoginRequest, RegisterRequest, UserResponse } from '../types/api';
import { apiRequest } from './httpClient';

export function registerUser(payload: RegisterRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/api/auth/register', {
    method: 'POST',
    body: JSON.stringify(payload)
  });
}

export function loginUser(payload: LoginRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/api/auth/login', {
    method: 'POST',
    body: JSON.stringify(payload)
  });
}

export function getMe(): Promise<UserResponse> {
  return apiRequest<UserResponse>('/api/auth/me');
}
