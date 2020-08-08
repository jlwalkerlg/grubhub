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

  const [selectedIndex, setSelectedIndex] = useState<number>(0);

  const onButtonClick = useRef((e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    onSelect(e.currentTarget.dataset.id);
  }).current;

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

  const onInputKeydownTab = React.useRef((e: KeyboardEvent) => {
    if (e.key === "Tab") {
      setIsOpen(false);
    }
  }).current;

  const onInputKeydownArrow = React.useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "ArrowUp") {
        console.log("up", selectedIndex, predictions.length);
        if (selectedIndex === 0) {
          setSelectedIndex(predictions.length - 1);
        } else {
          setSelectedIndex(selectedIndex - 1);
        }
      } else if (e.key === "ArrowDown") {
        console.log("down", selectedIndex, predictions.length);
        if (selectedIndex === predictions.length - 1) {
          console.log("top");
          setSelectedIndex(0);
        } else {
          console.log("next");
          setSelectedIndex(selectedIndex + 1);
        }
      }
    },
    [selectedIndex, predictions.length]
  );

  useEffect(() => {
    if (!isOpen) {
      setSelectedIndex(0);
    }
  }, [isOpen]);

  const onInputFocus = React.useCallback(
    (e: KeyboardEvent) => {
      setIsOpen(predictions.length > 0);
    },
    [predictions.length]
  );

  const inputRef = React.useRef<HTMLInputElement>(null);

  useEffect(() => {
    inputRef.current.addEventListener("keydown", onInputKeydownTab);

    return () => {
      inputRef.current.removeEventListener("keydown", onInputKeydownTab);
    };
  }, []);

  useEffect(() => {
    if (isOpen) {
      inputRef.current.addEventListener("keydown", onInputKeydownArrow);
    }

    return () => {
      if (isOpen) {
        inputRef.current.removeEventListener("keydown", onInputKeydownArrow);
      }
    };
  }, [selectedIndex, predictions.length, isOpen]);

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
          {predictions.map((x, index) => {
            return (
              <li key={x.id} className="w-full">
                <button
                  tabIndex={-1}
                  data-id={x.id}
                  onClick={onButtonClick}
                  className={`py-2 px-4 w-full text-left bg-white hover:bg-gray-100 border-t border-gray-300 cursor-pointer ${
                    selectedIndex === index ? "bg-gray-100" : ""
                  }`}
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
