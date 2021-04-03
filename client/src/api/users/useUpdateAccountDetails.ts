import { useMutation, useQueryClient } from "react-query";
import api, { ApiError } from "../apii";
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
      await api.put("/account/details", command);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getAuthUserQueryKey());
      },
    }
  );
}
