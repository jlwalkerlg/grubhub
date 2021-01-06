import { NextPage } from "next";
import Link from "next/link";
import { useRouter } from "next/router";
import React, { useEffect, useRef, useState } from "react";
import useCuisines from "~/api/restaurants/useCuisines";
import useSearchRestaurants from "~/api/restaurants/useSearchRestaurants";
import CashIcon from "~/components/Icons/CashIcon";
import CheckIcon from "~/components/Icons/CheckIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import CreditCardIcon from "~/components/Icons/CreditCardIcon";
import CutleryIcon from "~/components/Icons/CutleryIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import MenuIcon from "~/components/Icons/MenuIcon";
import ThumbsUpIcon from "~/components/Icons/ThumbsUpIcon";
import Layout from "~/components/Layout/Layout";
import useClickAwayListener from "~/services/click-away-listener/useClickAwayListener";
import usePostcodeLookup from "~/services/geolocation/usePostcodeLookup";
import useFocusTrap from "~/services/useFocusTrap";
import useIsRouterReady from "~/services/useIsRouterReady";
import { getCurrentDayOfWeek, haversine, url } from "~/services/utils";
import styles from "./Restaurants.module.css";

const sortByWhitelist = ["delivery_fee", "time", "min_order", "distance"];

const RestaurantsSearch: React.FC = () => {
  const router = useRouter();

  const [isSortMenuOpen, setIsSortMenuOpen] = useState(false);

  const onClickSortLink = () => {
    setIsSortMenuOpen(false);
  };

  useEffect(() => {
    const listener = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        setIsSortMenuOpen(false);
      }
    };

    if (isSortMenuOpen) {
      document.addEventListener("keydown", listener);
    }

    return () => {
      if (isSortMenuOpen) {
        document.removeEventListener("keydown", listener);
      }
    };
  }, [isSortMenuOpen, setIsSortMenuOpen]);

  const sortByMobileMenuRef = useRef<HTMLDivElement>();

  useClickAwayListener(
    sortByMobileMenuRef,
    () => {
      if (isSortMenuOpen) {
        setIsSortMenuOpen(false);
      }
    },
    [isSortMenuOpen, setIsSortMenuOpen]
  );

  const onClickSortButton = () => {
    setIsSortMenuOpen(!isSortMenuOpen);
  };

  const [isFilterMenuOpen, setIsFilterMenuOpen] = useState(false);
  const [showFilterMenu, setShowFilterMenu] = useState(false);
  const filterMenuRef = useRef<HTMLDivElement>();

  const openFilterMenu = () => {
    setIsFilterMenuOpen(true);
    setShowFilterMenu(true);
  };

  const closeFilterMenu = () => {
    setShowFilterMenu(false);
  };

  useEffect(() => {
    const listener = () => {
      if (!showFilterMenu) {
        setIsFilterMenuOpen(false);
      }
    };

    filterMenuRef.current?.addEventListener("animationend", listener);

    return () => {
      filterMenuRef.current?.removeEventListener("animationend", listener);
    };
  }, [showFilterMenu]);

  const filterMenuStartRef = useRef<HTMLButtonElement>();
  const filterMenuEndRef = useRef<HTMLButtonElement>();
  useFocusTrap(isFilterMenuOpen, filterMenuStartRef, filterMenuEndRef);

  const postcode = router.query.postcode.toString();

  const {
    data: restaurants,
    isLoading: isLoadingRestaurants,
    isError: isSearchError,
    error: searchError,
  } = useSearchRestaurants({ ...router.query, postcode });

  const {
    data: coords,
    isLoading: isLoadingCoords,
    isError: isPostcodeLookupError,
    error: postcodeLookupError,
  } = usePostcodeLookup(postcode);

  const day = getCurrentDayOfWeek();

  const routeWithSortByParam = (sortBy: string) => {
    const params = { ...router.query, sort_by: sortBy };

    if (sortBy === null) {
      delete params["sort_by"];
    }

    if (Object.keys(params).length === 0) {
      return router.pathname;
    }

    return url(router.pathname, params);
  };

  const sortedBy = sortByWhitelist.includes(router.query.sort_by?.toString())
    ? router.query.sort_by
    : null;

  const {
    data: cuisines,
    isLoading: isLoadingCuisines,
    isError: isErrorCuisines,
  } = useCuisines();

  const filteredCuisines = router.query.cuisines?.toString().split(",") ?? [];

  const routeWithCuisine = (cuisine: string) => {
    const params = {
      ...router.query,
      cuisines: [...filteredCuisines, cuisine].sort().join(","),
    };

    return url(router.pathname, params);
  };

  const routeWithoutCuisine = (cuisine: string) => {
    const params = {
      ...router.query,
      cuisines: filteredCuisines.filter((x) => x !== cuisine).join(","),
    };

    if (!params.cuisines) {
      delete params.cuisines;
    }

    return url(router.pathname, params);
  };

  const routeWithoutCuisines = () => {
    const params = { ...router.query };

    if (params.cuisines) {
      delete params.cuisines;
    }

    return url(router.pathname, params);
  };

  return (
    <Layout title="Restaurants">
      <main>
        <div className="container">
          <div className="mt-6 flex">
            <div className="w-1/5 self-start px-4 pb-4 hidden lg:block">
              <p className="font-bold flex items-center px-2">
                <CutleryIcon className="w-4 h-4" />
                <span className="ml-2">Cuisines</span>
                <Link href={routeWithoutCuisines()}>
                  <a className="ml-auto text-primary text-sm font-medium">
                    Reset
                  </a>
                </Link>
              </p>

              {isLoadingCuisines && <p>Loading cuisines...</p>}

              {!isLoadingCuisines && isErrorCuisines && (
                <p>Error loading cuisines.</p>
              )}

              {!isLoadingCuisines && !isErrorCuisines && (
                <ul className="mt-2">
                  {cuisines.map((cuisine) => {
                    const isCuisineFiltered = filteredCuisines.includes(
                      cuisine.name
                    );

                    return (
                      <li key={cuisine.name}>
                        <Link
                          href={
                            isCuisineFiltered
                              ? routeWithoutCuisine(cuisine.name)
                              : routeWithCuisine(cuisine.name)
                          }
                        >
                          <a
                            className={`flex items-center py-2 px-3 mt-3 bg-white rounded-lg border border-gray-300 hover:border-gray-400 ${
                              styles["cuisine-link"]
                            } ${isCuisineFiltered ? styles["selected"] : ""}`}
                          >
                            <CheckIcon
                              className={`w-5 h-5 ${styles["cuisine-check"]}`}
                            />
                            <span className={styles["cuisine-name"]}>
                              {cuisine.name}
                            </span>
                          </a>
                        </Link>
                      </li>
                    );
                  })}
                </ul>
              )}
            </div>

            <div className="flex-1 lg:ml-6">
              <div className="lg:hidden flex justify-between items-center relative">
                <button
                  className="flex items-center cursor-pointer hover:text-primary"
                  onClick={onClickSortButton}
                >
                  <MenuIcon className="w-4 h-4" alt={3} />
                  <span className="ml-1">Sort</span>
                </button>

                <button
                  className="flex items-center cursor-pointer hover:text-primary"
                  onClick={openFilterMenu}
                >
                  <MenuIcon className="w-4 h-4" alt={3} />
                  <span className="ml-1">Filter</span>
                </button>

                {isSortMenuOpen && (
                  <div
                    ref={sortByMobileMenuRef}
                    className="absolute left-0 top-100 mt-2 bg-white p-4 rounded-sm shadow"
                  >
                    <p className="font-bold text-left">Sort By</p>
                    <ul className="mt-2">
                      <li>
                        <Link href={routeWithSortByParam(null)}>
                          <a
                            onClick={onClickSortLink}
                            className={`flex items-center py-1 hover:text-primary transition-colors ${
                              sortedBy === null ? "text-primary" : ""
                            }`}
                          >
                            <ThumbsUpIcon className="w-5 h-5" />
                            <span className="ml-2 font-light">Default</span>
                          </a>
                        </Link>
                      </li>
                      <li>
                        <Link href={routeWithSortByParam("distance")}>
                          <a
                            onClick={onClickSortLink}
                            className={`flex items-center py-1 hover:text-primary transition-colors ${
                              sortedBy === "distance" ? "text-primary" : ""
                            }`}
                          >
                            <LocationMarkerIcon className="w-5 h-5" />
                            <span className="ml-2 font-light">Distance</span>
                          </a>
                        </Link>
                      </li>
                      <li>
                        <Link href={routeWithSortByParam("delivery_fee")}>
                          <a
                            onClick={onClickSortLink}
                            className={`flex items-center py-1 hover:text-primary transition-colors ${
                              sortedBy === "delivery_fee" ? "text-primary" : ""
                            }`}
                          >
                            <CashIcon className="w-5 h-5" />
                            <span className="ml-2 font-light">
                              Delivery fee
                            </span>
                          </a>
                        </Link>
                      </li>
                      <li>
                        <Link href={routeWithSortByParam("min_order")}>
                          <a
                            onClick={onClickSortLink}
                            className={`flex items-center py-1 hover:text-primary transition-colors ${
                              sortedBy === "min_order" ? "text-primary" : ""
                            }`}
                          >
                            <CreditCardIcon className="w-5 h-5" />
                            <span className="ml-2 font-light">
                              Minimum order
                            </span>
                          </a>
                        </Link>
                      </li>
                      <li>
                        <Link href={routeWithSortByParam("time")}>
                          <a
                            onClick={onClickSortLink}
                            className={`flex items-center py-1 hover:text-primary transition-colors ${
                              sortedBy === "time" ? "text-primary" : ""
                            }`}
                          >
                            <ClockIcon className="w-5 h-5" />
                            <span className="ml-2 font-light">
                              Fastest delivery
                            </span>
                          </a>
                        </Link>
                      </li>
                    </ul>
                  </div>
                )}

                {isFilterMenuOpen && (
                  <div
                    ref={filterMenuRef}
                    className={`fixed top-0 left-0 w-screen h-screen z-50 bg-gray-100 flex flex-col transform transition-transform ${
                      showFilterMenu ? styles["slide-up"] : styles["slide-down"]
                    }`}
                  >
                    <div className="flex-0 bg-white shadow-md p-3 flex justify-between items-center">
                      <button
                        ref={filterMenuStartRef}
                        className="text-primary text-sm font-medium"
                        onClick={closeFilterMenu}
                      >
                        Cancel
                      </button>

                      <p className="font-semibold">Filters</p>

                      <Link href={routeWithoutCuisines()}>
                        <a
                          className="text-primary text-sm font-medium"
                          onClick={closeFilterMenu}
                        >
                          Reset
                        </a>
                      </Link>
                    </div>

                    <div className="flex-1 overflow-y-scroll p-3">
                      <p className="mt-1 ml-3 font-semibold">Cuisines</p>

                      {isLoadingCuisines && <p>Loading cuisines...</p>}

                      {!isLoadingCuisines && isErrorCuisines && (
                        <p>Error loading cuisines.</p>
                      )}

                      {!isLoadingCuisines && !isErrorCuisines && (
                        <ul className="text-gray-700">
                          {cuisines.map((cuisine) => {
                            const isCuisineFiltered = filteredCuisines.includes(
                              cuisine.name
                            );

                            return (
                              <li key={cuisine.name}>
                                <Link
                                  href={
                                    isCuisineFiltered
                                      ? routeWithoutCuisine(cuisine.name)
                                      : routeWithCuisine(cuisine.name)
                                  }
                                >
                                  <a
                                    className={`flex items-center py-2 px-3 mt-3 bg-white rounded-lg border border-gray-300 hover:border-gray-400 ${
                                      styles["cuisine-link"]
                                    } ${
                                      isCuisineFiltered
                                        ? styles["selected"]
                                        : ""
                                    }`}
                                  >
                                    <CheckIcon
                                      className={`w-5 h-5 ${styles["cuisine-check"]}`}
                                    />
                                    <span className={styles["cuisine-name"]}>
                                      {cuisine.name}
                                    </span>
                                  </a>
                                </Link>
                              </li>
                            );
                          })}
                        </ul>
                      )}
                    </div>

                    <div className="flex-0 bg-white shadow-md p-3">
                      <button
                        ref={filterMenuEndRef}
                        className="btn btn-primary w-full"
                        onClick={closeFilterMenu}
                      >
                        View Restaurants
                      </button>
                    </div>
                  </div>
                )}
              </div>

              {(isLoadingRestaurants || isLoadingCoords) && (
                <p className="mt-2 lg:mt-0">Loading restaurants...</p>
              )}

              {(isSearchError || isPostcodeLookupError) &&
                !isLoadingRestaurants &&
                !isLoadingCoords && (
                  <p className="mt-2 lg:mt-0">
                    {(searchError || postcodeLookupError).message}
                  </p>
                )}

              {!isLoadingRestaurants &&
                !isLoadingCoords &&
                !isSearchError &&
                !isPostcodeLookupError && (
                  <div className="mt-2 lg:mt-0">
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

                        const closingTime = restaurant.openingTimes[day].close;

                        return (
                          <Link
                            key={restaurant.id}
                            href={`/restaurants/${restaurant.id}`}
                          >
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
                                <p
                                  className={`font-bold ${styles["restaurant-name"]}`}
                                >
                                  {restaurant.name}
                                </p>

                                <div className="mt-1 lg:flex text-sm">
                                  <p className="flex-1 text-primary font-semibold">
                                    Closes {closingTime ?? "midnight"}
                                  </p>

                                  <div className="flex-1 lg:ml-2 text-gray-800 mt-2 lg:mt-0">
                                    <div className="flex items-center">
                                      <CashIcon className="w-5 h-5 mr-1 text-primary" />
                                      {restaurant.deliveryFee === 0 ? (
                                        <p>Free delivery</p>
                                      ) : (
                                        <p>
                                          Delivery £
                                          {restaurant.deliveryFee.toFixed(2)}
                                        </p>
                                      )}
                                      <p className="ml-3">
                                        Min. order £
                                        {restaurant.minimumDeliverySpend.toFixed(
                                          2
                                        )}
                                      </p>
                                    </div>
                                    <div className="flex items-center mt-1 lg:mt-2">
                                      <p className="flex items-center">
                                        <ClockIcon className="w-5 h-5 text-primary" />
                                        <span className="ml-1">
                                          {
                                            restaurant.estimatedDeliveryTimeInMinutes
                                          }{" "}
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
                )}
            </div>

            <div className="hidden lg:block w-1/5 self-start bg-white p-4 ml-6 rounded-sm shadow-sm">
              <p className="font-bold">Sort By</p>

              <ul className="mt-2">
                <li>
                  <Link href={routeWithSortByParam(null)}>
                    <a
                      className={`flex items-center py-1 hover:text-primary transition-colors ${
                        sortedBy === null ? "text-primary" : ""
                      }`}
                    >
                      <ThumbsUpIcon className="w-5 h-5" />
                      <span className="ml-2 font-light">Default</span>
                    </a>
                  </Link>
                </li>
                <li>
                  <Link href={routeWithSortByParam("distance")}>
                    <a
                      className={`flex items-center py-1 hover:text-primary transition-colors ${
                        sortedBy === "distance" ? "text-primary" : ""
                      }`}
                    >
                      <LocationMarkerIcon className="w-5 h-5" />
                      <span className="ml-2 font-light">Distance</span>
                    </a>
                  </Link>
                </li>
                <li>
                  <Link href={routeWithSortByParam("delivery_fee")}>
                    <a
                      className={`flex items-center py-1 hover:text-primary transition-colors ${
                        sortedBy === "delivery_fee" ? "text-primary" : ""
                      }`}
                    >
                      <CashIcon className="w-5 h-5" />
                      <span className="ml-2 font-light">Delivery fee</span>
                    </a>
                  </Link>
                </li>
                <li>
                  <Link href={routeWithSortByParam("min_order")}>
                    <a
                      className={`flex items-center py-1 hover:text-primary transition-colors ${
                        sortedBy === "min_order" ? "text-primary" : ""
                      }`}
                    >
                      <CreditCardIcon className="w-5 h-5" />
                      <span className="ml-2 font-light">Minimum order</span>
                    </a>
                  </Link>
                </li>
                <li>
                  <Link href={routeWithSortByParam("time")}>
                    <a
                      className={`flex items-center py-1 hover:text-primary transition-colors ${
                        sortedBy === "time" ? "text-primary" : ""
                      }`}
                    >
                      <ClockIcon className="w-5 h-5" />
                      <span className="ml-2 font-light">Fastest delivery</span>
                    </a>
                  </Link>
                </li>
              </ul>
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

  return <RestaurantsSearch />;
};

export default Restaurants;
