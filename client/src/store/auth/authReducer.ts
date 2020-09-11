import { User } from "./User";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";

export interface LoginAction {
  type: typeof LOGIN;
  payload: {
    user: User;
    restaurant: RestaurantDto;
  };
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

type AuthAction = LoginAction | LogoutAction;

export interface AuthState {
  user: User;
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
      restaurant: action.payload.restaurant,
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
