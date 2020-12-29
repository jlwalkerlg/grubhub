import { NextPage } from "next";
import Link from "next/link";
import { useRouter } from "next/router";
import React from "react";
import useIsRouterReady from "useIsRouterReady";
import useSearchRestaurants from "~/api/restaurants/useSearchRestaurants";
import CashIcon from "~/components/Icons/CashIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import Layout from "~/components/Layout/Layout";
import usePostcodeLookup from "~/services/geolocation/usePostcodeLookup";
import { getCurrentDayOfWeek, haversine } from "~/services/utils";
import styles from "./Restaurants.module.css";

const RestaurantsSearch: React.FC<{ postcode: string }> = ({ postcode }) => {
  const {
    data: restaurants,
    isLoading: isLoadingRestaurants,
    isError: isSearchError,
    error: searchError,
  } = useSearchRestaurants(postcode);

  const {
    data: coords,
    isLoading: isLoadingCoords,
    isError: isPostcodeLookupError,
    error: postcodeLookupError,
  } = usePostcodeLookup(postcode);

  if (isLoadingRestaurants || isLoadingCoords) {
    return (
      <Layout title="Restaurants">
        <main>
          <div className="container">
            <p>Loading restaurants...</p>
          </div>
        </main>
      </Layout>
    );
  }

  if (isSearchError || isPostcodeLookupError) {
    const error = searchError || postcodeLookupError;

    return (
      <Layout title="Restaurants">
        <main>
          <div className="container">
            <p>Error: {error.message}</p>
          </div>
        </main>
      </Layout>
    );
  }

  const day = getCurrentDayOfWeek();

  return (
    <Layout title="Restaurants">
      <main>
        <div className="container">
          <div className="mt-6">
            <div className="lg:w-2/3 mx-auto">
              <h1 className="font-bold text-lg">
                {restaurants.length} restaurant{restaurants.length > 1 && "s"}{" "}
                delivering to {postcode}
              </h1>

              <div className="mt-3">
                {restaurants.map((restaurant) => {
                  const distance = haversine(
                    {
                      latitude: restaurant.latitude,
                      longitude: restaurant.longitude,
                    },
                    coords
                  );

                  const closingTime = restaurant.openingTimes[day].close;

                  return (
                    <Link
                      key={restaurant.id}
                      href={`/restaurants/${restaurant.id}`}
                    >
                      <a
                        className={`flex bg-white p-4 rounded-sm shadow-sm hover:shadow cursor-pointer mt-3 ${styles["restaurant-section"]}`}
                      >
                        <div>
                          <img
                            src="http://foodbakery.chimpgroup.com/wp-content/uploads/fb-restaurant-01-1.jpg"
                            width="95"
                            height="95"
                            alt={`${restaurant.name} logo`}
                          />
                        </div>
                        <div className="ml-3 flex-1">
                          <p
                            className={`font-bold ${styles["restaurant-name"]}`}
                          >
                            {restaurant.name}
                          </p>

                          <div className="mt-1 lg:flex text-sm">
                            <div className="flex-1">
                              <p className="text-primary font-semibold">
                                Closes{" "}
                                {closingTime === "24:00"
                                  ? "midnight"
                                  : closingTime}
                              </p>
                            </div>

                            <div className="flex-1 lg:ml-2 text-gray-800 mt-2 lg:mt-0">
                              <p className="flex items-center">
                                <CashIcon className="w-5 h-5 mr-1 text-primary" />
                                {restaurant.deliveryFee === 0 ? (
                                  <span>Free delivery</span>
                                ) : (
                                  <span>
                                    Delivery £
                                    {restaurant.deliveryFee.toFixed(2)}
                                  </span>
                                )}
                                <span className="ml-3">
                                  Min. order £
                                  {restaurant.minimumDeliverySpend.toFixed(2)}
                                </span>
                              </p>
                              <div className="flex items-center mt-1 lg:mt-2">
                                <p className="flex items-center">
                                  <ClockIcon className="w-5 h-5 text-primary" />
                                  <span className="ml-1">
                                    {restaurant.estimatedDeliveryTimeInMinutes}{" "}
                                    mins
                                  </span>
                                </p>
                                <p className="flex items-center ml-3">
                                  <LocationMarkerIcon
                                    className="w-5 h-5 text-primary"
                                    solid
                                  />
                                  <span className="ml-1">
                                    {distance.toFixed(2)} km away
                                  </span>
                                </p>
                              </div>
                            </div>
                          </div>
                        </div>
                      </a>
                    </Link>
                  );
                })}
              </div>
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
};

const Restaurants: NextPage = () => {
  const isRouterReady = useIsRouterReady();

  const router = useRouter();

  if (!isRouterReady) {
    return null;
  }

  if (router.query.postcode === undefined) {
    router.push("/");
    return null;
  }

  return <RestaurantsSearch postcode={router.query.postcode.toString()} />;
};

export default Restaurants;
