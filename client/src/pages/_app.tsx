import { AppProps } from "next/app";
import { NextPageContext } from "next";
import { AppContextType } from "next/dist/next-server/lib/utils";

import { Provider } from "react-redux";
import { useStore } from "../store/store";

import cookie from "cookie";
import jwt from "jsonwebtoken";
import { UserDto } from "~/api/dtos/UserDto";
import { initializeStore, State } from "~/store/store";
import { UserRole } from "~/store/auth/User";

import "~/styles/index.css";

export default function App({ Component, pageProps }: AppProps) {
  const store = useStore(pageProps.initialReduxState);

  return (
    <Provider store={store}>
      <Component {...pageProps} />
    </Provider>
  );
}

const getUser = async (context: NextPageContext): Promise<UserDto> => {
  const authUserCacheToken = cookie.parse(context.req.headers.cookie || "")[
    "auth_jwt"
  ];
  if (!authUserCacheToken) return null;

  try {
    const user: UserDto = jwt.verify(
      authUserCacheToken,
      process.env.JWT_SECRET
    ) as UserDto;

    return user;
  } catch (e) {
    context.res.setHeader("Set-Cookie", [
      cookie.serialize("auth_token", "", {
        expires: new Date(0),
      }),
      cookie.serialize("auth_jwt", "", {
        expires: new Date(0),
      }),
    ]);

    return null;
  }
};

App.getInitialProps = async (context: AppContextType) => {
  // don't run on client
  if (context.ctx.req === undefined) {
    return {
      pageProps: {
        initialReduxState: null,
      },
    };
  }

  const user = await getUser(context.ctx);

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
