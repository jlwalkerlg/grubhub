import { useSelector, useDispatch } from "react-redux";

import cookie from "cookie";

import { State } from "~/store/store";
import authApi, {
  LoginCommand,
  UpdateUserDetailsCommand,
} from "~/api/users/userApi";
import {
  createLoginAction,
  createLogoutAction,
  createUpdateAuthRestaurantDetailsAction,
  createUpdateAuthUserDetailsAction,
} from "./authActionCreators";
import { Result } from "~/services/Result";
import { UserDto } from "~/api/users/UserDto";
import restaurantsApi, {
  UpdateRestaurantDetailsCommand,
} from "~/api/restaurants/restaurantsApi";
import { AuthState } from "./authReducer";
import userApi from "~/api/users/userApi";

export default function useAuth() {
  const dispatch = useDispatch();

  const { user, restaurant, menu } = useSelector<State, AuthState>(
    (state) => state.auth
  );

  const isLoggedIn = user !== null;

  const login = async (command: LoginCommand): Promise<Result<UserDto>> => {
    const loginResponse = await authApi.login(command);

    if (!loginResponse.isSuccess) {
      return Result.fail(loginResponse.error);
    }

    const getAuthUserResponse = await authApi.getAuthData();

    if (!getAuthUserResponse.isSuccess) {
      return Result.fail(getAuthUserResponse.error);
    }

    const user = getAuthUserResponse.data;

    dispatch(createLoginAction(user));

    document.cookie = cookie.serialize("auth_data", JSON.stringify(user), {
      expires: new Date(Date.now() + 60 * 60 * 24 * 14 * 1000),
      httpOnly: false,
      path: "/",
    });

    return Result.ok(user);
  };

  const logout = async (): Promise<Result> => {
    const response = await authApi.logout();

    if (response.isSuccess) {
      dispatch(createLogoutAction());

      document.cookie = cookie.serialize("auth_data", "", {
        expires: new Date(0),
        httpOnly: false,
        path: "/",
      });

      return Result.ok();
    }

    return Result.fail(response.error);
  };

  const updateRestaurantDetails = async (
    command: UpdateRestaurantDetailsCommand
  ): Promise<Result> => {
    const response = await restaurantsApi.updateDetails(restaurant.id, command);

    if (response.isSuccess) {
      dispatch(createUpdateAuthRestaurantDetailsAction(command));

      return Result.ok();
    }

    return Result.fail(response.error);
  };

  const updateUserDetails = async (
    command: UpdateUserDetailsCommand
  ): Promise<Result> => {
    const response = await userApi.updateDetails(command);

    if (response.isSuccess) {
      dispatch(createUpdateAuthUserDetailsAction(command));

      return Result.ok();
    }

    return Result.fail(response.error);
  };

  return {
    isLoggedIn,
    user,
    restaurant,
    menu,
    login,
    logout,
    updateRestaurantDetails,
    updateUserDetails,
  };
}
