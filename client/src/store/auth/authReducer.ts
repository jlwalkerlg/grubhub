import { MenuDto } from "~/api/restaurants/MenuDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UpdateRestaurantDetailsCommand } from "~/api/restaurants/restaurantsApi";
import { UpdateUserDetailsCommand } from "~/api/users/userApi";
import { UserDto } from "~/api/users/UserDto";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";
export const SET_AUTH_RESTAURANT = "SET_AUTH_RESTAURANT";
export const SET_AUTH_RESTAURANT_MENU = "SET_AUTH_RESTAURANT_MENU";
export const UPDATE_RESTAURANT_DETAILS = "UPDATE_RESTAURANT_DETAILS";
export const UPDATE_USER_DETAILS = "UPDATE_USER_DETAILS";

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

export interface SetAuthRestaurantMenuAction {
  type: typeof SET_AUTH_RESTAURANT_MENU;
  payload: {
    menu: MenuDto;
  };
}

export interface UpdateRestaurantDetailsAction {
  type: typeof UPDATE_RESTAURANT_DETAILS;
  payload: {
    command: UpdateRestaurantDetailsCommand;
  };
}

export interface UpdateUserDetailsAction {
  type: typeof UPDATE_USER_DETAILS;
  payload: {
    command: UpdateUserDetailsCommand;
  };
}

type AuthAction =
  | LoginAction
  | LogoutAction
  | SetAuthRestaurantAction
  | SetAuthRestaurantMenuAction
  | UpdateRestaurantDetailsAction
  | UpdateUserDetailsAction;

export interface AuthState {
  user: UserDto;
  restaurant: RestaurantDto;
  menu: MenuDto;
}

const initialState = {
  user: null,
  restaurant: null,
  menu: null,
};

export default function authReducer(
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
      menu: null,
    };
  }

  if (action.type === SET_AUTH_RESTAURANT) {
    return {
      ...state,
      restaurant: action.payload.restaurant,
    };
  }

  if (action.type === SET_AUTH_RESTAURANT_MENU) {
    return {
      ...state,
      menu: action.payload.menu,
    };
  }

  if (action.type === UPDATE_RESTAURANT_DETAILS) {
    return {
      ...state,
      restaurant: {
        ...state.restaurant,
        ...action.payload.command,
      },
    };
  }

  if (action.type === UPDATE_USER_DETAILS) {
    return {
      ...state,
      user: {
        ...state.user,
        ...action.payload.command,
      },
    };
  }

  return state;
}
