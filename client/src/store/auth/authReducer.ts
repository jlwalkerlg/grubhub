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
  user: User;
}

const initialState = {
  user: null,
};

export default function (
  state: AuthState = initialState,
  action: AuthAction
): AuthState {
  if (action.type === LOGIN) {
    return {
      user: action.payload.user,
    };
  }

  if (action.type === LOGOUT) {
    return {
      user: null,
    };
  }

  return state;
}
