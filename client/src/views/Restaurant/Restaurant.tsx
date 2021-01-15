import { useRouter } from "next/router";
import React, { FC, useCallback, useEffect, useState } from "react";
import { MenuItemDto } from "~/api/menu/MenuDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import Layout from "~/components/Layout/Layout";
import { useGoogleMap } from "~/services/geolocation/useGoogleMap";
import useDate from "~/services/useDate";
import useScroll from "~/services/useScroll";
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

  const { dayOfWeek } = useDate();

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

const MenuItem: FC<{ item: MenuItemDto }> = ({ item }) => {
  return (
    <button className="bg-white rounded border border-gray-200 p-4 w-full text-left mt-2">
      <p className="font-bold text-gray-700 text-xl">{item.name}</p>
      {item.description && (
        <p className="text-gray-700 text-sm mt-1">{item.description}</p>
      )}
      <p className="text-primary mt-2">£{item.price.toFixed(2)}</p>
    </button>
  );
};

const Menu: FC<{
  restaurant: RestaurantDto;
  setHash: (hash: string) => any;
}> = ({ restaurant, setHash }) => {
  const [listEl, setListEl] = useState<HTMLElement>();
  const [elements, setElements] = useState<HTMLElement[]>([]);

  useEffect(() => {
    setListEl(document.getElementById("categoryList"));

    setElements(
      Array.prototype.slice.call(
        document.querySelectorAll("#categoryList > div")
      )
    );
  }, []);

  useScroll(
    () => {
      if (!listEl || elements.length === 0) return;

      if (listEl.getBoundingClientRect().y > window.innerHeight) {
        setHash(elements[0]?.id ?? "");
        return;
      }

      for (let i = 0; i < elements.length; i++) {
        const el = elements[i];

        const rect = el.getBoundingClientRect();

        if (rect.y >= 64 && rect.y <= window.innerHeight / 2) {
          setHash(el.id);
          return;
        }

        if (rect.y > window.innerHeight / 2) {
          setHash(elements[i - 1]?.id ?? elements[0].id);
          return;
        }
      }

      setHash(elements[elements.length - 1].id);
    },
    500,
    [listEl, elements]
  );

  return (
    <div id="categoryList">
      {restaurant.menu.categories
        .filter((category) => category.items.length > 0)
        .map((category) => {
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
      {restaurant.menu.categories
        .filter((category) => category.items.length > 0)
        .map((category) => {
          const isCategoryOpen = openCategories.includes(category.name);

          return (
            <div
              key={category.name}
              id={category.name}
              className="border-b border-gray-400"
            >
              <button
                className="flex items-center justify-between p-4"
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
                  <ChevronIcon
                    direction="up"
                    className="h-8 w-8 text-primary"
                  />
                ) : (
                  <ChevronIcon
                    direction="down"
                    className="h-8 w-8 text-primary"
                  />
                )}
              </button>

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

  const [hash, setHash] = useState("");

  useEffect(() => setHash(restaurant?.menu.categories[0]?.name || ""), [
    restaurant,
  ]);

  const onClickCategoryLink = useCallback(
    (e: React.MouseEvent<HTMLAnchorElement>) => {
      e.preventDefault();

      const id = e.currentTarget.dataset.target;

      const el = document.getElementById(id);
      const nav = document.getElementById("nav");

      window.scrollBy({
        top: el.getBoundingClientRect().top - nav.offsetHeight,
        behavior: "smooth",
      });

      setHash(id);
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
              <ul className="sticky top-20 mt-8 text-gray-600">
                {restaurant.menu.categories.map((category) => {
                  return (
                    <li key={category.name}>
                      <a
                        href={`#${category.name}`}
                        data-target={category.name}
                        onClick={onClickCategoryLink}
                        className={`pl-2 py-2 block hover:text-gray-900 hover:font-semibold border-l border-gray-400 hover:border-gray-900 ${
                          hash === category.name
                            ? "text-gray-900 font-semibold border-gray-900"
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
                    <Menu restaurant={restaurant} setHash={setHash} />
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
