import { NextPage } from "next";
import { useRouter } from "next/router";
import React from "react";
import useIsRouterReady from "useIsRouterReady";
import useSearchRestaurants from "~/api/restaurants/useSearchRestaurants";
import Layout from "~/components/Layout/Layout";

const RestaurantsSearch: React.FC<{ postcode: string }> = ({ postcode }) => {
  const { data: restaurants, isLoading, isError, error } = useSearchRestaurants(
    postcode
  );

  if (isLoading) {
    return (
      <Layout title="Restaurants">
        <main>
          <div className="container">
            <p>Loading restaurants...</p>
          </div>
        </main>
      </Layout>
    );
  }

  if (isError) {
    return (
      <Layout title="Restaurants">
        <main>
          <div className="container">
            <h2>Whoops!</h2>
            <p>{error.message}</p>
          </div>
        </main>
      </Layout>
    );
  }

  return (
    <Layout title="Restaurants">
      <main>
        <div className="container">
          <h2>
            {restaurants.length} restaurant{restaurants.length > 1 && "s"}{" "}
            delivering to {postcode}
          </h2>
        </div>
      </main>
    </Layout>
  );
};

const Restaurants: NextPage = () => {
  const isRouterReady = useIsRouterReady();

  const router = useRouter();
  const { postcode } = router.query;

  if (!isRouterReady) {
    return null;
  }

  if (postcode === undefined) {
    router.push("/");
    return null;
  }

  return <RestaurantsSearch postcode={postcode.toString()} />;
};

export default Restaurants;
