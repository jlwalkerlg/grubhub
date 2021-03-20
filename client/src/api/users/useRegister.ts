import { useMutation } from "react-query";
import Api, { ApiError } from "../api";

export interface RegisterCommand {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export default function useRegister() {
  return useMutation<void, ApiError, RegisterCommand, null>(async (command) => {
    await Api.post("/register", command);
  });
}
