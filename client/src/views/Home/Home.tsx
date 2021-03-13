import { NextPage } from "next";
import Router from "next/router";
import React from "react";
import { useForm } from "react-hook-form";
import GeolocationIcon from "~/components/Icons/GeolocationIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import Layout from "~/components/Layout/Layout";
import { useToasts } from "~/components/Toaster/Toaster";
import useCurrentLocation from "~/services/geolocation/useCurrentLocation";
import { useRules } from "~/services/useRules";

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

  const rules = useRules(() => ({
    postcode: (builder) => builder.required().postcode(),
  }));

  const onSubmit = form.handleSubmit(async (data) => {
    if (form.formState.isSubmitting) return;

    localStorage.setItem("postcode", data.postcode);

    Router.push(`/restaurants?postcode=${data.postcode}`);
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
          <form
            method="GET"
            action="/restaurants"
            onSubmit={onSubmit}
            className="bg-primary rounded-sm py-4 px-4 mt-8 text-center max-w-4xl mx-auto"
          >
            <div className="relative rounded-sm border bg-white text-gray-600">
              <button
                type="button"
                onClick={onClickLocation}
                className="absolute right-1 top-1 p-1"
              >
                {isLoadingLocation ? (
                  <SpinnerIcon className="w-6 h-6 animate-spin text-green-500" />
                ) : (
                  <GeolocationIcon className="w-6 h-6 cursor-pointer text-green-500" />
                )}
              </button>
              <input
                ref={form.register({
                  validate: rules.postcode,
                })}
                className="shadow bg-transparent appearance-none w-full py-2 pr-12 pl-3 text-gray-700 focus:outline-none focus:shadow-outline"
                id="postcode"
                name="postcode"
                type="text"
                placeholder="Enter your postcode"
                data-invalid={!!form.errors.postcode}
              />
            </div>
            {form.errors.postcode && (
              <p className="form-error text-gray-100 mt-1">
                {form.errors.postcode.message}
              </p>
            )}
            <button
              disabled={form.formState.isSubmitting}
              type="submit"
              className="btn btn-secondary text-lg w-full mt-3"
            >
              Search
            </button>
          </form>
        </header>
      </main>
    </Layout>
  );
};

export default Home;
