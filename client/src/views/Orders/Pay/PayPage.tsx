import {
  CardElement,
  Elements,
  useElements,
  useStripe,
} from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, FormEvent } from "react";
import useOrder, { OrderDto } from "~/api/orders/useOrder";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import TruckIcon from "~/components/Icons/TruckIcon";
import { AuthLayout } from "~/components/Layout/Layout";
import { STRIPE_PUBLISHABLE_KEY } from "~/config";
import { formatAddress } from "~/services/utils";
import "./PayPage.module.css";
import usePay from "./usePay";

const stripePromise =
  typeof window === "undefined"
    ? undefined
    : loadStripe(STRIPE_PUBLISHABLE_KEY);

const PaymentForm: FC<{ order: OrderDto }> = ({ order }) => {
  const stripe = useStripe();
  const elements = useElements();

  const router = useRouter();

  const { mutate: pay, isError, error, isLoading } = usePay();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();

    if (!stripe || !elements) {
      return;
    }

    pay(order.paymentIntentClientSecret, {
      onSuccess: async () => {
        await router.push(`/orders/${order.id}`);
      },
    });
  };

  return (
    <div className="flex-1">
      <div className="rounded bg-white border border-gray-300 p-2">
        <h2 className="font-bold text-xl text-gray-700">Payment details</h2>

        {isError && <p className="text-primary mt-1 mb-4">{error.message}</p>}

        <form onSubmit={handleSubmit} className="mt-2">
          <div>
            <label htmlFor="card" className="label">
              Card details <span className="text-primary">*</span>
            </label>
            <div className="input px-2">
              <CardElement id="card" />
            </div>
          </div>

          <button
            className="btn btn-primary w-full mt-4"
            disabled={!stripe || !elements || isLoading}
          >
            Place my order
          </button>
        </form>
      </div>

      <div className="rounded bg-white border border-gray-300 p-2 mt-4 text-sm text-gray-700">
        <h2 className="font-bold text-lg text-gray-700">Demo card details</h2>

        <div className="mt-2">
          <div className="mt-1">
            <span className="font-semibold text-gray-800">Number: </span>
            <span>4242 4242 4242 4242</span>
          </div>

          <div className="mt-1">
            <span className="font-semibold text-gray-800">Expiry date: </span>
            <span>Any valid expiry date (04/24)</span>
          </div>

          <div className="mt-1">
            <span className="font-semibold text-gray-800">CVC: </span>
            <span>Any valid CVC (242)</span>
          </div>

          <div className="mt-1">
            <span className="font-semibold text-gray-800">ZIP: </span>
            <span>Any valid ZIP (42424)</span>
          </div>
        </div>
      </div>
    </div>
  );
};

const Pay: FC = () => {
  const router = useRouter();
  const orderId = router.query.id?.toString();
  const isLoadingRouter = !orderId;

  const {
    data: order,
    isLoading: isLoadingOrder,
    isError: isOrderError,
  } = useOrder(orderId, {
    enabled: !isLoadingRouter,
  });

  const isLoading = isLoadingOrder || isLoadingRouter;
  const isError = isOrderError;

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
          <Link href={`/restaurants/${orderId}`}>
            <a className="text-primary cursor-pointer">restaurant</a>
          </Link>{" "}
          and try again.
        </p>
      </div>
    );
  }

  if (!isLoading && !isError && !order?.items.length) {
    router.push(`/restaurants/${orderId}`);
    return null;
  }

  if (!isLoading && !isError && order.status !== "Placed") {
    router.push(`/orders/${orderId}`);
    return null;
  }

  const total = order.subtotal + order.deliveryFee + order.serviceFee;

  return (
    <div className="md:flex items-start">
      <Elements stripe={stripePromise}>
        <PaymentForm order={order} />
      </Elements>

      <div className="mt-4 md:mt-0 md:ml-2 flex-1 rounded bg-white border border-gray-300 p-2">
        <h2 className="font-bold text-xl text-gray-700">Your order</h2>

        <ul className="mt-2">
          {order.items.map((item) => {
            return (
              <li
                key={item.id}
                className="flex items-center justify-between text-sm"
              >
                <span>
                  {item.quantity} x {item.name}
                </span>
                <span>{(item.quantity * item.price).toFixed(2)}</span>
              </li>
            );
          })}
        </ul>

        <hr className="mt-4 -mx-2 border-gray-300" />

        <p className="mt-2 font-bold text-gray-700 flex items-center justify-between">
          <span>Subtotal</span>
          <span>{order.subtotal.toFixed(2)}</span>
        </p>

        <p className="mt-3 flex items-center justify-between text-sm">
          <span>Delivery Fee</span>
          <span>{order.deliveryFee.toFixed(2)}</span>
        </p>

        <p className="mt-1 flex items-center justify-between text-sm">
          <span>Service Charge</span>
          <span>{order.serviceFee.toFixed(2)}</span>
        </p>

        <p className="mt-3 font-bold text-xl text-gray-700 flex items-center justify-between">
          <span>Total</span>
          <span>£ {total.toFixed(2)}</span>
        </p>

        <hr className="mt-4 -mx-2 border-gray-300" />

        <p className="mt-3 text-sm text-center text-gray-700">
          {formatAddress(
            order.restaurantAddressLine1,
            order.restaurantAddressLine2,
            order.restaurantCity,
            order.restaurantPostcode
          )}
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

const PayPage: FC = () => {
  return (
    <AuthLayout title="Pay">
      <div className="container max-w-3xl mt-4">
        <Pay />
      </div>
    </AuthLayout>
  );
};

export default PayPage;
