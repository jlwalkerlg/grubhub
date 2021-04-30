import { useMutation } from "react-query";
import api, { ApiError } from "../api";

export interface RegisterCommand {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

interface RegisterResponse {
  xsrfToken: string;
}

export default function useRegister() {
  return useMutation<void, ApiError, RegisterCommand, null>(async (command) => {
    const {
      data: { xsrfToken },
    } = await api.post<RegisterResponse>("/register", command);

    localStorage.setItem("XSRF-TOKEN", xsrfToken);
  });
}
