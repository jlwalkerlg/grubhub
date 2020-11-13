import { NextPage } from "next";
import Router from "next/router";
import React from "react";
import useAuth from "~/api/users/useAuth";
import Layout from "~/components/Layout/Layout";
import LoginForm from "./LoginForm/LoginForm";

const Login: NextPage = () => {
  const { isLoggedIn } = useAuth();

  if (isLoggedIn) {
    Router.push("/");
    return null;
  }

  return (
    <Layout title="Login">
      <main>
        <div className="container mt-8">
          <div className="shadow-xs p-8 bg-white rounded-sm">
            <h2 className="text-center font-medium text-xl">Welcome Back</h2>
            <LoginForm />
          </div>
        </div>
      </main>
    </Layout>
  );
};

export default Login;
