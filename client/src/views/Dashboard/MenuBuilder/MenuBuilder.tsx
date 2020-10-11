import { NextPage } from "next";
import React from "react";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "../DashboardLayout";
import AddMenuCategoryForm from "./AddMenuCategoryForm";
import MenuItem from "./MenuItem";
import NewMenuItemDropdown from "./NewMenuItemDropdown";

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
          <div key={category.name} className="mt-4">
            <div className="rounded bg-gray-100 px-4 py-3 shadow-sm text-primary font-medium">
              {category.name}
            </div>

            <div className="p-2">
              {category.items.map((item) => (
                <MenuItem key={item.name} category={category} item={item} />
              ))}

              <NewMenuItemDropdown category={category} />
            </div>
          </div>
        ))}
      </div>
    </DashboardLayout>
  );
};

export default MenuBuilder;
