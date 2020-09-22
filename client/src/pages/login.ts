import { GetServerSideProps } from "next";

import cookie from "cookie";

import { Login } from "~/views/Login/Login";
import { UserDto } from "~/api/users/UserDto";
import { withGuestOnly } from "~/utils/withGuestOnly";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const cookies = cookie.parse(ctx.req.headers.cookie || "");
  const user = JSON.parse(cookies["auth_data"] || null) as UserDto;

  if (user !== null) {
    ctx.res
      .writeHead(307, {
        Location: "/dashboard",
      })
      .end();
  }

  return {
    props: {},
  };
};

export default withGuestOnly(Login);
