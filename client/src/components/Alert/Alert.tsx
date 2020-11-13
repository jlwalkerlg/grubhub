import React from "react";
import InfoIcon from "../Icons/InfoIcon";

interface AlertProps {
  title?: string;
  message: string;
}

export const ErrorAlert: React.FC<AlertProps> = ({ title, message }) => {
  return (
    <div
      className="bg-red-100 border-t-4 border-red-500 rounded-b text-red-900 px-4 py-3 shadow-md"
      role="alert"
    >
      <div className="flex">
        <div className={title ? "py-1" : undefined}>
          <InfoIcon className="h-6 w-6 text-red-500" />
        </div>
        <div className="ml-4">
          {title && <p className="font-bold">{title}</p>}
          <p className="text-sm">{message}</p>
        </div>
      </div>
    </div>
  );
};

export const SuccessAlert: React.FC<AlertProps> = ({ title, message }) => {
  return (
    <div
      className="bg-green-100 border-t-4 border-green-500 rounded-b text-green-900 px-4 py-3 shadow-md"
      role="alert"
    >
      <div className="flex">
        <div className={title ? "py-1" : undefined}>
          <InfoIcon className="h-6 w-6 text-green-500" />
        </div>
        <div className="ml-4">
          {title && <p className="font-bold">{title}</p>}
          <p className="text-sm">{message}</p>
        </div>
      </div>
    </div>
  );
};
