import { MutableRefObject, useCallback, useEffect } from "react";

export default function useFocusListener(
  ref: MutableRefObject<HTMLElement>,
  callback: () => any,
  deps: any[] = []
) {
  const cb = useCallback(callback, deps);

  useEffect(() => {
    ref.current?.addEventListener("focus", cb);
    return () => {
      ref.current?.removeEventListener("focus", cb);
    };
  }, [ref.current, cb]);
}
