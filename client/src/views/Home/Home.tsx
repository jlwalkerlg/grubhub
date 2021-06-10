import { NextPage } from "next";
import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/router";
import React from "react";
import GeolocationIcon from "~/components/Icons/GeolocationIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import Layout from "~/components/Layout/Layout";
import { useToasts } from "~/components/Toaster/Toaster";
import useCurrentLocation from "~/services/geolocation/useCurrentLocation";
import useForm from "~/services/useForm";
import { useRules } from "~/services/useRules";
import styles from "./Home.module.css";

const cities = [
  {
    name: "Manchester",
    postcode: "M2 4WU",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/indian.jpg",
    },
  },
  {
    name: "Birmingham",
    postcode: "B4 7ET",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/mexican.jpg",
    },
  },
  {
    name: "London",
    postcode: "SW1A 2DX",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/ramen.jpg",
    },
  },
  {
    name: "Leeds",
    postcode: "LS1 3AA",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/burger.jpg",
    },
  },
  {
    name: "Cardiff",
    postcode: "CF10 1GL",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/noodles.jpg",
    },
  },
  {
    name: "Glasgow",
    postcode: "G1 2AQ",
    img: {
      src: "http://d3bvhdd3xj1ghi.cloudfront.net/pizza.jpg",
    },
  },
];

const Home: NextPage = () => {
  const { addToast } = useToasts();

  const {
    location,
    isLoading: isLoadingLocation,
    getCurrentLocation,
  } = useCurrentLocation();

  const form = useForm({
    defaultValues: {
      postcode:
        location?.postcode ?? typeof window === "undefined"
          ? ""
          : window.localStorage.getItem("postcode") ?? "",
    },
    reValidateMode: "onSubmit",
  });

  const rules = useRules({
    postcode: (b) => b.required().postcode(),
  });

  const router = useRouter();

  const onSubmit = form.handleSubmit(async (data) => {
    localStorage.setItem("postcode", data.postcode);

    router.push(`/restaurants?postcode=${data.postcode}`);
  });

  const onClickLocation = async () => {
    getCurrentLocation({
      onSuccess: ({ postcode }) => {
        form.setValue("postcode", postcode);
      },

      onError: (error) => {
        addToast(error.message);
      },
    });
  };

  return (
    <Layout title="Home">
      <main>
        <h1 className="sr-only">Order takeaway on Grub Hub</h1>

        <div
          aria-hidden
          className={`overflow-hidden relative ${styles.banner}`}
        >
          <Image
            src="http://d3bvhdd3xj1ghi.cloudfront.net/home-banner.jpg"
            width={1800}
            height={1200}
            layout="responsive"
          />
        </div>

        <header className="mx-4">
          <div
            className={`bg-white p-4 md:p-8 md:pb-12 rounded shadow relative mx-auto ${styles.header}`}
          >
            <div className="text-center">
              <p className="font-bold text-3xl md:text-5xl text-red-700">
                Tuck into a takeaway today
              </p>

              <p className="mt-3 text-gray-700 text-lg">
                Find restaurants delivering right now, near you
              </p>
            </div>

            <form
              method="GET"
              action="/restaurants"
              onSubmit={onSubmit}
              className="mt-6 mx-auto"
              style={{ maxWidth: 611 }}
            >
              <div className="relative rounded-sm border bg-white text-gray-600">
                <button
                  type="button"
                  onClick={onClickLocation}
                  className="absolute right-1 top-1 p-1"
                >
                  {isLoadingLocation ? (
                    <SpinnerIcon className="w-6 h-6 animate-spin text-green-700" />
                  ) : (
                    <GeolocationIcon className="w-6 h-6 cursor-pointer text-green-700" />
                  )}
                </button>
                <input
                  ref={form.register({
                    validate: rules.postcode,
                  })}
                  className="border border-gray-100 bg-transparent appearance-none w-full py-2 pr-12 pl-3 text-gray-700 focus:outline-none focus:shadow-outline"
                  id="postcode"
                  name="postcode"
                  type="text"
                  placeholder="Enter your postcode"
                  data-invalid={!!form.errors.postcode}
                />
              </div>
              {form.errors.postcode && (
                <p className="form-error mt-1">
                  {form.errors.postcode.message}
                </p>
              )}
              <button
                disabled={form.formState.isSubmitting}
                type="submit"
                className="btn btn-primary w-full mt-3 normal-case font-semibold"
              >
                Search
              </button>
            </form>
          </div>
        </header>

        <div className="container mt-16">
          <h2 className="text-2xl font-semibold text-gray-800">
            Popular locations
          </h2>

          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 mt-4">
            {cities.map((city) => {
              return (
                <Link
                  key={city.name}
                  href={`/restaurants?postcode=${city.postcode}`}
                >
                  <a className="rounded overflow-hidden shadow-sm hover:shadow-lg">
                    <Image
                      src={city.img.src}
                      sizes="(min-width: 640px) 296px, (min-width: 768px) 360px, (min-width: 1024px) 320px, (min-width: 1280px) 405px, (min-width: 1536px) 490px, 560px"
                      width={1920}
                      height={1280}
                      layout="responsive"
                    />

                    <div className="p-2 pb-3">
                      <p className="font-semibold text-xl text-gray-700">
                        {city.name}
                      </p>
                    </div>
                  </a>
                </Link>
              );
            })}
          </div>
        </div>
      </main>
    </Layout>
  );
};

export default Home;
