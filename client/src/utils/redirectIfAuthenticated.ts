import { GetServerSidePropsContext } from "next";
import { getUserFromRequest } from "./getUserFromRequest";
import { redirect } from "./redirect";

export const redirectIfAuthenticated = (ctx: GetServerSidePropsContext) => {
  const user = getUserFromRequest(ctx.req);

  if (user !== null) {
    redirect(ctx.res, "/dashboard");
  }
};
