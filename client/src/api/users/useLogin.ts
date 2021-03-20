import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUser, getAuthUserQueryKey } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

export default function useLogin() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, LoginCommand, null>(
    async (command) => {
      await Api.post("/auth/login", command);
    },
    {
      onSuccess: async () => {
        const user = await getAuthUser();
        queryClient.setQueryData(getAuthUserQueryKey(), user);

        localStorage.setItem("isLoggedIn", "true");
      },
      onError: () => {
        localStorage.removeItem("isLoggedIn");
      },
    }
  );
}
