import { useSelector, useDispatch } from "react-redux";

import { State } from "~/store/store";
import { AuthState } from "./authReducer";
import authApi, { LoginRequest } from "~/api/authApi";
import { User } from "./User";
import { createLoginAction } from "./authActionCreators";
import { ApiError } from "~/lib/Error";
import { Result } from "~/lib/Result";

export default function useAuth() {
  const dispatch = useDispatch();
  const { user } = useSelector<State, AuthState>((state) => state.auth);

  const isLoggedIn = user !== null;

  const login = async (
    request: LoginRequest
  ): Promise<Result<User, ApiError>> => {
    const response = await authApi.login(request);

    if (response.isSuccess) {
      const userDto = response.data.data;

      const user = new User(
        userDto.id,
        userDto.name,
        userDto.email,
        userDto.role
      );

      dispatch(createLoginAction(user));

      return Result.ok<User, ApiError>(user);
    }

    return Result.fail(new ApiError(response));
  };

  return { isLoggedIn, user, login };
}
