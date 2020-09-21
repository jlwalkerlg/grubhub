import { NextPageContext } from "next";

import cookie from "cookie";

import { State, initializeStore } from "~/store/store";
import { UserDto, UserRole } from "~/api/users/UserDto";

function redirect(context: NextPageContext, location: string) {
  context.res
    .writeHead(307, {
      Location: location,
    })
    .end();
}

export class GetInitialPropsBuilder {
  private requiresAuth = false;
  public requireAuth() {
    this.requiresAuth = true;
    return this;
  }

  private requiredRole: UserRole = null;
  public requireRole(role: UserRole) {
    this.requireAuth();
    this.requiredRole = role;
    return this;
  }

  private isGuestOnly = false;
  public guestOnly() {
    this.isGuestOnly = true;
    return this;
  }

  private mutator: (state: State) => Promise<void> = null;
  public useState(mutator: (state: State) => Promise<void>) {
    this.mutator = mutator;
    return this;
  }

  public build() {
    return async (context: NextPageContext) => {
      const store = initializeStore();
      const state = store.getState();

      // server only
      if (context.req !== undefined) {
        const cookies = cookie.parse(context.req.headers.cookie || "");
        const user = JSON.parse(cookies["auth_data"] || null) as UserDto;

        if (this.requiresAuth && user === null) {
          redirect(context, "/login");
        }

        if (this.isGuestOnly && user !== null) {
          redirect(context, "/");
        }

        if (
          user !== null &&
          this.requiredRole !== null &&
          user.role !== this.requiredRole
        ) {
          return {
            error: 403,
            initialReduxState: null,
          };
        }

        if (user !== null) {
          state.auth.user = user;
        }
      }

      if (this.mutator !== null) {
        await this.mutator(state);
      }

      return {
        initialReduxState: state,
      };
    };
  }
}
