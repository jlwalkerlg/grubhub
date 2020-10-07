import React from "react";
import Head from "next/head";

import { ToastContainer } from "react-toastify";

import Nav from "~/components/Nav/Nav";

interface Props {
  title: string;
  children: React.ReactNode;
}

const Layout: React.FC<Props> = ({ title, children }) => {
  return (
    <div className="pt-16 pb-64">
      <Head>
        <title>{title}</title>
      </Head>
      <Nav />
      {children}
      <ToastContainer
        position="bottom-center"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
    </div>
  );
};

export default Layout;
