import cookie from "cookie";
import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getAuthUserQueryKey } from "./useAuth";

async function logout() {
  const response = await Api.post("/auth/logout");

  if (!response.isSuccess) {
    throw response.error;
  }

  document.cookie = cookie.serialize("auth_data", "", {
    expires: new Date(0),
    httpOnly: false,
    path: "/",
  });
}

export default function useLogout() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, null, null>(async () => {
    await logout();

    queryCache.setQueryData(getAuthUserQueryKey(), null);
  });
}
