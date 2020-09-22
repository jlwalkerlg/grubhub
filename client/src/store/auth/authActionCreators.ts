import {
  LOGIN,
  LoginAction,
  LOGOUT,
  LogoutAction,
  SetAuthRestaurantAction,
} from "./authReducer";
import { UserDto } from "~/api/users/UserDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";

export const createLoginAction = (user: UserDto): LoginAction => ({
  type: LOGIN,
  payload: {
    user,
  },
});

export const createLogoutAction = (): LogoutAction => ({
  type: LOGOUT,
});

export const createSetAuthRestaurantAction = (
  restaurant: RestaurantDto
): SetAuthRestaurantAction => ({
  type: "SET_AUTH_RESTAURANT",
  payload: {
    restaurant,
  },
});
