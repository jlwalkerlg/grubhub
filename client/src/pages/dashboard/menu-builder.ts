import { GetServerSideProps } from "next";

import restaurantsApi from "~/api/restaurants/restaurantsApi";
import { initializeStore } from "~/store/store";

import MenuBuilder from "~/views/Dashboard/MenuBuilder";
import { withAuth } from "~/utils/withAuth";
import {
  createSetAuthRestaurantMenuAction,
  createSetAuthRestaurantAction,
} from "~/store/auth/authActionCreators";
import { dispatchUserFromRequest } from "~/utils/dispatchUserFromRequest";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const store = initializeStore();

  const user = dispatchUserFromRequest(ctx, store, { required: true });

  if (user.role !== "RestaurantManager") {
    ctx.res.writeHead(403).end();
  }

  const [restaurantResponse, menuResponse] = await Promise.all([
    await restaurantsApi.getById(user.restaurantId),
    await restaurantsApi.getMenuByRestaurantId(user.restaurantId),
  ]);

  const restaurant = restaurantResponse.data;
  store.dispatch(createSetAuthRestaurantAction(restaurant));

  const menu = menuResponse.data;
  store.dispatch(createSetAuthRestaurantMenuAction(menu));

  return {
    props: {
      initialReduxState: store.getState(),
    },
  };
};

export default withAuth(MenuBuilder);
