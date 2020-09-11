import { LOGIN, LoginAction, LOGOUT, LogoutAction } from "./authReducer";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UserDto } from "~/api/users/UserDto";

export const createLoginAction = (
  user: UserDto,
  restaurant: RestaurantDto
): LoginAction => ({
  type: LOGIN,
  payload: {
    user,
    restaurant,
  },
});

export const createLogoutAction = (): LogoutAction => ({
  type: LOGOUT,
});
