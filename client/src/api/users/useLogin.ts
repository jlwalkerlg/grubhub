import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUser, getAuthUserQueryKey } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

async function login(command: LoginCommand) {
  await Api.post("/auth/login", command);
}

export default function useLogin() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, LoginCommand, null>(
    async (command) => {
      await login(command);

      localStorage.setItem("isLoggedIn", "true");
    },
    {
      onSuccess: async () => {
        const user = await getAuthUser();

        queryClient.setQueryData(getAuthUserQueryKey(), user);
      },
      onError: () => {
        localStorage.removeItem("isLoggedIn");
      },
    }
  );
}
