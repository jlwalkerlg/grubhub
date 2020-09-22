import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UserDto } from "~/api/users/UserDto";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";
export const SET_AUTH_RESTAURANT = "SET_AUTH_RESTAURANT";

export interface LoginAction {
  type: typeof LOGIN;
  payload: {
    user: UserDto;
  };
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

export interface SetAuthRestaurantAction {
  type: typeof SET_AUTH_RESTAURANT;
  payload: {
    restaurant: RestaurantDto;
  };
}

type AuthAction = LoginAction | LogoutAction | SetAuthRestaurantAction;

export interface AuthState {
  user: UserDto;
  restaurant: RestaurantDto;
}

const initialState = {
  user: null,
  restaurant: null,
};

export default function (
  state: AuthState = { ...initialState },
  action: AuthAction
): AuthState {
  if (action.type === LOGIN) {
    return {
      ...state,
      user: action.payload.user,
    };
  }

  if (action.type === LOGOUT) {
    return {
      ...state,
      user: null,
      restaurant: null,
    };
  }

  if (action.type === SET_AUTH_RESTAURANT) {
    return {
      ...state,
      restaurant: action.payload.restaurant,
    };
  }

  return state;
}
