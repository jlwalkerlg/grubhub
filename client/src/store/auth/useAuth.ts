import { useSelector, useDispatch } from "react-redux";

import cookie from "cookie";

import { State } from "~/store/store";
import { AuthState } from "./authReducer";
import authApi, { LoginRequest } from "~/api/users/userApi";
import { createLoginAction, createLogoutAction } from "./authActionCreators";
import { ApiError } from "~/lib/Error";
import { Result } from "~/lib/Result";
import { UserDto } from "~/api/users/UserDto";

export default function useAuth() {
  const dispatch = useDispatch();

  const { user, restaurant } = useSelector<State, AuthState>(
    (state) => state.auth
  );

  const isLoggedIn = user !== null;

  const login = async (
    request: LoginRequest
  ): Promise<Result<UserDto, ApiError>> => {
    const response = await authApi.login(request);

    if (response.isSuccess) {
      const data = response.data;

      dispatch(createLoginAction(data.user, data.restaurant));

      document.cookie = cookie.serialize("auth_data", JSON.stringify(data), {
        expires: new Date(Date.now() + 60 * 60 * 24 * 14 * 1000),
        httpOnly: false,
        path: "/",
      });

      return Result.ok<UserDto, ApiError>(data.user);
    }

    return Result.fail(new ApiError(response));
  };

  const logout = async (): Promise<Result<null, ApiError>> => {
    const response = await authApi.logout();

    if (response.isSuccess) {
      dispatch(createLogoutAction());

      document.cookie = cookie.serialize("auth_data", "", {
        expires: new Date(0),
        httpOnly: false,
        path: "/",
      });

      return Result.ok<null, ApiError>(null);
    }

    return Result.fail(new ApiError(response));
  };

  return { isLoggedIn, user, restaurant, login, logout };
}
