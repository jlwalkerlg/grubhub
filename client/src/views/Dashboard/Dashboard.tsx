import React from "react";

import Layout from "~/components/Layout/Layout";
import DashboardIcon from "~/components/Icons/DashboardIcon";
import useAuth from "~/store/auth/useAuth";
import { withAuth } from "~/utils/withAuth";

export const Dashboard = withAuth(() => {
  const { restaurant } = useAuth();

  return (
    <Layout title="Dashboard">
      <main>
        <div className="restaurant-banner py-24">
          <div className="container">
            <h2 className="text-white text-4xl tracking-wider">
              {restaurant.name}
            </h2>
          </div>
        </div>

        <div className="container mt-8">
          <div className="flex items-stretch">
            <div className="w-1/4 bg-white shadow-sm">
              <ul>
                <li>
                  <a
                    href="/dashboard"
                    className="flex items-center hover:text-primary py-3 px-6 border-b border-gray-200 border-solid"
                  >
                    <DashboardIcon className="w-4 h-4 fill-current" />
                    <span className="uppercase ml-2 text-xs font-semibold tracking-wide">
                      Dashboard
                    </span>
                  </a>
                </li>
              </ul>
            </div>

            <div className="w-3/4 ml-4 bg-white p-8 shadow-sm border-t-2 border-solid border-gray-300">
              <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
                Welcome To Your Restaurant
              </h2>
              {restaurant.status === "PendingApproval" && (
                <p className="mt-2 text-gray-700">
                  Your restaurant is currently pending application. Please check
                  back later.
                </p>
              )}
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
});
