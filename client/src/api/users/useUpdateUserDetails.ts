import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import useAuth, { getAuthUserQueryKey } from "./useAuth";
import { UserDto } from "./UserDto";

export interface UpdateUserDetailsCommand {
  name: string;
  email: string;
}

async function updateUserDetails(command: UpdateUserDetailsCommand) {
  const response = await Api.put("/auth/user", command);

  if (!response.isSuccess) {
    throw response.error;
  }
}

export default function useUpdateUserDetails() {
  const queryCache = useQueryCache();

  const { user } = useAuth();

  return useMutation<void, Error, UpdateUserDetailsCommand, null>(
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
