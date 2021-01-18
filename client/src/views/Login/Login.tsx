import { NextPage } from "next";
import { useRouter } from "next/router";
import React from "react";
import useAuth from "~/api/users/useAuth";
import Layout from "~/components/Layout/Layout";
import LoginForm from "./LoginForm";

const Login: NextPage = () => {
  const router = useRouter();

  const { isLoggedIn, user } = useAuth();

  if (isLoggedIn) {
    if (router.query.redirect_to) {
      router.push(router.query.redirect_to.toString());
    } else if (user.role === "RestaurantManager") {
      router.push("/dashboard");
    } else {
      router.push("/");
    }
    return null;
  }

  return (
    <Layout title="Login">
      <main>
        <div className="container mt-8">
          <div className="ring-1 ring-black ring-opacity-5 p-8 bg-white rounded-sm">
            <h2 className="text-center font-medium text-xl">Welcome Back</h2>
            <LoginForm />
          </div>
        </div>
      </main>
    </Layout>
  );
};

export default Login;
