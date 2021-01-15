import { debounce, throttle } from "lodash";
import { useCallback, useEffect } from "react";

export default function useScroll(
  callback: () => any,
  wait?: number,
  deps: any[] = []
) {
  const throttled = useCallback(throttle(callback, wait), [deps]);
  const debounced = useCallback(debounce(callback, wait), [deps]);

  useEffect(() => {
    document.addEventListener("scroll", throttled);
    document.addEventListener("scroll", debounced);

    return () => {
      document.removeEventListener("scroll", throttled);
      document.removeEventListener("scroll", debounced);
    };
  }, [throttled]);
}
