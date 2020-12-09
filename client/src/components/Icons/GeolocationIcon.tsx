import React from "react";

const GeolocationIcon: React.FC<{ className: string }> = ({ className }) => {
  return (
    <svg
      fill="currentColor"
      stroke="none"
      viewBox="0 0 24 24"
      className={className}
    >
      <path d="M2 11.8214L21 3L12.1786 22L10.1429 13.8571L2 11.8214ZM11.7882 12.2118L12.7455 16.0408L16.8936 7.10644L7.95923 11.2545L11.7882 12.2118Z"></path>
    </svg>
  );
};

export default GeolocationIcon;
