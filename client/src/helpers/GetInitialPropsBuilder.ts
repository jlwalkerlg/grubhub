import { NextPageContext } from "next";
import { State, initializeStore } from "~/store/store";
import { UserRole, User } from "~/store/auth/User";
import { getUserFromContext, getAuthToken } from "./auth";
import restaurantsApi from "~/api/restaurantsApi";
import Api from "~/api/Api";

function redirect(context: NextPageContext, location: string) {
  context.res.writeHead(307, {
    Location: location,
  });

  context.res.end();
}

export class GetInitialPropsBuilder {
  private requiresAuth = false;
  public requireAuth() {
    this.requiresAuth = true;
    return this;
  }

  private isGuestOnly = false;
  public guestOnly() {
    this.isGuestOnly = true;
    return this;
  }

  public build() {
    return async (context: NextPageContext) => {
      // don't run on client
      if (context.req === undefined) {
        return {
          initialReduxState: null,
        };
      }

      let state: State = null;

      const userDto = await getUserFromContext(context);

      if (this.requiresAuth && userDto === null) {
        redirect(context, "/login");
      }

      if (this.isGuestOnly && userDto !== null) {
        redirect(context, "/");
      }

      if (userDto !== null) {
        const store = initializeStore();
        state = store.getState();

        const user: User = {
          id: userDto.id,
          name: userDto.name,
          email: userDto.email,
          role: UserRole[userDto.role],
        };

        state.auth.user = user;

        Api.addAuthToken(getAuthToken(context));

        if (user.role === UserRole.RestaurantManager) {
          const restaurantResult = await restaurantsApi.getAuthUserRestaurantDetails();
          const restaurant = restaurantResult.data.data;

          state.auth.restaurant = restaurant;
        }
      }

      return {
        initialReduxState: state,
      };
    };
  }
}
