import { GetServerSideProps } from "next";

import { Home } from "~/views/Home/Home";
import { initializeStore } from "~/store/store";
import { dispatchUserFromRequest } from "~/services/auth/authHelpers";

export const getServerSideProps: GetServerSideProps = async (ctx) => {
  const store = initializeStore();

  dispatchUserFromRequest(ctx, store);

  return {
    props: {
      initialReduxState: store.getState(),
    },
  };
};

export default Home;
