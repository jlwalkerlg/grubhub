import { MutableRefObject, useEffect } from "react";

export default function useClickAwayListener(
  ref: MutableRefObject<HTMLElement>,
  callback: () => any,
  deps: Array<any> = []
) {
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (ref.current && !ref.current.contains(<HTMLElement>e.target)) {
        callback();
      }
    }

    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, [ref, ...deps]);
}
