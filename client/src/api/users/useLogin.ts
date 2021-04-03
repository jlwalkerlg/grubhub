import { useMutation } from "react-query";
import api, { ApiError } from "../api";
import useAuth, { getAuthUser, UserDto } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

export default function useLogin() {
  const { setUser } = useAuth();

  return useMutation<UserDto, ApiError, LoginCommand, null>(async (command) => {
    await api.post("/auth/login", command);
    const user = await getAuthUser();
    setUser(user);
    return user;
  });
}
