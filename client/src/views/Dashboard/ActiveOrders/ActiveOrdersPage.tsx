import React, { FC, useMemo, useState } from "react";
import { OrderDto, OrderStatus } from "~/api/orders/OrderDto";
import useActiveRestaurantOrders from "~/api/orders/useActiveRestaurantOrders";
import { useOrdersHub } from "~/api/orders/useOrdersHub";
import CheckIcon from "~/components/Icons/CheckIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import InfoIcon from "~/components/Icons/InfoIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { formatDate } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";
import styles from "./ActiveOrdersPage.module.css";

const statusBadgeTexts: Map<OrderStatus, string> = new Map([
  ["PaymentConfirmed", "Confirmed"],
]);

const OrderTableRow: FC<{ order: OrderDto }> = ({ order }) => {
  const formattedTime = useMemo(
    () => formatDate(new Date(order.placedAt), "hh:mm:ss"),
    [order.placedAt]
  );

  const received = order.subtotal + order.deliveryFee;

  return (
    <tr
      className={`border-b border-gray-200 hover:bg-gray-100 ${styles["order-item-row"]}`}
    >
      <td className="py-3 px-3 text-left whitespace-nowrap">{order.number}</td>
      <td className="py-3 px-3 text-left whitespace-nowrap">{formattedTime}</td>
      <td className="py-3 px-3 text-left whitespace-nowrap">{order.address}</td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        £{received.toFixed(2)}
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <span className="bg-yellow-200 text-yellow-700 py-1 px-3 rounded-full text-xs font-semibold">
          {statusBadgeTexts.get(order.status)}
        </span>
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <div className="flex items-center justify-center">
          <button
            className="mr-2 text-green-500 hover:text-green-800 transition-colors transform hover:scale-110"
            title="Accept order"
          >
            <CheckIcon className="h-5" />
          </button>
          <button
            className="mr-2 text-red-500 hover:text-red-800 transition-colors transform hover:scale-110"
            title="Reject order"
          >
            <CloseIcon className="h-5" />
          </button>
        </div>
      </td>
    </tr>
  );
};

const OrdersTable: FC<{ orders: OrderDto[] }> = ({ orders }) => {
  return (
    <div className="overflow-x-auto">
      <table className="w-full table-auto">
        <thead>
          <tr className="bg-primary-700 text-gray-100 uppercase text-sm leading-normal">
            <th className="py-3 px-3 text-left">#</th>
            <th className="py-3 px-3 text-left">Time</th>
            <th className="py-3 px-3 text-left">Address</th>
            <th className="py-3 px-3 text-left">Total</th>
            <th className="py-3 px-3 text-left">Status</th>
            <th className="py-3 px-3 text-left">Actions</th>
          </tr>
        </thead>

        <tbody className="text-gray-700 text-sm">
          {orders.map((order) => {
            return <OrderTableRow key={order.id} order={order} />;
          })}
        </tbody>
      </table>
    </div>
  );
};

const ActiveOrdersPage: FC = () => {
  const [ordersMap, setOrdersMap] = useState<{ [id: string]: OrderDto }>({});

  const sortedOrders = useMemo(() => {
    return Object.values(ordersMap).sort((a, b) =>
      new Date(a.confirmedAt) > new Date(b.confirmedAt) ? -1 : 1
    );
  }, [ordersMap]);

  const ordersSortedByConfirmedAt = useMemo(() => {
    return Object.values(ordersMap).sort((a, b) =>
      new Date(a.confirmedAt) > new Date(b.confirmedAt) ? -1 : 1
    );
  }, [ordersMap]);

  const latestOrderConfirmed = ordersSortedByConfirmedAt[0]?.confirmedAt;

  const [hasNewOrders, setHasNewOrders] = useState(false);

  const {
    isLoading: isLoadingOrders,
    isFetching: isFetchingOrders,
    isError: isOrdersError,
    refetch,
  } = useActiveRestaurantOrders(
    {
      confirmedAfter: latestOrderConfirmed,
    },
    {
      onSuccess: (newOrders) => {
        const orders = { ...ordersMap };

        for (const order of newOrders) {
          orders[order.id] = order;
        }

        setOrdersMap(orders);
        setHasNewOrders(false);
      },

      enabled: false,
    }
  );

  const loadNewOrders = () => refetch();

  const {
    isLoading: isLoadingHub,
    isConnectionError: isHubConnectionError,
  } = useOrdersHub({
    configure: (connection) => {
      connection.on("new-order", () => setHasNewOrders(true));
    },

    onConnect: async () => {
      await refetch();
    },
  });

  const isLoading = isLoadingOrders || isLoadingHub;
  const isError = isOrdersError || isHubConnectionError;

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="font-semibold text-2xl">Active Orders</h1>

        {hasNewOrders && (
          <div className="flex items-center text-primary">
            <InfoIcon className="h-4 w-4" />

            <button
              className={`ml-2 font-semibold ${
                isFetchingOrders ? "opacity-50" : " animate-pulse"
              }`}
              onClick={loadNewOrders}
              disabled={isFetchingOrders}
            >
              Load new orders
            </button>
          </div>
        )}
      </div>

      {isLoading && (
        <div className="mt-4">
          <SpinnerIcon className="w-6 h-6 animate-spin" />
        </div>
      )}

      {!isLoading && isError && (
        <div className="mt-4">There was an error loading your orders.</div>
      )}

      {!isLoading && !isError && (
        <div className="mt-4">
          {sortedOrders.length ? (
            <OrdersTable orders={sortedOrders} />
          ) : (
            <p>No orders yet.</p>
          )}
        </div>
      )}
    </div>
  );
};

const OrderPageLayout: FC = () => {
  return (
    <DashboardLayout>
      <ActiveOrdersPage />
    </DashboardLayout>
  );
};

export default OrderPageLayout;
