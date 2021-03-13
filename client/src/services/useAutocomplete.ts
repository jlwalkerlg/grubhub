import { MutableRefObject, useCallback, useEffect, useState } from "react";
import useClickAwayListener from "./useClickAwayListener";
import useEscapeKeyListener from "./useEscapeKeyListener";
import useFocusListener from "./useFocusListener";
import useFocusTrap from "./useFocusTrap";

export default function useAutocomplete(
  predictions: any[],
  inputRef: MutableRefObject<HTMLInputElement>,
  endRef: MutableRefObject<HTMLElement>,
  wrapperRef: MutableRefObject<HTMLElement>
) {
  const [isOpen, setIsOpen] = useState(false);

  useFocusTrap(isOpen, inputRef, endRef);

  useEffect(() => setIsOpen(predictions.length > 0), [predictions]);

  useFocusListener(inputRef, () => setIsOpen(predictions.length > 0), [
    predictions.length,
    setIsOpen,
  ]);

  useClickAwayListener(wrapperRef, () => setIsOpen(false), []);

  useEscapeKeyListener(() => setIsOpen(false), []);

  const close = useCallback(() => {
    setIsOpen(false);
  }, []);

  return { isOpen, close };
}
