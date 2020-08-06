import React, { FC, ReactNode, MouseEvent, useRef, useEffect } from "react";

interface Props {
  predictions: Array<{ id: string; description: ReactNode }>;
  onSelect(id: string): void;
  clear(): void;
}

const Autocomplete: FC<Props> = ({ predictions, onSelect, clear }) => {
  const onClick = useRef((e: MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();

    onSelect(e.currentTarget.dataset.id);
  }).current;

  const onButtonKeydown = (e: React.KeyboardEvent<HTMLButtonElement>) => {
    if (
      e.key === "Tab" &&
      !e.shiftKey &&
      e.currentTarget.dataset.id === predictions[predictions.length - 1].id
    ) {
      clear();
    }
  };

  const onDocumentKeydown = useRef((e: KeyboardEvent) => {
    if (e.key === "Escape") {
      clear();
    }
  }).current;

  useEffect(() => {
    document.addEventListener("keydown", onDocumentKeydown);

    return () => {
      document.removeEventListener("keydown", onDocumentKeydown);
    };
  }, []);

  return (
    <ul className="absolute top-100 w-full rounded-lg shadow">
      {predictions.map((x) => {
        return (
          <li key={x.id} className="w-full">
            <button
              onKeyDown={onButtonKeydown}
              data-id={x.id}
              onClick={onClick}
              className="py-2 px-4 w-full text-left bg-white hover:bg-gray-100 border-t border-gray-300 cursor-pointer"
              role="button"
            >
              {x.description}
            </button>
          </li>
        );
      })}
    </ul>
  );
};

export default Autocomplete;
