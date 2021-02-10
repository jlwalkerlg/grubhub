import {
  CardElement,
  Elements,
  useElements,
  useStripe,
} from "@stripe/react-stripe-js";
import { loadStripe, StripeCardElementOptions } from "@stripe/stripe-js";
import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC } from "react";
import { useForm } from "react-hook-form";
import { useQueryCache } from "react-query";
import { OrderDto } from "~/api/orders/OrderDto";
import useActiveOrder, {
  getActiveOrderQueryKey,
} from "~/api/orders/useActiveOrder";
import { getOrderQueryKey } from "~/api/orders/useOrder";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useRestaurant from "~/api/restaurants/useRestaurant";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import TruckIcon from "~/components/Icons/TruckIcon";
import { AuthLayout } from "~/components/Layout/Layout";
import {
  combineRules,
  PostcodeRule,
  RequiredRule,
} from "~/services/forms/Rule";
import { setFormErrors } from "~/services/forms/setFormErrors";
import "./CheckoutPage.module.css";
import useCheckout from "./useCheckout";

const stripePromise =
  typeof window === "undefined"
    ? undefined
    : loadStripe(process.env.NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY);

const CARD_ELEMENT_OPTIONS: StripeCardElementOptions = {};

const CheckoutForm: FC<{ order: OrderDto; restaurant: RestaurantDto }> = ({
  order,
  restaurant,
}) => {
  const stripe = useStripe();
  const elements = useElements();

  const router = useRouter();

  const cache = useQueryCache();

  const [checkout, { isError, error }] = useCheckout();

  const form = useForm({
    defaultValues: {
      addressLine1: "",
      addressLine2: "",
      addressLine3: "",
      city: "",
      postcode: "",
    },
  });

  const handleSubmit = form.handleSubmit(async (data) => {
    if (!stripe || !elements || form.formState.isSubmitting) {
      return;
    }

    await checkout(
      {
        restaurantId: restaurant.id,
        orderId: order.id,
        ...data,
      },
      {
        onSuccess: async () => {
          cache.removeQueries(getOrderQueryKey(order.id));
          cache.prefetchQuery(getOrderQueryKey(order.id));
          await router.push(`/orders/${order.id}`);
          cache.removeQueries(getActiveOrderQueryKey(restaurant.id));
        },
        onError: (error) => {
          if (error.isValidationError) {
            setFormErrors(error.errors, form);
          }
        },
      }
    );
  });

  return (
    <div className="flex-1 rounded bg-white border border-gray-300 p-2 ">
      <h2 className="font-bold text-xl text-gray-700">Payment details</h2>

      {isError && <p className="text-primary mt-1 mb-4">{error.message}</p>}

      <form onSubmit={handleSubmit} className="mt-2">
        <div>
          <label htmlFor="card" className="label">
            Card details <span className="text-primary">*</span>
          </label>
          <div className="input px-2">
            <CardElement id="card" options={CARD_ELEMENT_OPTIONS} />
          </div>
        </div>

        <div className="mt-3">
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
            Address Line 2
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
          <label className="label" htmlFor="addressLine3">
            Address Line 3
          </label>
          <input
            ref={form.register}
            className="input"
            type="text"
            name="addressLine3"
            id="addressLine3"
            data-invalid={!!form.errors.addressLine3}
          />
          {form.errors.addressLine3 && (
            <p className="form-error mt-1">
              {form.errors.addressLine3.message}
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
          className="btn btn-primary w-full mt-4"
          disabled={!stripe || !elements || form.formState.isSubmitting}
        >
          Place my order
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
    data: order,
    isLoading: isLoadingOrder,
    isError: isOrderError,
  } = useActiveOrder(restaurantId, {
    enabled: !isLoadingRouter,
  });

  const {
    data: restaurant,
    isLoading: isLoadingRestaurant,
    isError: isRestaurantError,
  } = useRestaurant(restaurantId, {
    enabled: !isLoadingRouter,
  });

  const isLoading = isLoadingOrder || isLoadingRestaurant || isLoadingRouter;
  const isError = isOrderError || isRestaurantError;

  if (isLoading) {
    return (
      <div className="flex flex-col justify-center items-center">
        <SpinnerIcon className="w-6 h-6 animate-spin" />
        <p className="mt-2 text-gray-700">Loading payment page.</p>
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

  if (!isLoading && !isError && !order?.items.length) {
    router.push(`/restaurants/${restaurantId}`);
    return null;
  }

  const subtotal = order.items.reduce(
    (total, item) => (total += item.quantity * item.menuItemPrice),
    0
  );

  const serviceCharge = 0.5;

  const total = subtotal + restaurant.deliveryFee + serviceCharge;

  return (
    <div className="md:flex items-start">
      <Elements stripe={stripePromise}>
        <CheckoutForm order={order} restaurant={restaurant} />
      </Elements>

      <div className="mt-4 md:mt-0 md:ml-2 flex-1 rounded bg-white border border-gray-300 p-2">
        <h2 className="font-bold text-xl text-gray-700">Your order</h2>

        <ul className="mt-2">
          {order.items.map((item) => {
            return (
              <li
                key={item.menuItemId}
                className="flex items-center justify-between text-sm"
              >
                <span>
                  {item.quantity} x {item.menuItemName}
                </span>
                <span>{(item.quantity * item.menuItemPrice).toFixed(2)}</span>
              </li>
            );
          })}
        </ul>

        <hr className="mt-4 -mx-2 border-gray-300" />

        <p className="mt-2 font-bold text-gray-700 flex items-center justify-between">
          <span>Subtotal</span>
          <span>{subtotal.toFixed(2)}</span>
        </p>

        <p className="mt-3 flex items-center justify-between text-sm">
          <span>Delivery Fee</span>
          <span>{restaurant.deliveryFee.toFixed(2)}</span>
        </p>

        <p className="mt-1 flex items-center justify-between text-sm">
          <span>Service Charge</span>
          <span>{serviceCharge.toFixed(2)}</span>
        </p>

        <p className="mt-3 font-bold text-xl text-gray-700 flex items-center justify-between">
          <span>Total</span>
          <span>£ {total.toFixed(2)}</span>
        </p>

        <hr className="mt-4 -mx-2 border-gray-300" />

        <p className="mt-3 text-sm text-center text-gray-700">
          {restaurant.address}
        </p>

        <hr className="mt-3 -mx-2 border-gray-300" />

        <p className="mt-3 mb-1 text-sm font-semibold flex items-center">
          <TruckIcon className="h-5" />
          <span className="ml-2">Delivery ASAP</span>
        </p>
      </div>
    </div>
  );
};

const CheckoutPage: FC = () => {
  return (
    <AuthLayout title="Checkout">
      <div className="container max-w-3xl mt-4">
        <Checkout />
      </div>
    </AuthLayout>
  );
};

export default CheckoutPage;
