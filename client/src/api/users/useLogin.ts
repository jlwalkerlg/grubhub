import { useMutation } from "react-query";
import api, { ApiError } from "../api";
import useAuth, { getAuthUser, UserDto } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

interface LoginResponse {
  xsrfToken: string;
}

export default function useLogin() {
  const { setUser } = useAuth();

  return useMutation<UserDto, ApiError, LoginCommand, null>(async (command) => {
    const {
      data: { xsrfToken },
    } = await api.post<LoginResponse>("/auth/login", command);
    localStorage.setItem("XSRF-TOKEN", xsrfToken);

    const user = await getAuthUser();
    setUser(user);

    return user;
  });
}
