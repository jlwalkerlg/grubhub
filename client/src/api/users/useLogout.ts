import cookie from "cookie";
import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getAuthUserQueryKey } from "./useAuth";

async function logout() {
  await Api.post("/auth/logout");
}

export default function useLogout() {
  const queryCache = useQueryCache();

  return useMutation<void, ApiError, null, null>(
    async () => {
      await logout();

      localStorage.removeItem("isLoggedIn");
    },
    {
      onSuccess: () => {
        queryCache.setQueryData(getAuthUserQueryKey(), null);

        document.cookie = cookie.serialize("auth_data", "", {
          expires: new Date(0),
          httpOnly: false,
          path: "/",
        });
      },
    }
  );
}
