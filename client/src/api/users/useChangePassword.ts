import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

interface ChangePasswordCommand {
  password: string;
}

export default function useChangePassword() {
  return useMutation<void, ApiError, ChangePasswordCommand, null>(
    async (command) => {
      await Api.put("/account/password", command);
    }
  );
}
