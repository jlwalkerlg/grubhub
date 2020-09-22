import React from "react";
import { NextPage } from "next";

import Layout from "~/components/Layout/Layout";
import LoginForm from "./LoginForm/LoginForm";

export const Login: NextPage = () => {
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
