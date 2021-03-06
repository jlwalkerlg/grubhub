import { NextPage } from "next";
import { useRouter } from "next/router";
import React from "react";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { DashboardLayout } from "../DashboardLayout";
import AddMenuCategoryForm from "./AddMenuCategoryForm";
import MenuCategory from "./MenuCategory";

const MenuBuilder: React.FC = () => {
  const { user } = useAuth();

  const { data: restaurant, isLoading, isError } = useRestaurant(
    user.restaurantId
  );

  if (isLoading) {
    return <SpinnerIcon className="h-6 w-6 animate-spin" />;
  }

  if (isError) {
    return <div className="my-2">Menu could not be loaded at this time.</div>;
  }

  return (
    <div className="mt-4 text-sm md:text-base">
      <AddMenuCategoryForm />

      {restaurant.menu?.categories.map((category) => (
        <MenuCategory key={category.name} category={category} />
      ))}
    </div>
  );
};

const MenuBuilderWrapper: NextPage = () => {
  const { user, isLoggedIn, isLoading: isLoadingAuth } = useAuth();

  const { isLoading: isLoadingMenu, isFetching } = useRestaurant(
    user?.restaurantId,
    {
      enabled: isLoggedIn,
    }
  );

  const router = useRouter();

  if (!isLoggedIn && !isLoadingAuth) {
    router.push("/login");
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
