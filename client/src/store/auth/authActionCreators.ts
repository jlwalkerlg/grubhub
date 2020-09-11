import { User } from "./User";
import { LOGIN, LoginAction, LOGOUT, LogoutAction } from "./authReducer";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";

export const createLoginAction = (
  user: User,
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
