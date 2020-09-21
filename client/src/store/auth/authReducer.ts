import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UserDto } from "~/api/users/UserDto";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";

export interface LoginAction {
  type: typeof LOGIN;
  payload: {
    user: UserDto;
  };
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

type AuthAction = LoginAction | LogoutAction;

export interface AuthState {
  user: UserDto;
  restaurant: RestaurantDto;
}

const initialState = {
  user: null,
  restaurant: null,
};

export default function (
  state: AuthState = initialState,
  action: AuthAction
): AuthState {
  if (action.type === LOGIN) {
    return {
      ...state,
      user: action.payload.user,
      restaurant: null,
    };
  }

  if (action.type === LOGOUT) {
    return {
      ...state,
      user: null,
      restaurant: null,
    };
  }

  return state;
}
