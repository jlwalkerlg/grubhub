import React from "react";

import useClickAwayListener from "~/services/click-away-listener/useClickAwayListener";

interface Props extends React.InputHTMLAttributes<HTMLInputElement> {
  inputRef?: (ref: HTMLInputElement) => void;
  predictions: Array<{ id: string; description: string }>;
  onSelection(id: string): void;
}

const Autocomplete: React.FC<Props> = ({
  inputRef: ref,
  predictions,
  onSelection,
  ...inputProps
}) => {
  const [isOpen, setIsOpen] = React.useState(predictions.length > 0);
  React.useEffect(() => {
    setIsOpen(predictions.length > 0);
  }, [predictions]);

  const [selectedIndex, setSelectedIndex] = React.useState<number>(0);

  const onButtonClick = React.useRef(
    (e: React.MouseEvent<HTMLButtonElement>) => {
      e.preventDefault();
      onSelection(e.currentTarget.dataset.id);
    }
  ).current;

  const onDocumentKeydown = React.useRef((e: KeyboardEvent) => {
    if (e.key === "Escape") {
      setIsOpen(false);
    }
  }).current;

  React.useEffect(() => {
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
        if (selectedIndex === 0) {
          setSelectedIndex(predictions.length - 1);
        } else {
          setSelectedIndex(selectedIndex - 1);
        }
      } else if (e.key === "ArrowDown") {
        if (selectedIndex === predictions.length - 1) {
          setSelectedIndex(0);
        } else {
          setSelectedIndex(selectedIndex + 1);
        }
      }
    },
    [selectedIndex, predictions.length]
  );

  const onInputKeydownEnter = React.useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter") {
        e.preventDefault();
        e.stopPropagation();

        onSelection(predictions[selectedIndex].id);
      }
    },
    [selectedIndex, predictions]
  );

  React.useEffect(() => {
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

  React.useEffect(() => {
    inputRef.current.addEventListener("keydown", onInputKeydownTab);

    return () => {
      inputRef.current.removeEventListener("keydown", onInputKeydownTab);
    };
  }, []);

  React.useEffect(() => {
    if (isOpen) {
      inputRef.current.addEventListener("keydown", onInputKeydownEnter);
    }

    return () => {
      if (isOpen) {
        inputRef.current.removeEventListener("keydown", onInputKeydownEnter);
      }
    };
  }, [onInputKeydownEnter, isOpen]);

  React.useEffect(() => {
    if (isOpen) {
      inputRef.current.addEventListener("keydown", onInputKeydownArrow);
    }

    return () => {
      if (isOpen) {
        inputRef.current.removeEventListener("keydown", onInputKeydownArrow);
      }
    };
  }, [selectedIndex, predictions.length, isOpen]);

  React.useEffect(() => {
    inputRef.current.addEventListener("focus", onInputFocus);

    return () => {
      inputRef.current.removeEventListener("focus", onInputFocus);
    };
  }, [onInputFocus]);

  const wrapperRef = React.useRef<HTMLDivElement>(null);
  useClickAwayListener(wrapperRef, () => setIsOpen(false));

  return (
    <div ref={wrapperRef} className="relative">
      <input
        ref={(e) => {
          inputRef.current = e;

          if (ref) {
            ref(e);
          }
        }}
        {...inputProps}
      />
      {isOpen && (
        <ul className="absolute top-100 w-full rounded-lg shadow">
          {predictions.map((x, index) => {
            return (
              <li key={x.id} className="w-full">
                <button
                  type="button"
                  tabIndex={-1}
                  data-id={x.id}
                  onClick={onButtonClick}
                  className={`py-2 px-4 w-full text-left bg-white hover:bg-gray-100 border-t border-gray-300 cursor-pointer ${
                    selectedIndex === index ? "bg-gray-100" : ""
                  }`}
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
