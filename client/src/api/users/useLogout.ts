import { useMutation } from "react-query";
import api, { ApiError } from "../api";

export default function useLogout() {
  return useMutation<void, ApiError, null, null>(async () => {
    await api.post("/auth/logout");
    localStorage.removeItem("XSRF-TOKEN");
  });
}
