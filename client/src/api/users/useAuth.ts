import { useQuery } from "react-query";
import api, { ApiError } from "~/api/Api";
import { UserDto } from "./UserDto";

export function getAuthUserQueryKey() {
  return "auth.user";
}

export async function getAuthUser() {
  try {
    const response = await api.get<UserDto>("/auth/user");
    return response.data;
  } catch (e) {
    if (e instanceof ApiError && e.statusCode === 401) {
      return null;
    }

    throw e;
  }
}

export function useAuthUser() {
  return useQuery<UserDto, ApiError>(getAuthUserQueryKey(), getAuthUser, {
    staleTime: Infinity,
    retry: false,
    refetchOnWindowFocus: false,
  });
}

export default function useAuth() {
  const { data: user, isLoading } = useAuthUser();

  const isLoggedIn = !!user;

  return {
    user,
    isLoggedIn,
    isLoading,
  };
}
