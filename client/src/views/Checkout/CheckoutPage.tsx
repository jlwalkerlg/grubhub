import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import { useForm } from "react-hook-form";
import { useQueryCache } from "react-query";
import useBasket from "~/api/baskets/useBasket";
import { getOrderQueryKey } from "~/api/orders/useOrder";
import { usePlaceOrder } from "~/api/orders/usePlaceOrder";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { AuthLayout } from "~/components/Layout/Layout";
import {
  combineRules,
  MobileRule,
  PostcodeRule,
  RequiredRule,
} from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";

const CheckoutForm: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const router = useRouter();

  const { user } = useAuth();

  const cache = useQueryCache();

  const [placeOrder, { isError, error }] = usePlaceOrder();

  const form = useForm({
    defaultValues: {
      mobile: "",
      addressLine1: "",
      addressLine2: "",
      city: "",
      postcode: "",
    },
  });

  const handleSubmit = form.handleSubmit(async (data) => {
    await placeOrder(
      {
        restaurantId: restaurant.id,
        ...data,
      },
      {
        onSuccess: async (orderId) => {
          cache.prefetchQuery(getOrderQueryKey(orderId));
          await router.push(`/orders/${orderId}/pay`);
        },
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, form);
          }
        },
      }
    );
  });

  const firstName = user.name.split(" ")[0];

  return (
    <div>
      <h2 className="font-bold text-xl text-gray-800 text-center pb-2">
        {firstName}, confirm your details.
      </h2>

      {isError && (
        <p className="text-primary text-center mt-2">{error.message}</p>
      )}

      <form onSubmit={handleSubmit} className="mt-4">
        <div>
          <label className="label" htmlFor="mobile">
            Mobile Number <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: combineRules([new RequiredRule(), new MobileRule()]),
            })}
            className="input"
            type="text"
            name="mobile"
            id="mobile"
            data-invalid={!!form.errors.mobile}
          />
          {form.errors.mobile && (
            <p className="form-error mt-1">{form.errors.mobile.message}</p>
          )}
        </div>

        <div className="mt-6">
          <label className="label" htmlFor="addressLine1">
            Address Line 1 <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: combineRules([new RequiredRule()]),
            })}
            className="input"
            type="text"
            name="addressLine1"
            id="addressLine1"
            data-invalid={!!form.errors.addressLine1}
          />
          {form.errors.addressLine1 && (
            <p className="form-error mt-1">
              {form.errors.addressLine1.message}
            </p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="addressLine2">
            Address Line 2 (optional)
          </label>
          <input
            ref={form.register}
            className="input"
            type="text"
            name="addressLine2"
            id="addressLine2"
            data-invalid={!!form.errors.addressLine2}
          />
          {form.errors.addressLine2 && (
            <p className="form-error mt-1">
              {form.errors.addressLine2.message}
            </p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="city">
            City <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: combineRules([new RequiredRule()]),
            })}
            className="input"
            type="text"
            name="city"
            id="city"
            data-invalid={!!form.errors.city}
          />
          {form.errors.city && (
            <p className="form-error mt-1">{form.errors.city.message}</p>
          )}
        </div>

        <div className="mt-3">
          <label className="label" htmlFor="postcode">
            Postcode <span className="text-primary">*</span>
          </label>
          <input
            ref={form.register({
              validate: combineRules([new RequiredRule(), new PostcodeRule()]),
            })}
            className="input"
            type="text"
            name="postcode"
            id="postcode"
            data-invalid={!!form.errors.postcode}
          />
          {form.errors.postcode && (
            <p className="form-error mt-1">{form.errors.postcode.message}</p>
          )}
        </div>

        <button
          className="btn btn-primary w-full mt-6"
          disabled={form.formState.isSubmitting}
        >
          Go to payment
        </button>
      </form>
    </div>
  );
};

const Checkout: FC = () => {
  const router = useRouter();
  const restaurantId = router.query.id?.toString();
  const isLoadingRouter = !restaurantId;

  const {
    data: restaurant,
    isLoading: isLoadingRestaurant,
    isError: isRestaurantError,
  } = useRestaurant(restaurantId, {
    enabled: !isLoadingRouter,
  });

  const {
    data: basket,
    isLoading: isLoadingBasket,
    isError: isBasketError,
  } = useBasket(restaurantId, {
    enabled: !isLoadingRouter,
  });

  const isLoading = isLoadingRestaurant || isLoadingBasket || isLoadingRouter;
  const isError = isRestaurantError || isBasketError;

  if (isLoading) {
    return (
      <div className="flex flex-col justify-center items-center">
        <SpinnerIcon className="w-6 h-6 animate-spin" />
        <p className="mt-2 text-gray-700">Loading order.</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex flex-col justify-center items-center">
        <p>
          There was an error loading your order. Return to{" "}
          <Link href={`/restaurants/${restaurantId}`}>
            <a className="text-primary cursor-pointer">restaurant</a>
          </Link>{" "}
          and try again.
        </p>
      </div>
    );
  }

  if (basket?.items.length === 0) {
    router.push(`/restaurants/${restaurantId}`);
    return null;
  }

  return <CheckoutForm restaurant={restaurant} />;
};

const CheckoutPage: FC = () => {
  return (
    <AuthLayout title="Food Snap | Checkout">
      <div className="container max-w-xl mt-4">
        <div className="rounded-sm bg-white border border-gray-200 py-6 px-8">
          <Checkout />
        </div>
      </div>
    </AuthLayout>
  );
};

export default CheckoutPage;
