import { useMutation } from "react-query";
import api, { ApiError } from "../Api";
import useAuth from "./useAuth";

export default function useLogout() {
  const { setUser } = useAuth();

  return useMutation<void, ApiError, null, null>(async () => {
    await api.post("/auth/logout");
    setUser(null);
  });
}
