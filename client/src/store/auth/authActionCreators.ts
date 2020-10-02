import {
  LOGIN,
  LoginAction,
  LOGOUT,
  LogoutAction,
  SetAuthRestaurantMenuAction,
  SetAuthRestaurantAction,
  UpdateRestaurantDetailsAction,
  UpdateUserDetailsAction,
  AddMenuItemAction,
} from "./authReducer";
import { UserDto } from "~/api/users/UserDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UpdateRestaurantDetailsCommand } from "~/api/restaurants/restaurantsApi";
import { UpdateUserDetailsCommand } from "~/api/users/userApi";
import { MenuDto, MenuItemDto } from "~/api/restaurants/MenuDto";

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

export const createSetAuthRestaurantMenuAction = (
  menu: MenuDto
): SetAuthRestaurantMenuAction => ({
  type: "SET_AUTH_RESTAURANT_MENU",
  payload: {
    menu,
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

export const createUpdateAuthUserDetailsAction = (
  command: UpdateUserDetailsCommand
): UpdateUserDetailsAction => ({
  type: "UPDATE_USER_DETAILS",
  payload: {
    command,
  },
});

export const createAddMenuItemAction = (
  item: MenuItemDto
): AddMenuItemAction => ({
  type: "ADD_MENU_ITEM",
  payload: {
    item,
  },
});
