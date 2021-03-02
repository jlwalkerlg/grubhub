import React, { FC, useMemo, useState } from "react";
import { useAcceptOrder } from "~/api/orders/useAcceptOrder";
import useActiveRestaurantOrders, {
  ActiveOrderDto,
} from "~/api/orders/useActiveRestaurantOrders";
import { OrderStatus } from "~/api/orders/useOrder";
import { useOrdersHub } from "~/api/orders/useOrdersHub";
import CheckIcon from "~/components/Icons/CheckIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import InfoIcon from "~/components/Icons/InfoIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import LoadingIconWrapper from "~/components/LoadingIconWrapper";
import { useToasts } from "~/components/Toaster/Toaster";
import { formatDate } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";
import styles from "./ActiveOrdersPage.module.css";
import OrderDetailsModal from "./OrderDetailsModal";

const statusBadgeTexts: Map<OrderStatus, string> = new Map([
  ["PaymentConfirmed", "Confirmed"],
  ["Accepted", "Accepted"],
]);

const StatusBadge: FC<{ order: ActiveOrderDto }> = ({ order }) => {
  return (
    <span className="bg-yellow-200 text-yellow-700 py-1 px-3 rounded-full text-xs font-semibold">
      {statusBadgeTexts.get(order.status)}
    </span>
  );
};

const OrderTableRow: FC<{ order: ActiveOrderDto }> = ({ order }) => {
  const estimatedDeliveryTime = useMemo(
    () => formatDate(new Date(order.estimatedDeliveryTime), "hh:mm"),
    [order.estimatedDeliveryTime]
  );

  const { addToast } = useToasts();

  const [accept, { isLoading: isAccepting }] = useAcceptOrder();

  const onAccept = async () => {
    if (isAccepting) return;

    await accept(
      { orderId: order.id },
      {
        onError: (error) => {
          addToast("Failed to accept order: " + error.message);
        },
      }
    );
  };

  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

  const openDetailsModal = () => setIsDetailsModalOpen(true);
  const closeDetailsModal = () => setIsDetailsModalOpen(false);

  return (
    <tr
      className={`border-b border-gray-200 hover:bg-gray-100 ${styles["order-item-row"]}`}
    >
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
      <td className="py-3 px-3 text-left whitespace-nowrap">
        {estimatedDeliveryTime}
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">{order.address}</td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        £{order.subtotal.toFixed(2)}
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <StatusBadge order={order} />
      </td>
      <td className="py-3 px-3 text-left whitespace-nowrap">
        <div className="flex items-center">
          {order.status !== "Accepted" && (
            <button
              className="mr-1 text-green-500 hover:text-green-800 transition-colors transform hover:scale-110"
              title="Accept order"
              onClick={onAccept}
              disabled={isAccepting}
            >
              <LoadingIconWrapper isLoading={isAccepting} className="h-5">
                <CheckIcon className="h-5" />
              </LoadingIconWrapper>
            </button>
          )}
          <button
            className="mr-1 text-red-500 hover:text-red-800 transition-colors transform hover:scale-110"
            title="Reject order"
          >
            <CloseIcon className="h-5" />
          </button>
        </div>
      </td>
    </tr>
  );
};

const OrdersTable: FC<{ orders: ActiveOrderDto[] }> = ({ orders }) => {
  return (
    <div className="overflow-x-auto">
      <table className="w-full table-auto">
        <thead>
          <tr className="bg-primary-700 text-gray-100 uppercase text-sm leading-normal">
            <th className="py-3 px-3 text-left">#</th>
            <th className="py-3 px-3 text-left">Deliver At</th>
            <th className="py-3 px-3 text-left">Address</th>
            <th className="py-3 px-3 text-left">Subtotal</th>
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
  const [ordersMap, setOrdersMap] = useState<{ [id: string]: ActiveOrderDto }>(
    {}
  );

  const sortedOrders = useMemo(() => {
    return Object.values(ordersMap).sort((a, b) =>
      new Date(a.placedAt) > new Date(b.placedAt) ? -1 : 1
    );
  }, [ordersMap]);

  const ordersSortedByPlacedAt = useMemo(() => {
    return Object.values(ordersMap).sort((a, b) =>
      new Date(a.placedAt) > new Date(b.placedAt) ? -1 : 1
    );
  }, [ordersMap]);

  const latestOrderConfirmed = ordersSortedByPlacedAt[0]?.placedAt;

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

      connection.on("order-accepted", (orderId) => {
        console.log({ accepted: orderId });
      });
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
