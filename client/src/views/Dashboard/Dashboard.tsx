import { NextPage } from "next";
import React from "react";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "./DashboardLayout";

const Dashboard: NextPage = () => {
  const { restaurant } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Welcome To Your Restaurant
      </h2>

      {restaurant.status === "PendingApproval" && (
        <p className="mt-2 text-gray-700">
          Your restaurant is currently pending application. Please check back
          later.
        </p>
      )}
    </DashboardLayout>
  );
};

export default Dashboard;
