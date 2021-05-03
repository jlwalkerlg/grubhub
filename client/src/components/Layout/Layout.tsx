import Head from "next/head";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useAuth from "~/api/users/useAuth";
import Nav from "~/components/Nav/Nav";
import Toaster from "../Toaster/Toaster";

interface LayoutProps {
  title: string;
  padded?: boolean;
}

const Layout: FC<LayoutProps> = ({ title, padded = true, children }) => {
  return (
    <div className={padded ? "py-16" : undefined}>
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

export const ErrorLayout: FC<{
  title?: string;
  error?: string;
}> = ({ title, error }) => {
  return (
    <Layout title={title || "Error | GrubHub"} padded={false}>
      <div className="h-screen w-screen flex justify-center items-center bg-white">
        <p>{error || "An unexpected error occured."}</p>
      </div>
    </Layout>
  );
};

interface AuthLayoutProps extends LayoutProps {
  authorised?: boolean;
}

export const AuthLayout: FC<AuthLayoutProps> = ({
  authorised = true,
  children,
  ...layoutProps
}) => {
  const { isLoggedIn, isLoading, user } = useAuth();
  const router = useRouter();

  if (!isLoading) {
    if (!isLoggedIn) {
      router.push("/");
    } else if (!authorised) {
      router.push(user.role === "RestaurantManager" ? "/dashboard" : "/");
    }
  }

  return <Layout {...layoutProps}>{isLoggedIn && children}</Layout>;
};

export default Layout;
