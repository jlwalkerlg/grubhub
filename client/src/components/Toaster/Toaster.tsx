import React, {
  createContext,
  FC,
  useContext,
  useEffect,
  useRef,
  useState,
} from "react";
import CloseIcon from "../Icons/CloseIcon";

interface Toast {
  id: number;
  message: string;
}

const ToastContext = createContext<{
  toasts: Toast[];
  addToast: (message: string) => void;
  deleteToast: (id: number) => void;
}>({
  toasts: [],
  addToast: (x) => {},
  deleteToast: (x) => {},
});

export function useToasts() {
  return useContext(ToastContext);
}

export const ToastProvider: FC = ({ children }) => {
  const [toasts, setToasts] = useState<Toast[]>([]);
  const [nextId, setNextId] = useState(0);

  const addToast = (message: string) => {
    setToasts([...toasts, { id: nextId, message }]);
    setNextId(nextId + 1);
  };

  const deleteToast = (id: number) => {
    setToasts(toasts.filter((x) => x.id !== id));
  };

  return (
    <ToastContext.Provider value={{ toasts, addToast, deleteToast }}>
      {children}
    </ToastContext.Provider>
  );
};

const Toast: FC<{ toast: Toast }> = ({ toast }) => {
  const { deleteToast } = useToasts();

  const [isDeleting, setIsDeleting] = useState(false);

  const onClick = () => {
    setIsDeleting(true);
  };

  const ref = useRef<HTMLDivElement>();

  useEffect(() => {
    const listener = () => {
      deleteToast(toast.id);
    };

    ref.current.addEventListener("transitionend", listener);

    return () => {
      ref.current?.removeEventListener("transitionend", listener);
    };
  }, [deleteToast, toast.id]);

  const [timeUntilDelete, setTimeUntilDelete] = useState(5000);
  const [paused, setPaused] = useState(false);

  useEffect(() => {
    if (paused) return;

    const interval = 100;

    const callback = () => {
      if (timeUntilDelete <= 0) {
        deleteToast(toast.id);
      } else if (!paused) {
        setTimeUntilDelete(timeUntilDelete - interval);
      }
    };

    const handle = setInterval(callback, interval);

    return () => {
      clearInterval(handle);
    };
  }, [timeUntilDelete, setTimeUntilDelete, paused, deleteToast, toast.id]);

  const onMouseEnter = () => {
    setPaused(true);
  };

  const onMouseLeave = () => {
    setPaused(false);
  };

  return (
    <div
      onMouseEnter={onMouseEnter}
      onMouseLeave={onMouseLeave}
      ref={ref}
      className={`rounded shadow-lg bg-primary text-white mt-2 py-2 px-3 md:mt-3 md:py-3 text-sm flex items-start ${
        isDeleting ? "opacity-0 transition-opacity" : ""
      }`}
    >
      <p className="flex-1">{toast.message}</p>
      <button className="cursor-pointer p-1 ml-2" onClick={onClick}>
        <CloseIcon className="w-4 h-4" />
      </button>
    </div>
  );
};

const Toaster: FC = () => {
  const { toasts } = useToasts();

  return (
    <div className="fixed bottom-4 left-4 right-4 sm:left-1/2 sm:right-auto sm:transform sm:-translate-x-1/2 sm:w-96 md:left-4 md:transform-none overflow-y-hidden">
      {toasts.map((toast) => (
        <Toast key={toast.id} toast={toast} />
      ))}
    </div>
  );
};

export default Toaster;
