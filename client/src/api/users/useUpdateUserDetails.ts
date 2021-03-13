import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import useAuth, { getAuthUserQueryKey, UserDto } from "./useAuth";

export interface UpdateUserDetailsCommand {
  firstName: string;
  lastName: string;
  email: string;
}

async function updateUserDetails(command: UpdateUserDetailsCommand) {
  await Api.put("/auth/user", command);
}

export default function useUpdateUserDetails() {
  const queryCache = useQueryCache();

  const { user } = useAuth();

  return useMutation<void, ApiError, UpdateUserDetailsCommand, null>(
    updateUserDetails,
    {
      onSuccess: (_, command) => {
        const newUser: UserDto = {
          ...user,
          firstName: command.firstName,
          lastName: command.lastName,
          email: command.email,
        };

        queryCache.setQueryData(getAuthUserQueryKey(), newUser);
      },
    }
  );
}
