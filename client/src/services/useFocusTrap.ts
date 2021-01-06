import { MutableRefObject, useEffect } from "react";

export default function useFocusTrap(
  active: boolean,
  start: MutableRefObject<HTMLElement>,
  end?: MutableRefObject<HTMLElement>
) {
  useEffect(() => {
    if (active) {
      start.current?.focus();
    }
  }, [active]);

  useEffect(() => {
    const startListener = (e: KeyboardEvent) => {
      if (e.key === "Tab" && e.shiftKey) {
        e.preventDefault();
        end?.current?.focus();
      }
    };

    const endListener = (e: KeyboardEvent) => {
      if (e.key === "Tab" && !e.shiftKey) {
        e.preventDefault();
        start.current?.focus();
      }
    };

    if (active) {
      start.current?.addEventListener("keydown", startListener);
      end?.current?.addEventListener("keydown", endListener);
    }

    return () => {
      if (active) {
        start.current?.removeEventListener("keydown", startListener);
        end?.current?.removeEventListener("keydown", endListener);
      }
    };
  }, [active, start.current, end?.current]);
}
