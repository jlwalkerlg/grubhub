import { AppProps } from "next/app";
import { QueryCache, ReactQueryCacheProvider } from "react-query";
import { Hydrate } from "react-query/hydration";
import "react-toastify/dist/ReactToastify.css";
import useIsAppMounted from "~/services/useIsAppMounted";
import "~/styles/index.css";
import ErrorPage from "~/views/Error/ErrorPage";

const queryCache = new QueryCache({
  defaultConfig: {
    queries: {
      staleTime: 120 * 1000,
    },
  },
});

function Wrapper({ Component, pageProps }: AppProps) {
  useIsAppMounted();

  return <Component {...pageProps} />;
}

export default function App(props: AppProps) {
  if (props.pageProps.error !== undefined) {
    return <ErrorPage code={props.pageProps.error} />;
  }

  return (
    <ReactQueryCacheProvider queryCache={queryCache}>
      <Hydrate state={props.pageProps.dehydratedState}>
        <Wrapper {...props} />
      </Hydrate>
    </ReactQueryCacheProvider>
  );
}
