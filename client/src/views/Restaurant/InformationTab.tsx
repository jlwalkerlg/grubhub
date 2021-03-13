import React, { FC, useEffect } from "react";
import { RestaurantDto } from "~/api/restaurants/useRestaurant";
import { useGoogleMap } from "~/services/geolocation/useGoogleMap";
import useDate from "~/services/useDate";
import { formatAddress } from "~/services/utils";

const InformationTab: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const { map } = useGoogleMap(
    "restaurant-location-map",
    {
      center: { lat: restaurant.latitude, lng: restaurant.longitude },
      zoom: 14,
      mapTypeControl: false,
      streetViewControl: false,
    },
    {
      enabled: !!restaurant,
    }
  );

  useEffect(() => {
    let marker: google.maps.Marker;

    if (map) {
      marker = new google.maps.Marker({
        position: { lat: restaurant.latitude, lng: restaurant.longitude },
        map,
        title: restaurant.name,
        animation: google.maps.Animation.DROP,
      });
    }

    return () => {
      if (map) {
        marker.setMap(null);
      }
    };
  }, [map, restaurant]);

  const { dayOfWeek } = useDate();

  return (
    <div className="bg-white p-4 rounded border border-gray-200">
      <h3 className="font-bold text-lg text-gray-700 mt-2">Where to find us</h3>

      <div className="relative mt-3 rounded overflow-hidden h-80 bg-gray-200">
        <div id="restaurant-location-map" className="w-full h-full"></div>
        <div className="rounded bg-white border border-gray-300 p-2 text-gray-600 absolute bottom-8 left-4 text-sm">
          <span>{restaurant.addressLine1}</span>
          <br />
          {restaurant.addressLine2 && (
            <>
              <span>{restaurant.addressLine2}</span>
              <br />
            </>
          )}
          <span>{restaurant.city}</span>
          <br />
          <span>{restaurant.postcode}</span>
        </div>
      </div>

      <p className="font-semibold text-gray-700 mt-3 block md:hidden">
        {formatAddress(
          restaurant.addressLine1,
          restaurant.addressLine2,
          restaurant.city,
          restaurant.postcode
        )}
      </p>

      <hr className="my-4 border-gray-300 md:hidden" />

      {restaurant.description && (
        <>
          <h3 className="font-bold text-lg text-gray-700 mt-4">
            A little bit about us
          </h3>

          <p className="text-gray-700 mt-3 block whitespace-pre-line">
            {restaurant.description}
          </p>

          <hr className="my-6 border-gray-300" />
        </>
      )}

      <h3 className="font-bold text-lg text-gray-700 mt-6">Opening times</h3>

      <ul className="text-sm">
        {Object.keys(restaurant.openingTimes).map((day) => {
          return (
            <li
              key={day}
              className={`flex justify-between items-center py-4 border-b ${
                day === "sunday" ? "border-transparent" : "border-gray-300"
              } ${day === dayOfWeek ? "text-primary" : ""}`}
            >
              <div>{day.slice(0, 1).toUpperCase() + day.slice(1)}</div>
              <div>
                {restaurant.openingTimes[day].open} -{" "}
                {restaurant.openingTimes[day].close ?? "midnight"}
              </div>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default InformationTab;
