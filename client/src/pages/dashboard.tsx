import { GetServerSideProps } from "next";

import cookie from "cookie";

import restaurantsApi from "~/api/restaurants/restaurantsApi";
import { UserDto } from "~/api/users/UserDto";
import { initializeStore } from "~/store/store";

import { Dashboard } from "~/views/Dashboard/Dashboard";
import { withAuth } from "~/utils/withAuth";
import {
  createLoginAction,
  createSetAuthRestaurantAction,
} from "~/store/auth/authActionCreators";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const store = initializeStore();

  const cookies = cookie.parse(ctx.req.headers.cookie || "");
  const user = JSON.parse(cookies["auth_data"] || null) as UserDto;

  if (user === null) {
    ctx.res.writeHead(401).end();
  }

  store.dispatch(createLoginAction(user));

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

export default withAuth(Dashboard);
