import React, { FC, useMemo } from "react";
import { OrderDto } from "~/api/orders/OrderDto";
import useRestaurantOrders from "~/api/orders/useRestaurantOrders";
import CheckIcon from "~/components/Icons/CheckIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { formatDate } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";

const OrderTableRow: FC<{ order: OrderDto }> = ({ order }) => {
  const formattedDate = useMemo(
    () => formatDate(new Date(order.placedAt), "dd/mm/yyyy"),
    [order.placedAt]
  );

  const formattedTime = useMemo(
    () => formatDate(new Date(order.placedAt), "hh:mm:ss"),
    [order.placedAt]
  );

  const received = order.subtotal + order.deliveryFee;

  return (
    <tr className="border-b border-gray-200 hover:bg-gray-100">
      <td className="py-3 px-6 text-left whitespace-nowrap">{order.number}</td>
      <td className="py-3 px-6 text-left whitespace-nowrap">{formattedDate}</td>
      <td className="py-3 px-6 text-left whitespace-nowrap">{formattedTime}</td>
      <td className="py-3 px-6 text-left whitespace-nowrap">{order.address}</td>
      <td className="py-3 px-6 text-left whitespace-nowrap">
        £{received.toFixed(2)}
      </td>
      <td className="py-3 px-6 text-left whitespace-nowrap">
        <span className="bg-yellow-200 text-yellow-700 py-1 px-3 rounded-full text-xs font-semibold">
          Confirmed
        </span>
      </td>
      <td className="py-3 px-6 text-left whitespace-nowrap">
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
            <th className="py-3 px-6 text-left">Number</th>
            <th className="py-3 px-6 text-left">Date</th>
            <th className="py-3 px-6 text-left">Time</th>
            <th className="py-3 px-6 text-left">Address</th>
            <th className="py-3 px-6 text-left">Total</th>
            <th className="py-3 px-6 text-left">Status</th>
            <th className="py-3 px-6 text-left">Actions</th>
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

const OrdersPage: FC = () => {
  const { data: orders, isLoading, isError } = useRestaurantOrders();

  return (
    <div>
      <h1 className="font-semibold text-2xl">Orders</h1>

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
          {orders.length ? (
            <OrdersTable orders={orders} />
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
      <OrdersPage />
    </DashboardLayout>
  );
};

export default OrderPageLayout;
