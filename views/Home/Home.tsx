import Head from "next/head";
import React, { FC } from "react";

export const Home: FC = () => {
  return (
    <div>
      <Head>
        <link
          href="https://fonts.googleapis.com/icon?family=Material+Icons"
          rel="stylesheet"
        />
        <title>Home</title>
      </Head>
      <h1 className="text-center uppercase">Home</h1>
    </div>
  );
};
