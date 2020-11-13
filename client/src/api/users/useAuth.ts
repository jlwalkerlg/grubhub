import { useQuery } from "react-query";
import api from "~/api/Api";
import { Error } from "~/services/Error";
import { UserDto } from "./UserDto";

export function getAuthUserQueryKey() {
  return "auth.user";
}

export async function getAuthUser() {
  const response = await api.get<UserDto>("/auth/user");

  if (response.isSuccess) {
    return response.data;
  }

  if (response.error.statusCode === 401) {
    return null;
  }

  throw response.error;
}

export function useAuthUser() {
  return useQuery<UserDto, Error>(getAuthUserQueryKey(), getAuthUser);
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
