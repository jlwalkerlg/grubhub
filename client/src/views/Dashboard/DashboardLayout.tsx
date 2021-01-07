import Link from "next/link";
import { useRouter } from "next/router";
import React from "react";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import BuildingIcon from "~/components/Icons/BuildingIcon";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import ClipboardIcon from "~/components/Icons/ClipboardIcon";
import ClockIcon from "~/components/Icons/ClockIcon";
import CutleryIcon from "~/components/Icons/CutleryIcon";
import IdentificationIcon from "~/components/Icons/IdentificationIcon";
import InfoIcon from "~/components/Icons/InfoIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import Layout from "~/components/Layout/Layout";

interface DashboardRoute {
  title: string;
  pathname: string;
  icon: React.FC<{ className: string }>;
}

const routes: DashboardRoute[] = [
  {
    title: "Restaurant Details",
    pathname: "/dashboard/restaurant-details",
    icon: BuildingIcon,
  },
  {
    title: "Opening Times",
    pathname: "/dashboard/opening-times",
    icon: ClockIcon,
  },
  {
    title: "Update Cuisines",
    pathname: "/dashboard/cuisines",
    icon: CutleryIcon,
  },
  {
    title: "Menu Builder",
    pathname: "/dashboard/menu-builder",
    icon: ClipboardIcon,
  },
  {
    title: "Manager Details",
    pathname: "/dashboard/manager-details",
    icon: IdentificationIcon,
  },
];

const menuLinkSize = 41;
const menuHeight = menuLinkSize * routes.length;

interface Props {
  route: DashboardRoute;
}

const MenuLinks: React.FC<Props> = ({ route }) => {
  return (
    <>
      {routes.map((x) => {
        return (
          <li key={x.pathname}>
            <Link href={x.pathname}>
              <a
                className={
                  "flex items-center hover:text-primary py-3 px-6 border-b border-gray-200 border-solid" +
                  (x === route ? " text-primary" : "")
                }
              >
                <x.icon className="w-4 h-4" />
                <span className="uppercase ml-2 text-xs font-semibold tracking-wide">
                  {x.title}
                </span>
              </a>
            </Link>
          </li>
        );
      })}
    </>
  );
};

export const Dashboard: React.FC<Props> = ({ children, route }) => {
  const { user } = useAuth();
  const { data: restaurant, isLoading, isError, error } = useRestaurant(
    user.restaurantId
  );

  const [isMenuOpen, setIsMenuOpen] = React.useState(false);

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  return (
    <main>
      <h1 className="sr-only">{route.title}</h1>

      <div className="restaurant-banner py-8 lg:py-16 xl:py-24">
        <div className="container">
          {isLoading && (
            <SpinnerIcon className="h-6 w-6 animate-spin text-gray-100" />
          )}
          {isError && (
            <div className="flex items-center">
              <InfoIcon className="h-6 w-6 inline-block text-red-600" />
              <p className="ml-2 text-gray-300">
                Failed to load restaurant: {error.message}
              </p>
            </div>
          )}
          {!isLoading && !isError && (
            <h2 className="text-white text-center lg:text-left text-2xl sm:text-3xl lg:text-4xl tracking-wider">
              {restaurant.name}
            </h2>
          )}
        </div>
      </div>

      <div className="container mt-4 lg:mt-8">
        <div className="lg:flex items-start">
          <div className="lg:w-1/4 bg-white shadow-sm">
            <div className="block lg:hidden">
              <button
                type="button"
                className="w-full flex items-center justify-center text-primary py-3 px-6 border-b border-gray-200 border-solid hover:shadow-sm"
                onClick={toggleMenu}
              >
                <span>Menu</span>
                <ChevronIcon
                  className={
                    "w-4 h-4 ml-2 transition-transform duration-150 ease-out" +
                    (isMenuOpen ? " transform rotate-180" : "")
                  }
                />
              </button>
              <ul
                className="overflow-hidden ease-out duration-300"
                style={{
                  height: isMenuOpen ? menuHeight : 0,
                  transitionProperty: "height",
                }}
              >
                <MenuLinks route={route} />
              </ul>
            </div>
            <div className="hidden lg:block">
              <ul>
                <MenuLinks route={route} />
              </ul>
            </div>
          </div>

          <div className="lg:w-3/4 lg:ml-4 mt-4 lg:mt-0 bg-white p-8 shadow-sm border-t-2 border-solid border-gray-300">
            {children}
          </div>
        </div>
      </div>
    </main>
  );
};

export const DashboardLayout: React.FC = (props) => {
  const router = useRouter();
  const route = routes.find((x) => x.pathname === router.pathname);

  const { isLoggedIn, isLoading } = useAuth();

  if (!isLoading && !isLoggedIn) {
    router.push("/login");
    return null;
  }

  return (
    <Layout title={route.title}>
      {isLoading && (
        <main className="h-24 flex justify-center items-center">
          <SpinnerIcon className="h-6 w-6 animate-spin" />
        </main>
      )}

      {!isLoading && isLoggedIn && <Dashboard {...props} route={route} />}
    </Layout>
  );
};
