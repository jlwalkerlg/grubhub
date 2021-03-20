import { NextPage } from "next";
import { useRouter } from "next/router";
import React from "react";
import useAuth from "~/api/users/useAuth";
import Layout from "~/components/Layout/Layout";
import RegisterRestaurantForm from "./RegisterRestaurantForm/RegisterRestaurantForm";

const RegisterRestaurant: NextPage = () => {
  const { isLoggedIn, user } = useAuth();

  const router = useRouter();

  if (isLoggedIn) {
    if (user.role === "RestaurantManager") {
      router.push("/dashboard");
    } else {
      router.push("/");
    }
  }

  return (
    <Layout title="Register Restaurant">
      <main>
        <div className="container mt-8">
          <div className="bg-white p-8 shadow-sm border-t-2 border-solid border-gray-300">
            <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
              Register Your Restaurant
            </h2>
            <p className="mt-2 text-gray-700">
              Enter the details of your restaurant and the restaurant&apos;s
              manager below. We will review your application and get back too
              you shortly.
            </p>

            <RegisterRestaurantForm />
          </div>
        </div>
      </main>
    </Layout>
  );
};

export default RegisterRestaurant;
