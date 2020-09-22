import { GetServerSidePropsContext } from "next";
import { Store } from "redux";
import { createLoginAction } from "~/store/auth/authActionCreators";
import { State } from "~/store/store";
import { getUserFromRequest } from "./getUserFromRequest";
import { redirect } from "./redirect";

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
