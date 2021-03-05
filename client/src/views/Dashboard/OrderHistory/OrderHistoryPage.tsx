import React, { FC, useMemo, useState } from "react";
import useRestaurantOrderHistory, {
  OrderModel,
} from "~/api/orders/useRestaurantOrderHistory";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import { formatDate } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";
import OrderDetailsModal from "./OrderDetailsModal";

const StatusBadge: FC<{ order: OrderModel }> = ({ order }) => {
  if (order.status === "Rejected") {
    return (
      <span className="bg-red-200 text-red-900 py-1 px-3 rounded-full text-xs font-semibold">
        Rejected
      </span>
    );
  }

  return (
    <span className="bg-green-200 text-green-900 py-1 px-3 rounded-full text-xs font-semibold">
      Delivered
    </span>
  );
};

const OrderTableRow: FC<{
  order: OrderModel;
}> = ({ order }) => {
  const placedAt = useMemo(() => {
    const date = new Date(order.placedAt);
    return formatDate(date, "dd/mm/yyyy hh:mm");
  }, [order.placedAt]);

  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  const openDetailsModal = () => setIsDetailsModalOpen(true);
  const closeDetailsModal = () => setIsDetailsModalOpen(false);

  return (
    <tr className="border-b border-gray-200 hover:bg-gray-100">
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <div>
          <button className="btn-link" onClick={openDetailsModal}>
            {order.number}
          </button>

          {isDetailsModalOpen && (
            <OrderDetailsModal order={order} onClose={closeDetailsModal} />
          )}
        </div>
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">{placedAt}</td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        £{order.subtotal.toFixed(2)}
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <StatusBadge order={order} />
      </td>
    </tr>
  );
};

const OrdersTable: FC<{
  orders: OrderModel[];
}> = ({ orders }) => {
  return (
    <div className="overflow-x-auto">
      <table className="w-full table-auto">
        <thead>
          <tr className="bg-primary-700 text-gray-100 uppercase text-sm leading-normal">
            <th className="py-3 px-3 text-left">#</th>
            <th className="py-3 px-3 text-left">Date/Time</th>
            <th className="py-3 px-3 text-left">Subtotal</th>
            <th className="py-3 px-3 text-left">Status</th>
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

const OrderHistoryPage: FC = () => {
  const { data: orders, isLoading, isError } = useRestaurantOrderHistory();

  return (
    <div>
      <h1 className="font-semibold text-2xl">Previous Orders</h1>

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

const OrderHistoryPageLayout: FC = () => {
  return (
    <DashboardLayout>
      <OrderHistoryPage />
    </DashboardLayout>
  );
};

export default OrderHistoryPageLayout;
