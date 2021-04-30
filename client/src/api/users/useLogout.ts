import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../api";
import useAuth from "./useAuth";

export default function useLogout() {
  const queryClient = useQueryClient();
  const { removeUser } = useAuth();

  return useMutation<void, ApiError, void, null>(
    async () => {
      await api.post("/auth/logout");
    },
    {
      onSuccess: () => {
        removeUser();
        queryClient.clear();
      },
    }
  );
}
