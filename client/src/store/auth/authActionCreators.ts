import { User } from "~/models/User";
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
