import { useMutation } from "react-query";
import api, { ApiError } from "../apii";
import useAuth, { UserDto } from "./useAuth";

export interface UpdateUserDetailsCommand {
  firstName: string;
  lastName: string;
  email: string;
}

async function updateUserDetails(command: UpdateUserDetailsCommand) {
  await api.put("/auth/user", command);
}

export default function useUpdateUserDetails() {
  const { user, setUser } = useAuth();

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

        setUser(newUser);
      },
    }
  );
}
