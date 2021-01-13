import { AppProps } from "next/app";
import Head from "next/head";
import React from "react";
import { QueryCache, ReactQueryCacheProvider } from "react-query";
import { Hydrate } from "react-query/hydration";
import { ToastProvider } from "~/components/Toaster/Toaster";
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
    <>
      <Head>
        <link rel="shortcut icon" href="/favicon.ico" />
      </Head>

      <ToastProvider>
        <ReactQueryCacheProvider queryCache={queryCache}>
          <Hydrate state={props.pageProps.dehydratedState}>
            <Wrapper {...props} />
          </Hydrate>
        </ReactQueryCacheProvider>
      </ToastProvider>
    </>
  );
}
