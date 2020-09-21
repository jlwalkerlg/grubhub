import { NextPage } from "next";
import React from "react";

const errors = {
  403: "You are unauthorised to view this page.",
};

interface Props {
  code: number;
}

const ErrorPage: NextPage<Props> = ({ code }) => {
  const description = errors[code] ?? "Page failed to load.";

  return (
    <div className="flex items-center justify-center h-screen bg-white">
      <div>
        <div className="inline-block border-r border-gray-700 border-solid p-3 pr-4 text-2xl font-medium">
          {code}
        </div>
        <div className="inline-block pl-4">{description}</div>
      </div>
    </div>
  );
};

export default ErrorPage;
