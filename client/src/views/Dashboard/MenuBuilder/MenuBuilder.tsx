import { NextPage } from "next";
import Router from "next/router";
import React from "react";
import useMenu from "~/api/menu/useMenu";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { DashboardLayout } from "../DashboardLayout";
import AddMenuCategoryForm from "./AddMenuCategoryForm";
import MenuCategory from "./MenuCategory";

const MenuBuilder: React.FC = () => {
  const { user } = useAuth();

  const { data: menu, isLoading, isError, error } = useMenu(user.restaurantId);

  if (isLoading) {
    return <SpinnerIcon className="h-6 w-6 animate-spin" />;
  }

  if (isError) {
    return <div className="my-2">{error.message}</div>;
  }

  return (
    <div className="mt-4">
      <AddMenuCategoryForm />

      {menu.categories.map((category) => (
        <MenuCategory key={category.name} category={category} />
      ))}
    </div>
  );
};

const MenuBuilderWrapper: NextPage = () => {
  const { user, isLoggedIn, isLoading: isLoadingAuth } = useAuth();

  const { isLoading: isLoadingMenu, isFetching } = useMenu(
    isLoggedIn && user.restaurantId
  );

  if (!isLoggedIn && !isLoadingAuth) {
    Router.push("/login");
    return null;
  }

  return (
    <DashboardLayout>
      <div className="flex items-center">
        <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
          Menu Builder
        </h2>
        {!isLoadingMenu && isFetching && (
          <div className="ml-2">
            <SpinnerIcon className="w-6 h-6 animate-spin" />
          </div>
        )}
      </div>

      {!isLoggedIn && isLoadingAuth && (
        <div className="mt-2">
          <SpinnerIcon className="w-6 h-6 animate-spin" />
        </div>
      )}

      {isLoggedIn && <MenuBuilder />}
    </DashboardLayout>
  );
};

export default MenuBuilderWrapper;
