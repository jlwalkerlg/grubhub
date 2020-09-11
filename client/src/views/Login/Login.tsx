import React from "react";
import Layout from "~/components/Layout/Layout";
import LoginForm from "./LoginForm/LoginForm";
import { withGuestOnly } from "~/utils/withGuestOnly";

export const Login = withGuestOnly(() => {
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
});
