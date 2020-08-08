import React, { FC, useRef, useEffect, useState } from "react";

import useClickAwayListener from "~/lib/ClickAwayListener/useClickAwayListener";

interface Props {
  predictions: Array<{ id: string; description: string }>;
  onSelect(id: string): void;
  children: HTMLInputElement;
}

const Autocomplete: FC<Props> = ({ predictions, onSelect, children }) => {
  const [isOpen, setIsOpen] = useState(predictions.length > 0);
  useEffect(() => {
    setIsOpen(predictions.length > 0);
  }, [predictions]);

  const onButtonClick = useRef((e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    onSelect(e.currentTarget.dataset.id);
  }).current;

  const onButtonKeydown = (e: React.KeyboardEvent<HTMLButtonElement>) => {
    if (
      e.key === "Tab" &&
      !e.shiftKey &&
      e.currentTarget.dataset.id === predictions[predictions.length - 1].id
    ) {
      setIsOpen(false);
    }
  };

  const onDocumentKeydown = useRef((e: KeyboardEvent) => {
    if (e.key === "Escape") {
      setIsOpen(false);
    }
  }).current;

  useEffect(() => {
    document.addEventListener("keydown", onDocumentKeydown);

    return () => {
      document.removeEventListener("keydown", onDocumentKeydown);
    };
  }, []);

  const onInputKeydown = React.useRef((e: KeyboardEvent) => {
    if (e.key === "Tab" && e.shiftKey) {
      setIsOpen(false);
    }
  }).current;

  const onInputFocus = React.useCallback(
    (e: KeyboardEvent) => {
      setIsOpen(predictions.length > 0);
    },
    [predictions.length]
  );

  const inputRef = React.useRef<HTMLInputElement>(null);

  useEffect(() => {
    inputRef.current.addEventListener("keydown", onInputKeydown);
    inputRef.current.addEventListener("focus", onInputFocus);

    return () => {
      inputRef.current.removeEventListener("keydown", onInputKeydown);
      inputRef.current.removeEventListener("focus", onInputFocus);
    };
  }, []);

  useEffect(() => {
    inputRef.current.addEventListener("focus", onInputFocus);

    return () => {
      inputRef.current.removeEventListener("focus", onInputFocus);
    };
  }, [onInputFocus]);

  const wrapperRef = useRef<HTMLDivElement>(null);
  useClickAwayListener(wrapperRef, () => setIsOpen(false));

  return (
    <div ref={wrapperRef} className="relative">
      {/* @ts-ignore */}
      {React.cloneElement(children, { ref: inputRef })}
      {isOpen && (
        <ul className="absolute top-100 w-full rounded-lg shadow">
          {predictions.map((x) => {
            return (
              <li key={x.id} className="w-full">
                <button
                  data-id={x.id}
                  onKeyDown={onButtonKeydown}
                  onClick={onButtonClick}
                  className="py-2 px-4 w-full text-left bg-white hover:bg-gray-100 border-t border-gray-300 cursor-pointer"
                  role="button"
                >
                  {x.description}
                </button>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};

export default Autocomplete;
