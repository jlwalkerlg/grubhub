import { LOGIN, LoginAction, LOGOUT, LogoutAction } from "./authReducer";
import { UserDto } from "~/api/users/UserDto";

export const createLoginAction = (user: UserDto): LoginAction => ({
  type: LOGIN,
  payload: {
    user,
  },
});

export const createLogoutAction = (): LogoutAction => ({
  type: LOGOUT,
});
