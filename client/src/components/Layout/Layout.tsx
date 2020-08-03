import React, { FC, ReactNode } from "react";
import Head from "next/head";
import Nav from "~/components/Nav/Nav";

interface Props {
  title: string;
  children: ReactNode;
}

const Layout: FC<Props> = ({ title, children }) => {
  return (
    <div className="pt-16 pb-64">
      <Head>
        <title>{title}</title>
      </Head>
      <Nav />
      {children}
    </div>
  );
};

export default Layout;
