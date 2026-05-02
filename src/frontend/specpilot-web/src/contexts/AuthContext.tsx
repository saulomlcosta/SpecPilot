import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type PropsWithChildren
} from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { getMe, loginUser, registerUser } from '../services/auth';
import { ApiError } from '../services/httpClient';
import { clearStoredAuthToken, getStoredAuthToken, setStoredAuthToken } from '../services/tokenStorage';
import type { LoginRequest, RegisterRequest, UserResponse } from '../types/api';

interface AuthContextValue {
  user: UserResponse | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (payload: LoginRequest) => Promise<void>;
  register: (payload: RegisterRequest) => Promise<void>;
  logout: () => void;
  refresh: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: PropsWithChildren) {
  const queryClient = useQueryClient();
  const [user, setUser] = useState<UserResponse | null>(null);
  const [isBootstrapping, setIsBootstrapping] = useState(true);
  const [token, setToken] = useState<string | null>(() => getStoredAuthToken());

  const meQuery = useQuery({
    queryKey: ['auth', 'me', token],
    queryFn: getMe,
    enabled: Boolean(token),
    retry: false
  });

  useEffect(() => {
    if (!token) {
      setUser(null);
      setIsBootstrapping(false);
      return;
    }

    if (meQuery.isSuccess) {
      setUser(meQuery.data);
      setIsBootstrapping(false);
      return;
    }

    if (meQuery.isError) {
      clearStoredAuthToken();
      setToken(null);
      setUser(null);
      setIsBootstrapping(false);
    }
  }, [meQuery.data, meQuery.isError, meQuery.isSuccess, token]);

  const loginMutation = useMutation({
    mutationFn: loginUser
  });

  const registerMutation = useMutation({
    mutationFn: registerUser
  });

  const handleAuthenticated = useCallback(
    (nextToken: string, nextUser: UserResponse) => {
      setStoredAuthToken(nextToken);
      setToken(nextToken);
      setUser(nextUser);
      queryClient.setQueryData(['auth', 'me', nextToken], nextUser);
    },
    [queryClient]
  );

  const login = useCallback(
    async (payload: LoginRequest) => {
      const response = await loginMutation.mutateAsync(payload);
      handleAuthenticated(response.token, response.user);
    },
    [handleAuthenticated, loginMutation]
  );

  const register = useCallback(
    async (payload: RegisterRequest) => {
      const response = await registerMutation.mutateAsync(payload);
      handleAuthenticated(response.token, response.user);
    },
    [handleAuthenticated, registerMutation]
  );

  const logout = useCallback(() => {
    clearStoredAuthToken();
    setToken(null);
    setUser(null);
    queryClient.removeQueries({ queryKey: ['auth', 'me'] });
  }, [queryClient]);

  const refresh = useCallback(async () => {
    if (!getStoredAuthToken()) {
      setUser(null);
      return;
    }

    try {
      const refreshedUser = await queryClient.fetchQuery({
        queryKey: ['auth', 'me', getStoredAuthToken()],
        queryFn: getMe
      });
      setUser(refreshedUser);
    } catch (error) {
      if (error instanceof ApiError) {
        logout();
      }
      throw error;
    }
  }, [logout, queryClient]);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      isLoading: isBootstrapping || meQuery.isFetching || loginMutation.isPending || registerMutation.isPending,
      login,
      register,
      logout,
      refresh
    }),
    [
      isBootstrapping,
      login,
      loginMutation.isPending,
      meQuery.isFetching,
      refresh,
      register,
      registerMutation.isPending,
      user,
      logout
    ]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error('useAuth deve ser usado dentro de AuthProvider.');
  }

  return context;
}
