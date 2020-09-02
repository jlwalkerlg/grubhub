import { AppProps } from "next/app";

import { Provider } from "react-redux";
import { useStore } from "../store";

import "~/styles/index.css";

function App({ Component, pageProps }: AppProps) {
  const store = useStore(pageProps.initialReduxState);

  return (
    <Provider store={store}>
      <Component {...pageProps} />
    </Provider>
  );
}

export default App;
