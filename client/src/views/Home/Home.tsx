import { NextPage } from "next";
import React from "react";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import Layout from "~/components/Layout/Layout";

const Home: NextPage = () => {
  return (
    <Layout title="Home">
      <main>
        <header className="container text-center">
          <p className="uppercase font-semibold pt-16 text-4xl tracking-widest">
            Hungry?
          </p>
          <p className="uppercase font-semibold text-2xl">
            What would you like to eat?
          </p>
          <p className="mt-4 text-lg">
            Enter your address to find nearby restaurants ready to serve fresh
            food straight to your door!
          </p>
          <div className="bg-primary rounded-sm py-4 px-4 mt-8 text-center">
            <div className="relative rounded-sm border bg-white text-gray-600">
              <span className="absolute left-0 top-0 pt-2 px-2">
                <LocationMarkerIcon className="w-6 h-6 text-gray-500" />
              </span>
              <input
                className="relative shadow bg-transparent appearance-none w-full py-2 pl-10 pr-3 text-gray-700 focus:outline-none focus:shadow-outline"
                id="address"
                type="text"
                placeholder="Enter your address"
              />
            </div>
            <button className="btn btn-secondary text-lg w-full mt-3">
              Search
            </button>
          </div>
        </header>
      </main>
    </Layout>
  );
};

export default Home;
