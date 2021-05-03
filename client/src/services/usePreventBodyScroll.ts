import { useEffect } from "react";

export function usePreventBodyScroll(isActive: boolean = true) {
  useEffect(() => {
    if (isActive) {
      document.body.classList.add("overflow-hidden");
      document.body.classList.add("h-screen");
    }

    return () => {
      if (isActive) {
        document.body.classList.remove("overflow-hidden");
        document.body.classList.remove("h-screen");
      }
    };
  }, [isActive]);
}
