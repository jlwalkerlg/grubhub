import { User } from "./User";
import { LOGIN, LoginAction, LOGOUT, LogoutAction } from "./authReducer";

export const createLoginAction = (user: User): LoginAction => ({
  type: LOGIN,
  payload: {
    user,
  },
});

export const createLogoutAction = (): LogoutAction => ({
  type: LOGOUT,
});
