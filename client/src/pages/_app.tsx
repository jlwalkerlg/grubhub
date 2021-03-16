import { AppProps } from "next/app";
import React from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import { Hydrate } from "react-query/hydration";
import { ToastProvider } from "~/components/Toaster/Toaster";
import useIsAppMounted from "~/services/useIsAppMounted";
import "~/styles/index.css";
import ErrorPage from "~/views/Error/ErrorPage";

const queryClient = new QueryClient({
  defaultOptions: {
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
    <ToastProvider>
      <QueryClientProvider client={queryClient}>
        <Hydrate state={props.pageProps.dehydratedState}>
          <Wrapper {...props} />
        </Hydrate>
      </QueryClientProvider>
    </ToastProvider>
  );
}
