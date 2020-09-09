import { useMemo } from "react";
import { createStore, applyMiddleware, combineReducers, Store } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";

import authReducer, { AuthState } from "./auth/authReducer";

export interface State {
  auth: AuthState;
}

let store: Store<State>;

function initStore(initialState?: State) {
  return createStore(
    combineReducers({
      auth: authReducer,
    }),
    initialState,
    composeWithDevTools(applyMiddleware())
  );
}

// For direct use from the server only.
export const initializeStore = (initialState?: State) => {
  // On the server, always return a new store.
  // It should only be initialised once on the server:
  // in the page component.
  if (typeof window === "undefined") {
    return initStore(initialState);
  }

  // On the client, return a new store but also cache it
  // in the `store` variable, so that it is carried over
  // to the next page and can be merged with a new store.
  let state: State;

  if (initialState !== undefined) {
    state = { ...(state || {}), ...initialState };
  }

  if (store !== undefined) {
    state = { ...(state || {}), ...store.getState() };
  }

  store = initStore(state);

  return store;
};

// For direct use from the client only.
export function useStore(initialState?: State) {
  return useMemo(() => initializeStore(initialState), [initialState]);
}
