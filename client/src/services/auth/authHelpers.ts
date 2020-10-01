import { GetServerSidePropsContext } from "next";
import { Store } from "redux";
import cookie from "cookie";
import { createLoginAction } from "~/store/auth/authActionCreators";
import { State } from "~/store/store";
import { IncomingMessage } from "http";
import { UserDto } from "~/api/users/UserDto";
import { redirect } from "../helpers";

export const dispatchUserFromRequest = (
  ctx: GetServerSidePropsContext,
  store: Store<State>,
  options: { required: boolean } = { required: false }
) => {
  const user = getUserFromRequest(ctx.req);

  if (user === null && options.required) {
    redirect(ctx.res, "/login");
  }

  if (user !== null) {
    store.dispatch(createLoginAction(user));
  }

  return user;
};

export const getUserFromRequest = (req: IncomingMessage) => {
  const cookies = cookie.parse(req.headers.cookie || "");
  return JSON.parse(cookies["auth_data"] || null) as UserDto;
};

export const redirectIfAuthenticated = (ctx: GetServerSidePropsContext) => {
  const user = getUserFromRequest(ctx.req);

  if (user !== null) {
    redirect(ctx.res, "/dashboard");
  }
};
