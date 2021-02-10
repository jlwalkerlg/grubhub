import Head from "next/head";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useAuth from "~/api/users/useAuth";
import Nav from "~/components/Nav/Nav";
import Toaster from "../Toaster/Toaster";

interface Props {
  title: string;
  children: React.ReactNode;
}

const Layout: React.FC<Props> = ({ title, children }) => {
  return (
    <div className="py-16 text-gray-900">
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

export const AuthLayout: FC<Props> = ({ title, children }) => {
  const { isLoggedIn, isLoading } = useAuth();
  const router = useRouter();

  if (!isLoggedIn && !isLoading) {
    router.push(`/login?redirect_to=${window.location.href}`);
  }

  return <Layout title={title}>{isLoggedIn && children}</Layout>;
};

export default Layout;
