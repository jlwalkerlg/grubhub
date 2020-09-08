import { User } from "./User";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";

export interface LoginAction {
  type: typeof LOGIN;
  payload: {
    user: User;
  };
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

type AuthAction = LoginAction | LogoutAction;

export interface AuthState {
  isLoggedIn: boolean;
  user: User;
}

const initialState = {
  isLoggedIn: false,
  user: null,
};

export default function (
  state: AuthState = initialState,
  action: AuthAction
): AuthState {
  if (action.type === LOGIN) {
    return {
      isLoggedIn: true,
      user: action.payload.user,
    };
  }

  if (action.type === LOGOUT) {
    return {
      isLoggedIn: false,
      user: null,
    };
  }

  return state;
}
