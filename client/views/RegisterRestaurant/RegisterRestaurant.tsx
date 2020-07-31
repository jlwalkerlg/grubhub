import React, { FC } from "react";
import Layout from "~/components/Layout/Layout";

export const RegisterRestaurant: FC = () => {
  return (
    <Layout title="Register Restaurant">
      <main>
        <div className="container mt-8">
          <div className="bg-white p-8 shadow-sm border-t-2 border-solid border-gray-300">
            <h2 className="text-2xl font-semibold text-gray-800 tracking-wider">
              Register Your Restaurant
            </h2>
            <p className="mt-2 text-gray-700">
              Enter the details of your restaurant and the restaurant&apos;s
              manager below. We will review your application and get back too
              you shortly.
            </p>

            <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
              Manager Details
            </p>

            <div className="mt-6">
              <label
                className="block text-sm text-gray-800 font-semibold"
                htmlFor="managerName"
              >
                Manager Name <span className="text-primary">*</span>
              </label>
              <input
                className="block w-full mt-2 py-2 px-4 border rounded-sm bg-gray-100"
                type="text"
                name="managerName"
                id="managerName"
              />
            </div>

            <div className="mt-4">
              <label
                className="block text-sm text-gray-800 font-semibold"
                htmlFor="managerEmail"
              >
                Manager Email <span className="text-primary">*</span>
              </label>
              <input
                className="block w-full mt-2 py-2 px-4 border rounded-sm bg-gray-100"
                type="email"
                name="managerEmail"
                id="managerEmail"
              />
            </div>

            <p className="text-gray-600 font-medium tracking-wide text-xl mt-8">
              Restaurant Details
            </p>

            <div className="mt-6">
              <label
                className="block text-sm text-gray-800 font-semibold"
                htmlFor="restaurantName"
              >
                Restaurant Name <span className="text-primary">*</span>
              </label>
              <input
                className="block w-full mt-2 py-2 px-4 border rounded-sm bg-gray-100"
                type="text"
                name="restaurantName"
                id="restaurantName"
              />
            </div>

            <div className="mt-4">
              <label
                className="block text-sm text-gray-800 font-semibold"
                htmlFor="restaurantPhone"
              >
                Restaurant Phone <span className="text-primary">*</span>
              </label>
              <input
                className="block w-full mt-2 py-2 px-4 border rounded-sm bg-gray-100"
                type="text"
                name="restaurantPhone"
                id="restaurantPhone"
              />
            </div>

            <div className="mt-4">
              <label
                className="block text-sm text-gray-800 font-semibold"
                htmlFor="restaurantPostCode"
              >
                Restaurant Post Code <span className="text-primary">*</span>
              </label>
              <input
                className="block w-full mt-2 py-2 px-4 border rounded-sm bg-gray-100"
                type="text"
                name="restaurantPostCode"
                id="restaurantPostCode"
              />
            </div>

            <div className="mt-8">
              <button className="btn btn-primary font-semibold w-full">
                Register
              </button>
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
};
