import React, { FC, ReactNode } from "react";
import Head from "next/head";
import Nav from "~/components/Nav/Nav";

interface Props {
  title: string;
  children: ReactNode;
}

const Layout: FC<Props> = ({ title, children }) => {
  return (
    <>
      <Head>
        <link
          href="https://fonts.googleapis.com/icon?family=Material+Icons"
          rel="stylesheet"
        />
        <title>{title}</title>
      </Head>
      <Nav />
      {children}
    </>
  );
};

export default Layout;
