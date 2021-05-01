import { useRouter } from "next/router";
import React, { FC, useState } from "react";
import useRestaurant, { RestaurantDto } from "~/api/restaurants/useRestaurant";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import Layout, { ErrorLayout } from "~/components/Layout/Layout";
import CuisineList from "./CuisineList";
import Header from "./Header";
import InformationTab from "./InformationTab";
import Order from "./Order";
import { RestaurantMenu, RestaurantMenuMobile } from "./RestaurantMenu";

const tabs = ["Menu", "Information"];

const Tabs: FC<{
  currentTab: string;
  setTab: (tab: string) => any;
}> = ({ currentTab, setTab }) => {
  return (
    <div className="bg-white border border-gray-200 rounded text-gray-700 text-sm">
      <div className="flex w-72 mx-auto md:w-full">
        {tabs.map((tab) => {
          return (
            <button
              onClick={() => setTab(tab)}
              className={`flex-1 py-4 border-b-2 border-transparent hover:font-bold hover:border-primary-500 hover:text-base ${
                currentTab === tab
                  ? "font-bold border-primary-500 text-base"
                  : ""
              }`}
            >
              {tab}
            </button>
          );
        })}
      </div>
    </div>
  );
};

const RestaurantPage: FC<{
  restaurant: RestaurantDto;
}> = ({ restaurant }) => {
  const [currentTab, setTab] = useState("Menu");

  const [hash, setHash] = useState(restaurant.menu?.categories[0]?.name || "");

  return (
    <Layout title={`${restaurant.name} | Order now on GrubHub!`}>
      <div
        aria-hidden
        className="h-72"
        style={{
          background: `url(${restaurant.banner}) no-repeat scroll 0 0 / cover`,
        }}
      ></div>

      <div className="lg:container md:px-4">
        <div className="flex mt-4">
          <main className="flex-1">
            <div className="flex">
              <div className="w-1/4 hidden lg:block">
                <CuisineList
                  restaurant={restaurant}
                  hash={hash}
                  setHash={setHash}
                />
              </div>

              <div className="flex-1 lg:ml-4 md:-mt-36">
                <Header restaurant={restaurant} />

                <div className="md:my-4">
                  <Tabs currentTab={currentTab} setTab={setTab} />
                </div>

                {currentTab === "Menu" && (
                  <>
                    <div className="hidden lg:block">
                      <RestaurantMenu
                        restaurant={restaurant}
                        setHash={setHash}
                      />
                    </div>

                    <div className="lg:hidden">
                      <RestaurantMenuMobile restaurant={restaurant} />
                    </div>
                  </>
                )}

                {currentTab === "Information" && (
                  <InformationTab restaurant={restaurant} />
                )}
              </div>
            </div>
          </main>

          <aside className="self-start md:w-1/3 md:ml-4">
            <Order restaurant={restaurant} />
          </aside>
        </div>
      </div>
    </Layout>
  );
};

const RestaurantPageContainer: FC = () => {
  const router = useRouter();

  const {
    data: restaurant,
    isLoading: isRestaurantLoading,
    isError: isRestaurantError,
  } = useRestaurant(router.query.id?.toString(), {
    enabled: router.isReady,
  });

  const isLoading = !router.isReady || isRestaurantLoading;
  const isError = isRestaurantError;

  if (isLoading) {
    return (
      <Layout title="Order now on GrubHub" padded={false}>
        <div className="h-screen w-screen flex justify-center items-center">
          <SpinnerIcon className="animate-spin text-black w-8 h-8" />
        </div>
      </Layout>
    );
  }

  if (isError) return <ErrorLayout />;

  return <RestaurantPage restaurant={restaurant} />;
};

export default RestaurantPageContainer;
