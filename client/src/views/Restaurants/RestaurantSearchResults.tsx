import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import api from "~/api/api";
import useSearchRestaurants from "~/api/restaurants/useSearchRestaurants";
import CashIcon from "~/components/Icons/CashIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import usePostcodeLookup from "~/services/geolocation/usePostcodeLookup";
import useDate from "~/services/useDate";
import { haversine } from "~/services/utils";
import styles from "./RestaurantSearchResults.module.css";

const RestaurantSearchResults: FC = () => {
  const router = useRouter();

  const postcode = router.query.postcode.toString();

  const {
    data,
    isLoading: isLoadingRestaurants,
    isFetching: isFetchingRestaurants,
    error: searchRestaurantsError,
    fetchNextPage,
    hasNextPage,
  } = useSearchRestaurants({ ...router.query, perPage: 15, postcode });

  const {
    data: coords,
    isLoading: isLoadingCoords,
    error: postcodeLookupError,
  } = usePostcodeLookup(postcode);

  const isLoading = isLoadingRestaurants || isLoadingCoords;
  const error = searchRestaurantsError || postcodeLookupError;

  const { dayOfWeek } = useDate();

  if (isLoading) {
    return (
      <div className="flex items-center justify-center mt-8">
        <SpinnerIcon className="w-6 h-6 animate-spin" />
      </div>
    );
  }

  if (error) {
    return api.isApiError(error) ? (
      <p>Restaurants failed to load: {error.message}</p>
    ) : (
      <p>Restaurants failed to load.</p>
    );
  }

  const restaurants = data?.pages.map((x) => x.restaurants).flat() ?? [];
  const count = data?.pages[0]?.count || 0;

  return (
    <div>
      <h1 className="font-bold text-lg">
        {count} {count === 1 ? "restaurant" : "restaurants"} delivering to{" "}
        <span className="whitespace-nowrap">{postcode}</span>
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

          const closingTime = restaurant.openingTimes[dayOfWeek].close;

          return (
            <Link key={restaurant.id} href={`/restaurants/${restaurant.id}`}>
              <a
                className={`flex bg-white p-4 rounded-sm shadow-sm hover:shadow cursor-pointer mt-3 ${styles["restaurant-section"]}`}
              >
                <div className="hidden sm:block">
                  <img
                    src={restaurant.thumbnail}
                    width="95"
                    height="95"
                    alt={`${restaurant.name} logo`}
                  />
                </div>

                <div className="sm:ml-3 flex-1">
                  <p className={`font-bold ${styles["restaurant-name"]}`}>
                    {restaurant.name}
                  </p>

                  <div className="mt-1 xl:flex text-sm">
                    <p className="flex-1 text-primary font-semibold">
                      Closes {closingTime ?? "midnight"}
                    </p>

                    <div className="flex-1 xl:ml-2 text-gray-800 mt-2 xl:mt-0">
                      <div className="flex items-center">
                        <CashIcon className="w-5 h-5 mr-1 text-primary" />
                        {restaurant.deliveryFee === 0 ? (
                          <p>Free delivery</p>
                        ) : (
                          <p>Delivery £{restaurant.deliveryFee.toFixed(2)}</p>
                        )}
                        <p className="ml-3">
                          Min. order £
                          {restaurant.minimumDeliverySpend.toFixed(2)}
                        </p>
                      </div>
                      <div className="flex items-center mt-1 xl:mt-2">
                        <p className="flex items-center">
                          <ClockIcon className="w-5 h-5 text-primary" />
                          <span className="ml-1">
                            {restaurant.estimatedDeliveryTimeInMinutes} mins
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

        {hasNextPage && (
          <button
            className="btn btn-primary mt-2 normal-case w-full"
            onClick={() => fetchNextPage()}
            disabled={isFetchingRestaurants !== false}
          >
            View more
          </button>
        )}
      </div>
    </div>
  );
};

export default RestaurantSearchResults;
