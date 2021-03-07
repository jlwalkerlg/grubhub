import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getAuthUserQueryKey } from "./useAuth";

interface UpdateAccountDetailsCommand {
  name: string;
  mobileNumber: string;
}

export default function useUpdateAccountDetails() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, UpdateAccountDetailsCommand, null>(
    async (command) => {
      await Api.put("/account/details", command);
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getAuthUserQueryKey());
      },
    }
  );
}
