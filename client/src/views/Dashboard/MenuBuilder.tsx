import { NextPage } from "next";
import React from "react";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "./DashboardLayout";

const MenuBuilder: NextPage = () => {
  const { menu } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Menu Builder
      </h2>

      <pre>{JSON.stringify(menu, null, 2)}</pre>
    </DashboardLayout>
  );
};

export default MenuBuilder;
