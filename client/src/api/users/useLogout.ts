import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUserQueryKey } from "./useAuth";

async function logout() {
  await Api.post("/auth/logout");
}

export default function useLogout() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, null, null>(
    async () => {
      await logout();

      localStorage.removeItem("isLoggedIn");
    },
    {
      onSuccess: () => {
        queryClient.setQueryData(getAuthUserQueryKey(), null);
      },
    }
  );
}
