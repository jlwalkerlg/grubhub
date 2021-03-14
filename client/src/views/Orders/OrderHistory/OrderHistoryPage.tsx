import Head from "next/head";
import Image from "next/image";
import Link from "next/link";
import React, { FC, useMemo } from "react";
import { OrderModel, useOrderHistory } from "~/api/orders/useOrderHistory";
import { AuthLayout } from "~/components/Layout/Layout";
import { formatDate } from "~/services/utils";

const OrderListItem: FC<{ order: OrderModel; index: number }> = ({
  order,
  index,
}) => {
  const placedAt = useMemo(() => {
    const date = new Date(order.placedAt);
    return formatDate(date, "dd/mm/yyyy");
  }, []);

  const total = order.subtotal + order.deliveryFee + order.serviceFee;

  return (
    <li className={`py-4 border-gray-300 ${index === 0 ? "" : "border-t"}`}>
      <Link href={`/orders/${order.id}`}>
        <a className="flex">
          <div>
            <Image
              src="https://d30v2pzvrfyzpo.cloudfront.net/uk/images/restaurants/125643.gif"
              alt={order.restaurantName}
              width={55}
              height={55}
            />
          </div>

          <div className="flex-1 ml-4">
            <p className="font-semibold">{order.restaurantName}</p>
            <p className="text-sm">{placedAt}</p>
            <p className="text-sm">
              <span>£{total.toFixed(2)}</span>{" "}
              {order.totalItems === 1 ? (
                <span>({order.totalItems} item)</span>
              ) : (
                <span>({order.totalItems} items)</span>
              )}
            </p>
          </div>
        </a>
      </Link>
    </li>
  );
};

const OrderHistoryList: FC<{ orders: OrderModel[] }> = ({ orders }) => {
  if (orders.length === 0) {
    return <p>You haven't ordered anything!</p>;
  }

  return (
    <ul className="rounded border border-gray-300 px-4">
      {orders.map((order, index) => {
        return <OrderListItem key={order.id} order={order} index={index} />;
      })}
    </ul>
  );
};

const OrderHistory: FC = () => {
  const {
    data,
    isFetchingMore,
    fetchMore,
    canFetchMore,
    isError,
    isLoading,
  } = useOrderHistory({ perPage: 15 });

  const loadMoreOrders = () => fetchMore();

  const orders = data?.map((x) => x.orders).flat() ?? [];

  return (
    <div className="container max-w-3xl bg-white rounded p-4">
      <h1 className="text-4xl font-semibold text-gray-800">Previous orders</h1>

      {isLoading && <p>Loading your orders...</p>}

      {!isLoading && isError && (
        <p>Something went wrong loading your orders.</p>
      )}

      {!isLoading && !isError && (
        <div className="mt-4">
          <OrderHistoryList orders={orders} />

          {canFetchMore && (
            <button
              className="btn btn-primary mt-2 normal-case w-full"
              onClick={loadMoreOrders}
              disabled={isFetchingMore !== false}
            >
              View more
            </button>
          )}
        </div>
      )}
    </div>
  );
};

const OrderHistoryPage: FC = () => {
  return (
    <AuthLayout title="Your Orders | FoodSnap">
      <Head>
        <style
          dangerouslySetInnerHTML={{
            __html: `
            body {
              background-color: white !important;
            }
          `,
          }}
        ></style>
      </Head>

      <OrderHistory />
    </AuthLayout>
  );
};

export default OrderHistoryPage;
