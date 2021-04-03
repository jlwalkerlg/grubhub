import { useMutation } from "react-query";
import api, { ApiError } from "../api";

interface ChangePasswordCommand {
  currentPassword: string;
  newPassword: string;
}

export default function useChangePassword() {
  return useMutation<void, ApiError, ChangePasswordCommand, null>(
    async (command) => {
      await api.put("/account/password", command);
    }
  );
}
