import { MutableRefObject, useCallback, useEffect } from "react";

export default function useClickAwayListener(
  ref: MutableRefObject<HTMLElement>,
  callback: () => any,
  deps: any[] = []
) {
  const cb = useCallback(callback, deps);

  useEffect(() => {
    const listener = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(<HTMLElement>e.target)) {
        cb();
      }
    };

    document.addEventListener("click", listener);
    return () => {
      document.removeEventListener("click", listener);
    };
  }, [ref.current, cb]);
}
