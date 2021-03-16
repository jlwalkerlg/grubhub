import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUserQueryKey } from "./useAuth";

interface UpdateAccountDetailsCommand {
  firstName: string;
  lastName: string;
  mobileNumber: string;
}

export default function useUpdateAccountDetails() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateAccountDetailsCommand, null>(
    async (command) => {
      await Api.put("/account/details", command);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getAuthUserQueryKey());
      },
    }
  );
}
