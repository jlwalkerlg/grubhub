import { GetServerSideProps } from "next";

import { Login } from "~/views/Login/Login";
import { withGuestOnly } from "~/utils/withGuestOnly";
import { redirectIfAuthenticated } from "~/utils/redirectIfAuthenticated";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  redirectIfAuthenticated(ctx);

  return {
    props: {},
  };
};

export default withGuestOnly(Login);
