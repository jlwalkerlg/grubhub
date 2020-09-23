import { GetServerSideProps } from "next";

import restaurantsApi from "~/api/restaurants/restaurantsApi";
import { initializeStore } from "~/store/store";

import RestaurantDetails from "~/views/Dashboard/RestaurantDetails";
import { withAuth } from "~/utils/withAuth";
import { createSetAuthRestaurantAction } from "~/store/auth/authActionCreators";
import { dispatchUserFromRequest } from "~/utils/dispatchUserFromRequest";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const store = initializeStore();

  const user = dispatchUserFromRequest(ctx, store, { required: true });

  if (user.role !== "RestaurantManager") {
    ctx.res.writeHead(403).end();
  }

  const response = await restaurantsApi.getByManagerId(user.id);
  const restaurant = response.data;
  store.dispatch(createSetAuthRestaurantAction(restaurant));

  return {
    props: {
      initialReduxState: store.getState(),
    },
  };
};

export default withAuth(RestaurantDetails);
