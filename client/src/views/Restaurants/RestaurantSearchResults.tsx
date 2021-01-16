import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import useSearchRestaurants from "~/api/restaurants/useSearchRestaurants";
import CashIcon from "~/components/Icons/CashIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import usePostcodeLookup from "~/services/geolocation/usePostcodeLookup";
import useDate from "~/services/useDate";
import { haversine } from "~/services/utils";
import styles from "./RestaurantSearchResults.module.css";

const RestaurantSearchResults: FC = () => {
  const router = useRouter();

  const postcode = router.query.postcode.toString();

  const {
    data: restaurants,
    isFetching: isFetchingRestaurants,
    isError: isSearchError,
    error: searchError,
  } = useSearchRestaurants({ ...router.query, postcode });

  const {
    data: coords,
    isLoading: isLoadingCoords,
    isError: isPostcodeLookupError,
    error: postcodeLookupError,
  } = usePostcodeLookup(postcode);

  const { dayOfWeek } = useDate();

  if (isFetchingRestaurants || isLoadingCoords) {
    return <p>Loading restaurants...</p>;
  }

  if (isSearchError || isPostcodeLookupError) {
    return <p>{(searchError || postcodeLookupError).message}</p>;
  }

  return (
    <div>
      <h1 className="font-bold text-lg">
        {restaurants.length} restaurant
        {restaurants.length > 1 && "s"} delivering to{" "}
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
                    src="http://foodbakery.chimpgroup.com/wp-content/uploads/fb-restaurant-01-1.jpg"
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
      </div>
    </div>
  );
};

export default RestaurantSearchResults;
