import React from "react";

import Layout from "~/components/Layout/Layout";
import useAuth from "~/store/auth/useAuth";
import { useRouter } from "next/router";
import BuildingIcon from "~/components/Icons/BuildingIcon";
import IdentificationIcon from "~/components/Icons/IdentificationIcon";

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
    title: "Manager Details",
    pathname: "/dashboard/manager-details",
    icon: IdentificationIcon,
  },
];

interface Props {
  children: React.ReactNode;
}

export const DashboardLayout: React.FC<Props> = ({ children }) => {
  const router = useRouter();
  const { restaurant } = useAuth();

  const route = routes.find((x) => x.pathname === router.pathname);

  return (
    <Layout title={route.title}>
      <main>
        <h1 className="sr-only">{route.title}</h1>

        <div className="restaurant-banner py-24">
          <div className="container">
            <h2 className="text-white text-4xl tracking-wider">
              {restaurant.name}
            </h2>
          </div>
        </div>

        <div className="container mt-8">
          <div className="flex items-start">
            <div className="w-1/4 bg-white shadow-sm">
              <ul>
                {routes.map((x) => {
                  return (
                    <li key={x.pathname}>
                      <a
                        href={x.pathname}
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
                    </li>
                  );
                })}
              </ul>
            </div>

            <div className="w-3/4 ml-4 bg-white p-8 shadow-sm border-t-2 border-solid border-gray-300">
              {children}
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
};
