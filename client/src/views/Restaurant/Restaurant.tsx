import { useRouter } from "next/router";
import React, { FC, useEffect, useState } from "react";
import { ApiError } from "~/api/Api";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import Layout from "~/components/Layout/Layout";
import CuisineList from "./CuisineList";
import Header from "./Header";
import InformationTab from "./InformationTab";
import { Menu, MobileMenu } from "./Menu";
import Order from "./Order";

const Tabs: FC<{
  tab: string;
  setTab: (tab: "Menu" | "Information") => any;
}> = ({ tab, setTab }) => {
  return (
    <div className="bg-white border border-gray-200 rounded text-gray-700 text-sm">
      <div className="flex w-72 mx-auto md:w-full">
        <button
          onClick={() => setTab("Menu")}
          className={`flex-1 py-4 border-b-2 border-transparent hover:font-bold hover:border-primary-500 hover:text-base ${
            tab === "Menu" ? "font-bold border-primary-500 text-base" : ""
          }`}
        >
          Menu
        </button>
        <button
          onClick={() => setTab("Information")}
          className={`flex-1 py-4 border-b-2 border-transparent hover:font-bold hover:border-primary-500 hover:text-base ${
            tab === "Information"
              ? "font-bold border-primary-500 text-base"
              : ""
          }`}
        >
          Information
        </button>
      </div>
    </div>
  );
};

const Main: FC<{
  isLoading: boolean;
  isError: boolean;
  error: ApiError;
  restaurant: RestaurantDto;
}> = ({ isLoading, isError, error, restaurant }) => {
  const [tab, setTab] = useState<"Menu" | "Information">("Menu");

  const [hash, setHash] = useState("");

  useEffect(() => setHash(restaurant?.menu.categories[0]?.name || ""), [
    restaurant,
  ]);

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-full">
        <SpinnerIcon className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (isError) {
    return <p>Error loading restaurant: {error.message}</p>;
  }

  return (
    <div className="flex">
      <div className="w-1/4 hidden lg:block">
        <CuisineList restaurant={restaurant} hash={hash} setHash={setHash} />
      </div>

      <div className="flex-1 lg:ml-4 md:-mt-36">
        <Header restaurant={restaurant} />

        <div className="md:my-4">
          <Tabs tab={tab} setTab={setTab} />
        </div>

        {tab === "Menu" && (
          <>
            <div className="hidden lg:block">
              <Menu restaurant={restaurant} setHash={setHash} />
            </div>

            <div className="lg:hidden">
              <MobileMenu restaurant={restaurant} />
            </div>
          </>
        )}

        {tab === "Information" && <InformationTab restaurant={restaurant} />}
      </div>
    </div>
  );
};

const Restaurant: FC = () => {
  const router = useRouter();

  const { data: restaurant, isLoading, isError, error } = useRestaurant(
    router.query.id?.toString(),
    {
      enabled: router.query.id !== undefined,
    }
  );

  const loading = router.query.id === undefined || isLoading;

  const title = loading
    ? "FoodSnap"
    : isError
    ? "Whoops! | FoodSnap"
    : `${restaurant.name} | Order now on FoodSnap!`;

  return (
    <Layout title={title}>
      {!loading && !isError && (
        <div
          aria-hidden
          className="h-72"
          style={{
            background:
              "url(http://foodbakery.chimpgroup.com/wp-content/uploads/cover-photo17.jpg) no-repeat scroll 0 0 / cover",
          }}
        ></div>
      )}

      <div className="lg:container md:px-4">
        <div className="flex mt-4">
          <main className="flex-1">
            <Main
              isLoading={loading}
              isError={isError}
              error={error}
              restaurant={restaurant}
            />
          </main>

          <aside
            className={`self-start md:w-1/3 md:ml-4 ${
              loading || isError ? "md:mt-36" : ""
            }`}
          >
            <Order />
          </aside>
        </div>
      </div>
    </Layout>
  );
};

export default Restaurant;
