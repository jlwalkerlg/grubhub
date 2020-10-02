import { NextPage } from "next";
import React from "react";
import CloseIcon from "~/components/Icons/CloseIcon";
import PencilIcon from "~/components/Icons/PencilIcon";
import PlusIcon from "~/components/Icons/PlusIcon";
import useAuth from "~/store/auth/useAuth";
import { DashboardLayout } from "../DashboardLayout";

const categories = [
  {
    id: "1",
    name: "Pizza",
    items: [
      {
        id: "1",
        name: "Margherita",
        description: "Cheese & tomato",
        price: 9.99,
        menuCategoryId: "1",
      },
      {
        id: "2",
        name: "Hawaiian",
        description: "Ham & pineapple",
        price: 12.99,
        menuCategoryId: "1",
      },
    ],
    menuId: "1",
  },
  {
    id: "2",
    name: "Curries",
    items: [
      {
        id: "3",
        name: "Chicken Tikka Masala",
        description:
          "Tandoori style chicken with tikka marinade cooked in masala sauce",
        price: 11.99,
        menuCategoryId: "2",
      },
      {
        id: "4",
        name: "Lamb Rogan Josh",
        description:
          "Lamb meat cooked in rich, medium-spice, tomato-based sauce",
        price: 13.99,
        menuCategoryId: "2",
      },
    ],
    menuId: "1",
  },
];

const MenuBuilder: NextPage = () => {
  const { menu } = useAuth();

  return (
    <DashboardLayout>
      <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
        Menu Builder
      </h2>

      <div className="mt-4">
        {categories.map((category) => (
          <div key={category.id} className="mt-4">
            <div className="rounded bg-gray-100 px-4 py-3 shadow-sm text-primary font-medium">
              {category.name}
            </div>

            <div className="p-2">
              {category.items.map((item) => (
                <div key={item.id} className="px-4 py-2">
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

              <div className="rounded px-4 py-3 mt-2 border-dashed border border-gray-400 text-gray-600 font-medium flex items-center hover:shadow-sm cursor-pointer hover:bg-gray-100 hover:text-gray-700">
                <PlusIcon className="w-4 h-4" />
                <span className="ml-2">Add New Menu Item</span>
              </div>
            </div>
          </div>
        ))}
      </div>
    </DashboardLayout>
  );
};

export default MenuBuilder;
