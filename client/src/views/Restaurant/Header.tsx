import React, { FC } from "react";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useDate from "~/services/useDate";
import { isRestaurantOpen, nextOpenDay } from "~/services/utils";

const Header: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const { dayOfWeek } = useDate();

  const formattedDeliveryFee =
    restaurant.deliveryFee === +restaurant.deliveryFee.toFixed()
      ? restaurant.deliveryFee.toFixed()
      : restaurant.deliveryFee.toFixed(2);

  const formattedMinOrder =
    restaurant.minimumDeliverySpend ===
    +restaurant.minimumDeliverySpend.toFixed()
      ? restaurant.minimumDeliverySpend.toFixed()
      : restaurant.minimumDeliverySpend.toFixed(2);

  const isOpen = isRestaurantOpen(restaurant.openingTimes[dayOfWeek]);
  const nextOpens = isOpen
    ? null
    : nextOpenDay(restaurant.openingTimes) ?? null;

  return (
    <div className="bg-white rounded border border-gray-200 pb-8 flex flex-col items-center justify-start">
      <img
        src="http://foodbakery.chimpgroup.com/wp-content/uploads/fb-restaurant-01-1.jpg"
        width="65"
        height="65"
        alt={`${restaurant.name} logo`}
        className="rounded border-2 border-white -mt-8"
      />

      <h1 className="font-bold text-3xl tracking-wider text-gray-800 text-center mt-4">
        {restaurant.name}
      </h1>

      <p className="mt-3 text-gray-700">
        {restaurant.cuisines.map((x) => (
          <span key={x.name} className="mx-1">
            {x.name}
          </span>
        ))}
      </p>

      <p className="mt-3 text-center text-gray-700 text-sm">
        {restaurant.address}
      </p>

      <hr className="w-full mt-6 mb-2 border-gray-300 md:hidden" />

      <div className="bg-gray-100 shadow-sm mt-4 text-gray-800">
        <div className="px-4 pb-2 pt-3">
          {isOpen && <p className="font-semibold">Delivering now</p>}
          {!isOpen && nextOpens && <p>Opens {nextOpens}</p>}
          {!isOpen && !nextOpens && <p>Closed</p>}
        </div>

        <hr />

        <div className="flex items-center px-4 pb-2 pt-3 text-sm">
          {formattedDeliveryFee === "0" ? (
            <p>
              <span className="font-bold">Free</span> delivery
            </p>
          ) : (
            <p>
              <span className="font-bold">£{formattedDeliveryFee}</span>{" "}
              delivery fee
            </p>
          )}

          {formattedMinOrder === "0" ? (
            <p className="ml-16">
              <span className="font-bold">No min order</span>
            </p>
          ) : (
            <p className="ml-16">
              <span className="font-bold">£{formattedMinOrder}</span> min order
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Header;
