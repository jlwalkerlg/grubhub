import { useSelector, useDispatch } from "react-redux";

import { State } from "~/store/store";
import { AuthState } from "./authReducer";
import authApi, { LoginRequest } from "~/api/AuthApi";
import { User, UserRole } from "./User";
import { createLoginAction, createLogoutAction } from "./authActionCreators";
import { ApiError } from "~/lib/Error";
import { Result } from "~/lib/Result";

export default function useAuth() {
  const dispatch = useDispatch();

  const { user, restaurant } = useSelector<State, AuthState>(
    (state) => state.auth
  );

  const isLoggedIn = user !== null;

  const login = async (
    request: LoginRequest
  ): Promise<Result<User, ApiError>> => {
    const response = await authApi.login(request);

    if (response.isSuccess) {
      const data = response.data.data;

      const user = new User(
        data.user.id,
        data.user.name,
        data.user.email,
        UserRole[data.user.role]
      );

      dispatch(createLoginAction(user, data.restaurant));

      return Result.ok<User, ApiError>(user);
    }

    return Result.fail(new ApiError(response));
  };

  const logout = async (): Promise<Result<null, ApiError>> => {
    const response = await authApi.logout();

    if (response.isSuccess) {
      dispatch(createLogoutAction());

      return Result.ok<null, ApiError>(null);
    }

    return Result.fail(new ApiError(response));
  };

  return { isLoggedIn, user, restaurant, login, logout };
}
