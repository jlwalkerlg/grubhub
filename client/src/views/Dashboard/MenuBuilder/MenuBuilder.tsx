import { NextPage } from "next";
import React from "react";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "../DashboardLayout";
import AddMenuCategoryForm from "./AddMenuCategoryForm";
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
                <div key={item.name} className="px-4 py-2">
                  <div className="flex items-center justify-between">
                    <p className="font-semibold">{item.name}</p>
                    <div className="flex items-center justify-between">
                      <button type="button" className="text-blue-700">
                        <PencilIcon className="w-5 h-5" />
                      </button>
                      <button type="button" className="text-primary ml-2">
                        <CloseIcon className="w-5 h-5" />
                      </button>
                    </div>
                  </div>
                  <p className="text-gray-800">{item.description}</p>
                  <p className="mt-1 font-medium text-sm text-red-700">
                    £{item.price}
                  </p>
                </div>
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
