import { useRouter } from "next/router";
import React, { FC } from "react";
import useRestaurant from "~/api/restaurants/useRestaurant";
import MotorcycleIcon from "~/components/Icons/MotorcycleIcon";
import PhoneIcon from "~/components/Icons/PhoneIcon";
import Layout from "~/components/Layout/Layout";
import useDate from "~/services/useDate";
import styles from "./Restaurant.module.css";

const Retaurant: FC = () => {
  const router = useRouter();

  const { data: restaurant, isLoading, isError, error } = useRestaurant(
    router.query.id?.toString(),
    {
      enabled: router.query.id !== undefined,
    }
  );

  const { dayOfWeek } = useDate();

  if (router.query.id === undefined || isLoading) {
    return (
      <Layout title="FoodSnap">
        <p>Loading restaurant...</p>
      </Layout>
    );
  }

  if (isError) {
    return (
      <Layout title="Whoops! | FoodSnap">
        <p>Failed to load restaurant: {error.message}</p>
      </Layout>
    );
  }

  return (
    <Layout title={`${restaurant.name} | Order now on FoodSnap!`}>
      <header className={`py-20 text-gray-100 ${styles["header"]}`}>
        <div className="container">
          <div className="flex items-start">
            <div className="flex-1">
              <div className="flex items-start">
                <img
                  src="http://foodbakery.chimpgroup.com/wp-content/uploads/fb-restaurant-01-1.jpg"
                  width="95"
                  height="95"
                  alt={`${restaurant.name} logo`}
                  className="rounded-lg border-2 border-white"
                />

                <div className="ml-4">
                  <h1 className="text-4xl tracking-wider">{restaurant.name}</h1>
                  <div className="mt-2">
                    <a
                      href={`tel:${restaurant.phoneNumber}`}
                      className="inline-blockalign-middle text-lg text-gray-200 hover:text-primary-600 transition-colors"
                    >
                      <PhoneIcon className="w-5 h-5 inline-block align-middle" />
                      <span className="ml-1">{restaurant.phoneNumber}</span>
                    </a>
                  </div>
                </div>
              </div>
            </div>

            <div className="w-1/5">
              <div className="flex items-center">
                <div>
                  <MotorcycleIcon className="w-8 h-8" />
                </div>
                <div className="flex-1 ml-2 leading-snug tracking-wide">
                  <p>Delivery Fee: £{restaurant.deliveryFee.toFixed(2)}</p>
                  <p>
                    Min Order: £{restaurant.minimumDeliverySpend.toFixed(2)}
                  </p>
                </div>
              </div>

              <p className="mt-4 bg-white rounded-sm shadow-sm py-2 px-4 text-gray-900">
                <span className="text-primary">Today:</span>
                <span className="ml-2">
                  {restaurant.openingTimes[dayOfWeek].open} -{" "}
                  {restaurant.openingTimes[dayOfWeek].close ?? "midnight"}
                </span>
              </p>
            </div>
          </div>
        </div>
      </header>

      <main className="mt-4 p-4 container">
        <div className="flex">
          <div className="flex-1">Loading restaurant...</div>

          <aside className="w-1/5 bg-white rounded-sm shadow-sm p-4">
            <h2 className="font-semibold text-lg">Your Order</h2>
            <p>Loading your order...</p>
          </aside>
        </div>
      </main>
    </Layout>
  );
};

export default Retaurant;
