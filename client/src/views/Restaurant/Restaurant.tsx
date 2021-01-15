import { useRouter } from "next/router";
import React, { FC, useCallback, useEffect, useState } from "react";
import { MenuItemDto } from "~/api/menu/MenuDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import Layout from "~/components/Layout/Layout";
import { useGoogleMap } from "~/services/geolocation/useGoogleMap";
import useDate from "~/services/useDate";
import { isRestaurantOpen, nextOpenDay } from "~/services/utils";
import styles from "./Restaurant.module.css";

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
    if (map) {
      new google.maps.Marker({
        position: { lat: restaurant.latitude, lng: restaurant.longitude },
        map,
        title: restaurant.name,
        animation: google.maps.Animation.DROP,
      });
    }
  }, [map]);

  return (
    <div className="bg-white p-4 rounded border border-gray-200">
      <h3 className="font-bold text-lg text-gray-700 mt-2">Where to find us</h3>

      <div className="relative mt-3 rounded overflow-hidden h-80 bg-gray-200">
        <div id="restaurant-location-map" className="w-full h-full"></div>
        <div className="rounded bg-white border border-gray-300 p-2 text-gray-600 absolute bottom-8 left-4 text-sm">
          {restaurant.address
            .replaceAll(",", ",,")
            .split(", ")
            .map((x) => (
              <span key={x}>
                {x}
                <br />
              </span>
            ))}
        </div>
      </div>

      <p className="font-semibold text-gray-700 mt-3 block md:hidden">
        {restaurant.address}
      </p>

      <hr className="my-4 border-gray-300 md:hidden" />

      <h3 className="font-bold text-lg text-gray-700 mt-6">Opening times</h3>

      <ul className="text-sm">
        {Object.keys(restaurant.openingTimes).map((dayOfWeek) => {
          return (
            <li
              key={dayOfWeek}
              className={`flex justify-between items-center py-4 border-b ${
                dayOfWeek === "sunday"
                  ? "border-transparent"
                  : "border-gray-300"
              }`}
            >
              <div>
                {dayOfWeek.slice(0, 1).toUpperCase() + dayOfWeek.slice(1)}
              </div>
              <div>
                {restaurant.openingTimes[dayOfWeek].open} -{" "}
                {restaurant.openingTimes[dayOfWeek].close ?? "midnight"}
              </div>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

const MenuItem: FC<{ item: MenuItemDto }> = ({ item }) => {
  return (
    <li
      className="bg-white rounded border border-gray-200 p-4 w-full text-left mt-2"
      role="button"
    >
      <p className="font-bold text-gray-700 text-xl">{item.name}</p>
      {item.description && (
        <p className="text-gray-700 text-sm mt-1">{item.description}</p>
      )}
      <p className="text-primary mt-2">£{item.price.toFixed(2)}</p>
    </li>
  );
};

const Menu: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  return (
    <div>
      {restaurant.menu.categories.map((category) => {
        return (
          <div key={category.name} id={category.name} className="py-4">
            <h3 className="font-bold text-gray-700 text-2xl">
              {category.name}
            </h3>

            <ul className="mt-4">
              {category.items.map((item) => {
                return <MenuItem key={item.name} item={item} />;
              })}
            </ul>
          </div>
        );
      })}
    </div>
  );
};

const MobileMenu: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const [openCategories, setOpenCategories] = useState<string[]>([]);

  const openCategory = (category: string) => {
    setOpenCategories([category, ...openCategories]);
  };

  const closeCategory = (category: string) => {
    setOpenCategories(openCategories.filter((x) => x !== category));
  };

  return (
    <div>
      {restaurant.menu.categories.map((category) => {
        const isCategoryOpen = openCategories.includes(category.name);

        return (
          <div
            key={category.name}
            id={category.name}
            className="border-b border-gray-400"
          >
            <div
              className="flex items-center justify-between p-4"
              role="button"
              onClick={
                isCategoryOpen
                  ? () => closeCategory(category.name)
                  : () => openCategory(category.name)
              }
            >
              <h3 className="font-bold text-gray-700 text-2xl">
                {category.name}
              </h3>

              {isCategoryOpen ? (
                <ChevronIcon direction="up" className="h-8 w-8 text-primary" />
              ) : (
                <ChevronIcon
                  direction="down"
                  className="h-8 w-8 text-primary"
                />
              )}
            </div>

            {isCategoryOpen && (
              <ul className="px-4 pb-4">
                {category.items.map((item) => {
                  return <MenuItem key={item.name} item={item} />;
                })}
              </ul>
            )}
          </div>
        );
      })}
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

  const { dayOfWeek } = useDate();

  const [tab, setTab] = useState<"Menu" | "Information">("Menu");

  const [hash, setHash] = useState<string>("");
  useEffect(() => setHash(window.location.hash), []);

  const onClickCategoryLink = useCallback(
    (e: React.MouseEvent<HTMLAnchorElement>) => {
      e.preventDefault();

      const href = e.currentTarget.href;
      const hash = href.slice(href.indexOf("#"));
      const id = hash.slice(1);

      const el = document.getElementById(id);
      const nav = document.getElementById("nav");

      window.scrollBy(0, el.getBoundingClientRect().top - nav.offsetHeight);
      window.history.replaceState({}, "", href);

      setHash(hash);
    },
    []
  );

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
    <Layout title={`${restaurant.name} | Order now on FoodSnap!`}>
      <div aria-hidden className={styles["header"]}></div>

      <main className="lg:container md:px-4">
        <div className="flex">
          <div className="flex-1 flex">
            <div className="w-1/4 hidden lg:block">
              <ul className="mt-8 border-l border-gray-400 text-gray-600">
                {restaurant.menu.categories.map((category) => {
                  return (
                    <li key={category.name}>
                      <a
                        href={`#${category.name}`}
                        onClick={onClickCategoryLink}
                        className={`pl-2 py-2 block hover:text-gray-900 hover:font-semibold ${
                          hash === `#${category.name}`
                            ? "text-gray-900 font-semibold"
                            : ""
                        }`}
                      >
                        {category.name}
                      </a>
                    </li>
                  );
                })}
              </ul>
            </div>

            <div className="flex-1 lg:ml-4">
              <div className="bg-white rounded border border-gray-200 pb-8 flex flex-col items-center justify-start md:-mt-32">
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
                        <span className="font-bold">
                          £{formattedDeliveryFee}
                        </span>{" "}
                        delivery fee
                      </p>
                    )}

                    {formattedMinOrder === "0" ? (
                      <p className="ml-16">
                        <span className="font-bold">No min order</span>
                      </p>
                    ) : (
                      <p className="ml-16">
                        <span className="font-bold">£{formattedMinOrder}</span>{" "}
                        min order
                      </p>
                    )}
                  </div>
                </div>
              </div>

              <div className="bg-white border border-gray-200 rounded text-gray-700 text-sm md:my-4">
                <div className="flex w-72 mx-auto md:w-full">
                  <button
                    onClick={() => setTab("Menu")}
                    className={`flex-1 py-4 border-b-2 border-transparent hover:font-bold hover:border-primary-500 hover:text-base ${
                      tab === "Menu"
                        ? "font-bold border-primary-500 text-base"
                        : ""
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

              {tab === "Menu" && (
                <>
                  <div className="hidden lg:block">
                    <Menu restaurant={restaurant} />
                  </div>

                  <div className="lg:hidden">
                    <MobileMenu restaurant={restaurant} />
                  </div>
                </>
              )}

              {tab === "Information" && (
                <InformationTab restaurant={restaurant} />
              )}
            </div>
          </div>

          <div className="w-1/3 self-start -mt-32 hidden md:block">
            <div className="ml-4 bg-white rounded border border-gray-200 p-4">
              <h2 className="font-bold text-2xl tracking-wider text-gray-800">
                Your order
              </h2>

              <hr className="my-3 border-gray-300" />

              <p className="text-gray-700">
                You haven't added any food to your order...{" "}
                <span className="font-semibold italic">yet</span>.
              </p>
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
};

export default Restaurant;
