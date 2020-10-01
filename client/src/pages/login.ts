import { GetServerSideProps } from "next";

import { Login } from "~/views/Login/Login";
import { withGuestOnly } from "~/services/auth/withGuestOnly";
import { redirectIfAuthenticated } from "~/services/auth/redirectIfAuthenticated";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  redirectIfAuthenticated(ctx);

  return {
    props: {},
  };
};

export default withGuestOnly(Login);
