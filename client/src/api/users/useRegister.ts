import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUser, getAuthUserQueryKey } from "./useAuth";

export interface RegisterCommand {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export default function useRegister() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, RegisterCommand, null>(
    async (command) => {
      await Api.post("/register", command);

      localStorage.setItem("isLoggedIn", "true");
    },
    {
      onSuccess: async () => {
        const user = await getAuthUser();

        cache.setQueryData(getAuthUserQueryKey(), user);
      },
      onError: () => {
        localStorage.removeItem("isLoggedIn");
      },
    }
  );
}
