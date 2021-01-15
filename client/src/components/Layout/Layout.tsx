import Head from "next/head";
import React from "react";
import Nav from "~/components/Nav/Nav";
import Toaster from "../Toaster/Toaster";

interface Props {
  title: string;
  children: React.ReactNode;
}

const Layout: React.FC<Props> = ({ title, children }) => {
  return (
    <div className="pt-16 pb-64 text-gray-900">
      <Head>
        <link rel="shortcut icon" href="/favicon.ico" />
        <title>{title}</title>
      </Head>
      <Nav />
      {children}
      <Toaster />
    </div>
  );
};

export default Layout;
