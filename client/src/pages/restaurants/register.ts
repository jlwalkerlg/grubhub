import { GetServerSideProps } from "next";

import { RegisterRestaurant } from "~/views/RegisterRestaurant/RegisterRestaurant";
import { withGuestOnly } from "~/utils/withGuestOnly";
import { redirectIfAuthenticated } from "~/utils/redirectIfAuthenticated";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  redirectIfAuthenticated(ctx);

  return {
    props: {},
  };
};

export default withGuestOnly(RegisterRestaurant);
