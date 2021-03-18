import Head from "next/head";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useAuth from "~/api/users/useAuth";
import Nav from "~/components/Nav/Nav";
import Toaster from "../Toaster/Toaster";

interface LayoutProps {
  title: string;
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ title, children }) => {
  return (
    <div className="py-16">
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

interface AuthLayoutProps extends LayoutProps {
  authorised?: boolean;
}

export const AuthLayout: FC<AuthLayoutProps> = ({
  authorised = true,
  children,
  ...rest
}) => {
  const { isLoggedIn, isLoading, user } = useAuth();
  const router = useRouter();

  if (!isLoading && !isLoggedIn) {
    router.push(`/login?redirect_to=${window.location.href}`);
  } else if (!isLoading && !authorised) {
    if (user.role === "RestaurantManager") {
      router.push("/dashboard");
    } else {
      router.push("/");
    }
  }

  return <Layout {...rest}>{isLoggedIn && children}</Layout>;
};

export default Layout;
