import React, { FC } from "react";
import SpinnerIcon from "./Icons/SpinnerIcon";

const LoadingIconWrapper: FC<{ isLoading: boolean; className?: string }> = ({
  isLoading,
  className,
  children,
}) => {
  if (isLoading) {
    return <SpinnerIcon className={`${className} animate-spin`} />;
  }

  return <>{children}</>;
};

export default LoadingIconWrapper;
