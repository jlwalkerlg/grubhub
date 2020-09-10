import { NextPageContext } from "next";
import { State, initializeStore } from "~/store/store";
import { UserRole } from "~/store/auth/User";
import { getUserFromContext } from "./auth";

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

      const user = await getUserFromContext(context);

      if (this.requiresAuth && user === null) {
        redirect(context, "/login");
      }

      if (this.isGuestOnly && user !== null) {
        redirect(context, "/");
      }

      if (user !== null) {
        const store = initializeStore();
        state = store.getState();
        state.auth.user = {
          id: user.id,
          name: user.name,
          email: user.email,
          role: UserRole[user.role],
        };
      }

      return {
        initialReduxState: state,
      };
    };
  }
}
