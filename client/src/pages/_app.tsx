import { AppProps } from "next/app";

import { Provider } from "react-redux";
import { useStore } from "../store/store";

import ErrorPage from "~/views/Error/ErrorPage";

import "react-toastify/dist/ReactToastify.css";
import "~/styles/index.css";

export default function App({ Component, pageProps }: AppProps) {
  if (pageProps.error !== undefined) {
    return <ErrorPage code={pageProps.error} />;
  }

  const store = useStore(pageProps.initialReduxState);

  return (
    <Provider store={store}>
      <Component {...pageProps} />
    </Provider>
  );
}
