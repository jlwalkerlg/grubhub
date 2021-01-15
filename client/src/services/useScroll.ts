import { throttle } from "lodash";
import { useCallback, useEffect } from "react";

export default function useScroll(
  callback: () => any,
  wait?: number,
  deps: any[] = []
) {
  const cb = useCallback(throttle(callback, wait), [deps]);

  useEffect(() => {
    document.addEventListener("scroll", cb);

    return () => {
      document.removeEventListener("scroll", cb);
    };
  }, [cb]);
}
