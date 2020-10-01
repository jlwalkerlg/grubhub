import { MutableRefObject, useEffect } from "react";

export default function useClickAwayListener(
  ref: MutableRefObject<HTMLElement>,
  callback: () => void
) {
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (ref.current && !ref.current.contains(<HTMLElement>e.target)) {
        callback();
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [ref]);
}
