import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import useAuth, { getAuthUserQueryKey } from "./useAuth";
import { UserDto } from "./UserDto";

export interface UpdateUserDetailsCommand {
  name: string;
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
          name: command.name,
          email: command.email,
        };

        queryCache.setQueryData(getAuthUserQueryKey(), newUser);
      },
    }
  );
}
