import cookie from "cookie";
import { useMutation, useQueryCache } from "react-query";
import { Error } from "~/services/Error";
import Api from "../Api";
import { getAuthUser, getAuthUserQueryKey } from "./useAuth";

export interface LoginCommand {
  email: string;
  password: string;
}

async function login(command: LoginCommand) {
  const loginResponse = await Api.post("/auth/login", command);

  if (!loginResponse.isSuccess) {
    throw loginResponse.error;
  }
}

export default function useLogin() {
  const queryCache = useQueryCache();

  return useMutation<void, Error, LoginCommand, null>(async (command) => {
    await login(command);
    const user = await getAuthUser();

    document.cookie = cookie.serialize("auth_data", JSON.stringify(user), {
      expires: new Date(Date.now() + 60 * 60 * 24 * 14 * 1000),
      httpOnly: false,
      path: "/",
    });

    queryCache.setQueryData(getAuthUserQueryKey(), user);
  });
}
