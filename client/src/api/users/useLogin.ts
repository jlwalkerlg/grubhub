import cookie from "cookie";
import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../Api";
import { getAuthUser, getAuthUserQueryKey } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

async function login(command: LoginCommand) {
  await Api.post("/auth/login", command);
}

export default function useLogin() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, LoginCommand, null>(login, {
    onSuccess: async () => {
      const user = await getAuthUser();

      document.cookie = cookie.serialize("auth_data", JSON.stringify(user), {
        expires: new Date(Date.now() + 60 * 60 * 24 * 14 * 1000),
        httpOnly: false,
        path: "/",
      });

      cache.setQueryData(getAuthUserQueryKey(), user);
    },
  });
}
