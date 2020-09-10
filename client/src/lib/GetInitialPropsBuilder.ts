import { NextPageContext } from "next";

import cookie from "cookie";

import { State, initializeStore } from "~/store/store";
import { UserRole, User } from "~/store/auth/User";
import { AuthData } from "~/api/AuthApi";

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

      const cookies = cookie.parse(context.req.headers.cookie || "");
      const data = JSON.parse(cookies["auth_data"] || null) as AuthData;

      if (this.requiresAuth && data === null) {
        redirect(context, "/login");
      }

      if (this.isGuestOnly && data !== null) {
        redirect(context, "/");
      }

      if (data !== null) {
        const store = initializeStore();
        state = store.getState();

        const user: User = {
          id: data.user.id,
          name: data.user.name,
          email: data.user.email,
          role: UserRole[data.user.role],
        };

        state.auth.user = user;

        if (user.role === UserRole.RestaurantManager) {
          const restaurant = data.restaurant;

          state.auth.restaurant = restaurant;
        }
      }

      return {
        initialReduxState: state,
      };
    };
  }
}
