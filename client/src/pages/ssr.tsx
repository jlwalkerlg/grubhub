import { useDispatch, useSelector } from "react-redux";
import { initializeStore } from "../store";

const useCounter = () => {
  const count = useSelector((state) => state.count);
  const dispatch = useDispatch();
  const increment = () =>
    dispatch({
      type: "INCREMENT",
    });
  const decrement = () =>
    dispatch({
      type: "DECREMENT",
    });
  const reset = () =>
    dispatch({
      type: "RESET",
    });

  return { count, increment, decrement, reset };
};

export default function SSR() {
  const { count, increment, decrement, reset } = useCounter();

  return (
    <div>
      <h1>
        Count: <span>{count}</span>
      </h1>
      <button onClick={increment}>+1</button>
      <button onClick={decrement}>-1</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}

// The date returned here will be different for every request that hits the page,
// that is because the page becomes a serverless function instead of being statically
// exported when you use `getServerSideProps` or `getInitialProps`
export function getServerSideProps() {
  const reduxStore = initializeStore();
  const { dispatch } = reduxStore;

  dispatch({
    type: "TICK",
    light: false,
    lastUpdate: Date.now(),
  });

  return { props: { initialReduxState: reduxStore.getState() } };
}
