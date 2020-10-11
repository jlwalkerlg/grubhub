import { NextPage } from "next";
import React from "react";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "../DashboardLayout";
import AddMenuCategoryForm from "./AddMenuCategoryForm";
import MenuCategory from "./MenuCategory";

const MenuBuilder: NextPage = () => {
  const { menu } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Menu Builder
      </h2>

      <div className="mt-4">
        <AddMenuCategoryForm />

        {menu.categories.map((category) => (
          <MenuCategory key={category.name} category={category} />
        ))}
      </div>
    </DashboardLayout>
  );
};

export default MenuBuilder;
