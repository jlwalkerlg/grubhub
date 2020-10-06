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
  AddMenuCategoryAction,
  UpdateMenuItemAction,
} from "./authReducer";
import { UserDto } from "~/api/users/UserDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UpdateRestaurantDetailsRequest } from "~/api/restaurants/restaurantsApi";
import { UpdateUserDetailsCommand } from "~/api/users/userApi";
import {
  MenuCategoryDto,
  MenuDto,
  MenuItemDto,
} from "~/api/restaurants/MenuDto";

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
  request: UpdateRestaurantDetailsRequest
): UpdateRestaurantDetailsAction => ({
  type: "UPDATE_RESTAURANT_DETAILS",
  payload: {
    request: request,
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

export const createAddMenuCategoryAction = (
  category: MenuCategoryDto
): AddMenuCategoryAction => ({
  type: "ADD_MENU_CATEGORY",
  payload: {
    category,
  },
});

export const createAddMenuItemAction = (
  category: string,
  item: MenuItemDto
): AddMenuItemAction => ({
  type: "ADD_MENU_ITEM",
  payload: {
    category,
    item,
  },
});

export const createUpdateMenuItemAction = (
  categoryName: string,
  itemName: string,
  item: MenuItemDto
): UpdateMenuItemAction => ({
  type: "UPDATE_MENU_ITEM",
  payload: {
    categoryName,
    itemName,
    item,
  },
});
