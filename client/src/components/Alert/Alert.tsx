import React, { FC } from "react";
import InfoIcon from "../Icons/InfoIcon";

interface AlertProps {
  title?: string;
  message: string;
}

export const ErrorAlert: FC<AlertProps> = ({ title, message }) => {
  return (
    <div
      className="bg-red-100 border-t-4 border-red-500 rounded-b text-red-900 px-4 py-3 shadow-md"
      role="alert"
    >
      <div className="flex">
        <div className={title ? "py-1" : undefined}>
          <InfoIcon className="fill-current h-6 w-6 text-red-500" />
        </div>
        <div className="ml-4">
          {title && <p className="font-bold">{title}</p>}
          <p className="text-sm">{message}</p>
        </div>
      </div>
    </div>
  );
};
