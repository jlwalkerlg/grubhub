import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, useCallback, useEffect, useRef, useState } from "react";
import useCuisines from "~/api/cuisines/useCuisines";
import CashIcon from "~/components/Icons/CashIcon";
import CheckIcon from "~/components/Icons/CheckIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import CreditCardIcon from "~/components/Icons/CreditCardIcon";
import CutleryIcon from "~/components/Icons/CutleryIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import MenuIcon from "~/components/Icons/MenuIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import ThumbsUpIcon from "~/components/Icons/ThumbsUpIcon";
import Layout from "~/components/Layout/Layout";
import useClickAwayListener from "~/services/useClickAwayListener";
import useFocusTrap from "~/services/useFocusTrap";
import { url } from "~/services/utils";
import styles from "./Restaurants.module.css";
import RestaurantSearchResults from "./RestaurantSearchResults";

const sortByWhitelist = ["delivery_fee", "time", "min_order", "distance"];

const CuisineFilterList: FC = () => {
  const router = useRouter();

  const selectedCuisines = router.query.cuisines?.toString().split(",") ?? [];

  const { data: cuisines, isLoading, isError } = useCuisines();

  const routeWithCuisine = (cuisine: string) => {
    const params = {
      ...router.query,
      cuisines: [...selectedCuisines, cuisine].sort().join(","),
    };

    return url(router.pathname, params);
  };

  const routeWithoutCuisine = (cuisine: string) => {
    const params = {
      ...router.query,
      cuisines: selectedCuisines.filter((x) => x !== cuisine).join(","),
    };

    if (!params.cuisines) {
      delete params.cuisines;
    }

    return url(router.pathname, params);
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center mt-4">
        <SpinnerIcon className="h-6 w-6 animate-spin" />
      </div>
    );
  }

  if (isError) {
    return <p className="mt-2 px-2 text-sm">Cuisines failed to load.</p>;
  }

  return (
    <ul className="mt-2">
      {cuisines.map((cuisine) => {
        const isCuisineSelected = selectedCuisines.includes(cuisine.name);

        return (
          <li key={cuisine.name}>
            <Link
              href={
                isCuisineSelected
                  ? routeWithoutCuisine(cuisine.name)
                  : routeWithCuisine(cuisine.name)
              }
            >
              <a
                className={`flex items-center py-2 px-3 mt-3 bg-white rounded-lg border border-gray-300 hover:border-gray-400 ${
                  styles["cuisine-link"]
                } ${isCuisineSelected ? styles["selected"] : ""}`}
              >
                <CheckIcon className={`w-5 h-5 ${styles["cuisine-check"]}`} />
                <span className={styles["cuisine-name"]}>{cuisine.name}</span>
              </a>
            </Link>
          </li>
        );
      })}
    </ul>
  );
};

const MobileFilterMenu: FC<{
  show: boolean;
  routeWithoutCuisines: () => string;
  open: () => any;
  close: () => any;
  hide: () => any;
}> = ({ show, routeWithoutCuisines, open, close, hide }) => {
  const ref = useRef<HTMLDivElement>();
  const startRef = useRef<HTMLButtonElement>();
  const endRef = useRef<HTMLButtonElement>();

  useFocusTrap(show, startRef, endRef);

  useEffect(() => {
    const listener = () => {
      if (!show) {
        close();
      }
    };

    ref.current?.addEventListener("animationend", listener);

    return () => {
      ref.current?.removeEventListener("animationend", listener);
    };
  }, [show, close]);

  return (
    <div
      ref={ref}
      className={`fixed top-0 left-0 w-screen h-screen z-40 bg-gray-100 flex flex-col transform transition-transform ${
        show ? styles["slide-up"] : styles["slide-down"]
      }`}
    >
      <div className="flex-0 bg-white shadow-md p-3 flex justify-between items-center">
        <button
          ref={startRef}
          className="text-primary text-sm font-medium hover:underline"
          onClick={close}
        >
          Cancel
        </button>

        <p className="font-semibold">Filters</p>

        <Link href={routeWithoutCuisines()}>
          <a
            className="text-primary text-sm font-medium hover:underline"
            onClick={close}
          >
            Reset
          </a>
        </Link>
      </div>

      <div className="flex-1 overflow-y-scroll p-3">
        <p className="mt-1 ml-3 font-semibold">Cuisines</p>

        <CuisineFilterList />
      </div>

      <div className="flex-0 bg-white shadow-md p-3">
        <button ref={endRef} className="btn btn-primary w-full" onClick={close}>
          View Restaurants
        </button>
      </div>
    </div>
  );
};

const SortMenu: FC<{
  onClickSortLink?: () => any;
}> = ({ onClickSortLink }) => {
  const router = useRouter();

  const sortedBy = sortByWhitelist.includes(router.query.sort_by?.toString())
    ? router.query.sort_by.toString()
    : null;

  const getRouteWithSortByParam = (sortBy: string) => {
    const params = { ...router.query, sort_by: sortBy };

    if (sortBy === null) {
      delete params["sort_by"];
    }

    if (Object.keys(params).length === 0) {
      return router.pathname;
    }

    return url(router.pathname, params);
  };

  return (
    <div className="bg-white p-4 rounded-sm shadow lg:shadow-sm">
      <p className="font-bold">Sort By</p>

      <ul className="mt-2">
        <li>
          <Link href={getRouteWithSortByParam(null)}>
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
          <Link href={getRouteWithSortByParam("distance")}>
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
          <Link href={getRouteWithSortByParam("delivery_fee")}>
            <a
              onClick={onClickSortLink}
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
          <Link href={getRouteWithSortByParam("min_order")}>
            <a
              onClick={onClickSortLink}
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
          <Link href={getRouteWithSortByParam("time")}>
            <a
              onClick={onClickSortLink}
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
  );
};

const MobileSortMenu: FC<{
  isOpen: boolean;
  close: () => any;
}> = ({ isOpen, close }) => {
  useEffect(() => {
    const listener = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        close();
      }
    };

    if (isOpen) {
      document.addEventListener("keydown", listener);
    }

    return () => {
      if (isOpen) {
        document.removeEventListener("keydown", listener);
      }
    };
  }, [isOpen, close]);

  const ref = useRef<HTMLDivElement>();

  useClickAwayListener(
    ref,
    () => {
      if (isOpen) {
        close();
      }
    },
    [isOpen, close]
  );

  return (
    <div ref={ref} className="absolute left-0 top-100 mt-2">
      <SortMenu onClickSortLink={close} />
    </div>
  );
};

const RestaurantsPage: FC = () => {
  const router = useRouter();

  const [isSortMenuOpen, setIsSortMenuOpen] = useState(false);

  const openSortMenu = useCallback(() => setIsSortMenuOpen(true), []);

  const closeSortMenu = useCallback(() => setIsSortMenuOpen(false), []);

  const [isFilterMenuOpen, setIsFilterMenuOpen] = useState(false);
  const [showFilterMenu, setShowFilterMenu] = useState(false);

  const openFilterMenu = useCallback(() => {
    setIsFilterMenuOpen(true);
    setShowFilterMenu(true);
  }, []);

  const closeFilterMenu = useCallback(() => {
    setIsFilterMenuOpen(false);
  }, []);

  const hideFilterMenu = useCallback(() => {
    setShowFilterMenu(false);
  }, []);

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
                  <a className="ml-auto text-primary text-sm font-medium hover:underline">
                    Reset
                  </a>
                </Link>
              </p>

              <CuisineFilterList />
            </div>

            <div className="flex-1 lg:ml-6">
              <div className="lg:hidden flex justify-between items-center relative">
                <button
                  className="flex items-center cursor-pointer hover:text-primary"
                  onClick={isSortMenuOpen ? closeSortMenu : openSortMenu}
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
                  <MobileSortMenu
                    isOpen={isSortMenuOpen}
                    close={closeSortMenu}
                  />
                )}

                {isFilterMenuOpen && (
                  <MobileFilterMenu
                    routeWithoutCuisines={routeWithoutCuisines}
                    show={showFilterMenu}
                    open={openFilterMenu}
                    close={closeFilterMenu}
                    hide={hideFilterMenu}
                  />
                )}
              </div>

              <div className="mt-2 lg:mt-0">
                <RestaurantSearchResults />
              </div>
            </div>

            <div className="hidden lg:block w-1/5 self-start ml-6">
              <SortMenu />
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
};

const RestaurantsPageContainer: FC = () => {
  const router = useRouter();

  if (!router.isReady) {
    return (
      <Layout title="Restaurants | Grub Hub">
        <div className="flex items-center justify-center mt-8">
          <SpinnerIcon className="h-6 w-6 animate-spin" />
        </div>
      </Layout>
    );
  }

  if (!router.query.postcode) {
    router.push("/");
    return null;
  }

  return <RestaurantsPage />;
};

export default RestaurantsPageContainer;
