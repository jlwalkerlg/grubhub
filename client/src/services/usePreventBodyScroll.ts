import { useEffect } from "react";

export function usePreventBodyScroll(isActive: boolean = true) {
  useEffect(() => {
    if (isActive) {
      document.body.classList.add("overflow-y-hidden");
      document.body.classList.add("h-screen");
    }

    return () => {
      if (isActive) {
        document.body.classList.remove("overflow-y-hidden");
        document.body.classList.remove("h-screen");
      }
    };
  }, [isActive]);
}
