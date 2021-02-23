import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, useMemo } from "react";
import { useQueryCache } from "react-query";
import { OrderDto, OrderStatus } from "~/api/orders/OrderDto";
import useOrder, { getOrderQueryKey } from "~/api/orders/useOrder";
import useAuth from "~/api/users/useAuth";
import { UserDto } from "~/api/users/UserDto";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import ClipboardIcon from "~/components/Icons/ClipboardIcon";
import LocationMarkerIcon from "~/components/Icons/LocationMarkerIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { AuthLayout } from "~/components/Layout/Layout";
import { formatDate } from "~/services/utils";
import { useOrdersHub } from "../../../api/orders/useOrdersHub";

const statusTexts: Map<OrderStatus, string> = new Map([
  ["Placed", "Processing"],
  ["PaymentConfirmed", "Awaiting confirmation"],
]);

const statusDescriptions: Map<OrderStatus, string> = new Map([
  [
    "Placed",
    "Your order has been received and we are currently processing your payment details.",
  ],
  [
    "PaymentConfirmed",
    "We're currently awaiting confirmation from the restaurant.",
  ],
]);

const OrderHeader: FC<{ order: OrderDto }> = ({ order }) => {
  const formattedPlacedAt = useMemo(() => {
    var date = new Date(order.placedAt);
    return formatDate(date);
  }, [order.placedAt]);

  order.status = "PaymentConfirmed";

  const statusText = statusTexts.get(order.status);
  const statusDescription = statusDescriptions.get(order.status);

  return (
    <div className="bg-primary-600 px-4 py-4">
      <Link href="/orders">
        <a className="mb-4 inline-flex items-center rounded-full bg-primary-800 text-white px-3 py-1">
          <ChevronIcon direction="left" className="h-4" />
          <span className="ml-1 text-sm font-semibold hover:underline">
            Your orders
          </span>
        </a>
      </Link>

      <div className="bg-white rounded p-4 md:p-8 max-w-2xl mx-auto">
        <div className="flex items-center justify-between">
          <p className="font-semibold text-2xl md:text-4xl">{statusText}</p>
          <p className="font-semibold md:text-xl ml-3">{formattedPlacedAt}</p>
        </div>

        <hr className="my-4 md:my-6 border-gray-300" />

        <div className="text-sm md:text-base">
          <p>{statusDescription}</p>
        </div>
      </div>
    </div>
  );
};

const MobileOrderSummary: FC<{ order: OrderDto; user: UserDto }> = ({
  order,
  user,
}) => {
  const total = order.subtotal + order.deliveryFee + order.serviceFee;

  return (
    <>
      <div className="flex flex-col items-center mt-6">
        <Link href={`/restaurants/${order.restaurantId}`}>
          <a
            className="text-primary text-sm block rounded-sm overflow-hidden"
            title={`View the menu for ${order.restaurantName}`}
          >
            <Image
              src="https://d30v2pzvrfyzpo.cloudfront.net/uk/images/restaurants/125643.gif"
              alt={order.restaurantName}
              width={55}
              height={55}
            />
          </a>
        </Link>

        <p className="font-semibold text-2xl mt-2">{order.restaurantName}</p>

        <p className="text-sm text-gray-700">{order.restaurantAddress}</p>

        <p>
          <Link href={`/restaurants/${order.restaurantId}`}>
            <a
              className="text-primary text-sm"
              title={`View the menu for ${order.restaurantName}`}
            >
              View menu
            </a>
          </Link>
        </p>
      </div>

      <hr className="my-6 border-gray-300" />

      <div className="flex items-start px-4">
        <div className="pr-4">
          <LocationMarkerIcon className="w-5" />
        </div>

        <div>
          <h2 className="font-semibold">Delivering to</h2>

          <p className="font-semibold mt-3">{user.name}</p>
          <p>{order.address}</p>
        </div>
      </div>

      <hr className="my-6 border-gray-300" />

      <div className="px-4">
        <div className="flex items-start">
          <div className="pr-4">
            <ClipboardIcon className="w-5" />
          </div>

          <div>
            <h2 className="font-semibold">Order summary</h2>
          </div>
        </div>

        <ul className="mt-3 text-sm">
          {order.items.map((item) => {
            const total = item.quantity * item.menuItemPrice;

            return (
              <li
                key={item.menuItemId}
                className="flex items-center justify-between font-semibold mt-3"
              >
                <p>
                  {+item.quantity > 1 && <span>{+item.quantity}x </span>}
                  <span>{item.menuItemName}</span>
                </p>

                <p>{total.toFixed(2)}</p>
              </li>
            );
          })}
        </ul>
      </div>

      <hr className="my-6 border-gray-300" />

      <div className="px-4">
        <p className="flex items-center justify-between">
          <span>Subtotal</span>
          <span>{order.subtotal.toFixed(2)}</span>
        </p>

        <p className="flex items-center justify-between mt-2">
          <span>Delivery fee</span>
          <span>{order.deliveryFee.toFixed(2)}</span>
        </p>

        <p className="flex items-center justify-between">
          <span>Service fee</span>
          <span>{order.serviceFee.toFixed(2)}</span>
        </p>
      </div>

      <hr className="my-6 border-gray-300" />

      <div className="px-4">
        <p className="flex items-center justify-between font-bold">
          <span>Total</span>
          <span className="text-3xl">£{total.toFixed(2)}</span>
        </p>
      </div>
    </>
  );
};

const OrderSummary: FC<{ order: OrderDto; user: UserDto }> = ({
  order,
  user,
}) => {
  const total = order.subtotal + order.deliveryFee + order.serviceFee;

  return (
    <div className="mt-4 container">
      <div className="max-w-5xl mx-auto flex items-start">
        <div className="flex-1 bg-white rounded-lg border border-gray-300 p-4">
          <div className="flex items-start">
            <div>
              <Link href={`/restaurants/${order.restaurantId}`}>
                <a
                  className="text-primary text-sm block rounded-sm overflow-hidden"
                  title={`View the menu for ${order.restaurantName}`}
                >
                  <Image
                    src="https://d30v2pzvrfyzpo.cloudfront.net/uk/images/restaurants/125643.gif"
                    alt={order.restaurantName}
                    width={55}
                    height={55}
                  />
                </a>
              </Link>
            </div>

            <div className="ml-4">
              <p className="font-semibold text-xl">{order.restaurantName}</p>

              <p className="text-gray-700">{order.restaurantAddress}</p>

              <p className="text-gray-700">{order.restaurantPhoneNumber}</p>

              <p>
                <Link href={`/restaurants/${order.restaurantId}`}>
                  <a
                    className="text-primary text-sm"
                    title={`View the menu for ${order.restaurantName}`}
                  >
                    View menu
                  </a>
                </Link>
              </p>
            </div>
          </div>

          <hr className="my-4 border-gray-300" />

          <div>
            <h2 className="font-semibold text-xl">Order summary</h2>

            <ul className="mt-3 text-sm">
              {order.items.map((item) => {
                const total = item.quantity * item.menuItemPrice;

                return (
                  <li
                    key={item.menuItemId}
                    className="flex items-center justify-between font-semibold mt-3"
                  >
                    <p>
                      {+item.quantity > 1 && <span>{+item.quantity}x </span>}
                      <span>{item.menuItemName}</span>
                    </p>

                    <p>{total.toFixed(2)}</p>
                  </li>
                );
              })}
            </ul>
          </div>

          <hr className="my-4 border-gray-300" />

          <div>
            <p className="flex items-center justify-between">
              <span>Subtotal</span>
              <span>{order.subtotal.toFixed(2)}</span>
            </p>

            <p className="flex items-center justify-between mt-2">
              <span>Delivery fee</span>
              <span>{order.deliveryFee.toFixed(2)}</span>
            </p>

            <p className="flex items-center justify-between">
              <span>Service fee</span>
              <span>{order.serviceFee.toFixed(2)}</span>
            </p>
          </div>

          <hr className="my-4 border-gray-300" />

          <div>
            <p className="flex items-center justify-between font-bold text-gray-700">
              <span className="text-2xl">Total</span>
              <span className="text-3xl">£{total.toFixed(2)}</span>
            </p>
          </div>
        </div>

        <div className="flex-1 bg-white rounded border border-gray-300 p-4 ml-2">
          <div className="flex items-start px-4">
            <div className="pr-4">
              <LocationMarkerIcon className="w-5" />
            </div>

            <div>
              <h2 className="font-semibold">Delivering to</h2>

              <p className="font-semibold mt-3">{user.name}</p>
              <p>{order.address}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

const OrderDetails: FC<{ order: OrderDto }> = ({ order }) => {
  const cache = useQueryCache();

  useOrdersHub({
    configure: (connection) => {
      connection.on(`order_${order.id}.updated`, () => {
        cache.invalidateQueries(getOrderQueryKey(order.id));
      });
    },

    onConnect: () => {
      cache.invalidateQueries(getOrderQueryKey(order.id));
    },
  });

  const { user } = useAuth();

  return (
    <div>
      <OrderHeader order={order} />

      <div className="md:hidden">
        <MobileOrderSummary order={order} user={user} />
      </div>

      <div className="hidden md:block">
        <OrderSummary order={order} user={user} />
      </div>
    </div>
  );
};

const OrderDetailsPage: FC = () => {
  const router = useRouter();
  const orderId = router.query.id?.toString();

  const isRouterLoading = !orderId;

  const { data: order, isLoading: isOrderLoading, isError } = useOrder(
    orderId,
    { enabled: !isRouterLoading }
  );

  const isLoading = isRouterLoading || isOrderLoading;

  if (isLoading) {
    return (
      <AuthLayout title="Your Order">
        <div className="mt-4 flex flex-col justify-center items-center">
          <SpinnerIcon className="w-6 h-6 animate-spin" />
          <p className="mt-2 text-gray-700">Loading your order details.</p>
        </div>
      </AuthLayout>
    );
  }

  if (isError) {
    return (
      <AuthLayout title="Your Order">
        <div className="mt-4 flex flex-col justify-center items-center">
          <p>There was an error loading your order.</p>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout title="Your Order">
      <OrderDetails order={order} />
    </AuthLayout>
  );
};

export default OrderDetailsPage;
