import { useCallback, useEffect } from "react";

export default function useEscapeKeyListener(
  callback: () => any,
  deps: any[] = []
) {
  const cb = useCallback(callback, [callback, ...deps]);

  useEffect(() => {
    const listener = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        cb();
      }
    };

    document.addEventListener("keydown", listener);

    return () => {
      document.removeEventListener("keydown", listener);
    };
  }, [cb]);
}
