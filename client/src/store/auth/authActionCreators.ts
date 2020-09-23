import {
  LOGIN,
  LoginAction,
  LOGOUT,
  LogoutAction,
  SetAuthRestaurantAction,
  UpdateRestaurantDetailsAction,
} from "./authReducer";
import { UserDto } from "~/api/users/UserDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UpdateRestaurantDetailsCommand } from "~/api/restaurants/restaurantsApi";

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

export const createUpdateAuthRestaurantDetailsAction = (
  command: UpdateRestaurantDetailsCommand
): UpdateRestaurantDetailsAction => ({
  type: "UPDATE_RESTAURANT_DETAILS",
  payload: {
    command,
  },
});
