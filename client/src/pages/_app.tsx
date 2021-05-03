import { AppProps } from "next/app";
import React, { FC, useRef } from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import { Hydrate } from "react-query/hydration";
import { ToastProvider } from "~/components/Toaster/Toaster";
import useIsAppMounted from "~/services/useIsAppMounted";
import "~/styles/index.css";
import ErrorPage from "~/views/Error/ErrorPage";

const ComponentWrapper: FC<AppProps> = ({ Component, pageProps }) => {
  useIsAppMounted();

  return <Component {...pageProps} />;
};

export default function App({ Component, pageProps }: AppProps) {
  const queryClientRef = useRef<QueryClient>();

  if (!queryClientRef.current) {
    queryClientRef.current = new QueryClient({
      defaultOptions: {
        queries: {
          staleTime: 120 * 1000,
        },
      },
    });
  }

  if (pageProps.error !== undefined) {
    return <ErrorPage code={pageProps.error} />;
  }

  return (
    <ToastProvider>
      <QueryClientProvider client={queryClientRef.current}>
        <Hydrate state={pageProps.dehydratedState}>
          <ComponentWrapper Component={Component} {...pageProps} />
        </Hydrate>
      </QueryClientProvider>
    </ToastProvider>
  );
}
