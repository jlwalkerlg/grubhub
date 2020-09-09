import { AppProps } from "next/app";
import { AppContextType } from "next/dist/next-server/lib/utils";

import { Provider } from "react-redux";
import { useStore } from "../store/store";

import { initializeStore, State } from "~/store/store";
import { UserRole } from "~/store/auth/User";

import "~/styles/index.css";
import { getUserFromContext } from "~/helpers/auth";

export default function App({ Component, pageProps }: AppProps) {
  const store = useStore(pageProps.initialReduxState);

  return (
    <Provider store={store}>
      <Component {...pageProps} />
    </Provider>
  );
}

App.getInitialProps = async (context: AppContextType) => {
  // don't run on client
  if (context.ctx.req === undefined) {
    return {
      pageProps: {
        initialReduxState: null,
      },
    };
  }

  const user = await getUserFromContext(context.ctx);

  let state: State = null;
  if (user !== null) {
    const store = initializeStore();
    state = store.getState();
    state.auth.user = {
      id: user.id,
      name: user.name,
      email: user.email,
      role: UserRole[user.role],
    };
  }

  return {
    pageProps: {
      initialReduxState: state,
    },
  };
};
