import { AppProps } from "next/app";
import { QueryCache, ReactQueryCacheProvider } from "react-query";
import { Hydrate } from "react-query/hydration";
import "react-toastify/dist/ReactToastify.css";
import "~/styles/index.css";
import ErrorPage from "~/views/Error/ErrorPage";

const queryCache = new QueryCache({
  defaultConfig: {
    queries: {
      staleTime: 120 * 1000,
    },
  },
});

export default function App({ Component, pageProps }: AppProps) {
  if (pageProps.error !== undefined) {
    return <ErrorPage code={pageProps.error} />;
  }

  return (
    <ReactQueryCacheProvider queryCache={queryCache}>
      <Hydrate state={pageProps.dehydratedState}>
        <Component {...pageProps} />
      </Hydrate>
    </ReactQueryCacheProvider>
  );
}
