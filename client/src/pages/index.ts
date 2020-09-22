import { GetServerSideProps } from "next";

import cookie from "cookie";

import { Home } from "~/views/Home/Home";
import { UserDto } from "~/api/users/UserDto";
import { initializeStore } from "~/store/store";
import { createLoginAction } from "~/store/auth/authActionCreators";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const store = initializeStore();

  const cookies = cookie.parse(ctx.req.headers.cookie || "");
  const user = JSON.parse(cookies["auth_data"] || null) as UserDto;

  if (user !== null) {
    store.dispatch(createLoginAction(user));
  }

  return {
    props: {
      initialReduxState: store.getState(),
    },
  };
};

export default Home;
