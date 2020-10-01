import { GetServerSideProps } from "next";

import { RegisterRestaurant } from "~/views/RegisterRestaurant/RegisterRestaurant";
import { withGuestOnly } from "~/services/auth/withGuestOnly";
import { redirectIfAuthenticated } from "~/services/auth/authHelpers";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  redirectIfAuthenticated(ctx);

  return {
    props: {},
  };
};

export default withGuestOnly(RegisterRestaurant);
