import React, { FC, useMemo, useState } from "react";
import { useAcceptOrder } from "~/api/orders/useAcceptOrder";
import useActiveRestaurantOrders, {
  ActiveOrderDto,
} from "~/api/orders/useActiveRestaurantOrders";
import { useDeliverOrder } from "~/api/orders/useDeliverOrder";
import { useOrdersHub } from "~/api/orders/useOrdersHub";
import { useRejectOrder } from "~/api/orders/useRejectOrder";
import CheckIcon from "~/components/Icons/CheckIcon";
import InfoIcon from "~/components/Icons/InfoIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import LoadingIconWrapper from "~/components/LoadingIconWrapper";
import { useToasts } from "~/components/Toaster/Toaster";
import { formatDate } from "~/services/utils";
import { DashboardLayout } from "../DashboardLayout";
import styles from "./ActiveOrdersPage.module.css";
import OrderDetailsModal from "./OrderDetailsModal";

const StatusBadge: FC<{ order: ActiveOrderDto }> = ({ order }) => {
  if (order.status === "PaymentConfirmed") {
    return (
      <span className="bg-yellow-200 text-yellow-900 py-1 px-3 rounded-full text-xs font-semibold">
        Confirmed
      </span>
    );
  }

  return (
    <span className="bg-green-200 text-green-900 py-1 px-3 rounded-full text-xs font-semibold">
      Accepted
    </span>
  );
};

const OrderTableRow: FC<{
  order: ActiveOrderDto;
  onOrderStatusChanged: (id: string) => any;
}> = ({ order, onOrderStatusChanged }) => {
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
        onSuccess: async () => {
          await onOrderStatusChanged(order.id);
        },

        onError: (error) => {
          addToast(error.detail);
        },
      }
    );
  };

  const [reject, { isLoading: isRejecting }] = useRejectOrder();

  const onReject = async () => {
    if (isRejecting) return;

    await reject(
      { orderId: order.id },
      {
        onSuccess: async () => {
          await onOrderStatusChanged(order.id);
        },

        onError: (error) => {
          addToast(error.detail);
        },
      }
    );
  };

  const [deliver, { isLoading: isDelivering }] = useDeliverOrder();

  const onDeliver = async () => {
    if (isAccepting) return;

    await deliver(
      { orderId: order.id },
      {
        onSuccess: async () => {
          await onOrderStatusChanged(order.id);
        },

        onError: (error) => {
          addToast(error.detail);
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
          {order.status === "PaymentConfirmed" && (
            <>
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

              <button
                className="mr-1 text-red-500 hover:text-red-800 transition-colors transform hover:scale-110"
                title="Reject order"
                onClick={onReject}
                disabled={isRejecting}
              >
                <LoadingIconWrapper isLoading={isRejecting} className="h-5">
                  <CheckIcon className="h-5" />
                </LoadingIconWrapper>
              </button>
            </>
          )}
          {order.status === "Accepted" && (
            <button
              className="mr-1 text-green-500 hover:text-green-800 transition-colors transform hover:scale-110"
              title="Deliver order"
              onClick={onDeliver}
              disabled={isDelivering}
            >
              <LoadingIconWrapper isLoading={isDelivering} className="h-5">
                <CheckIcon className="h-5" />
              </LoadingIconWrapper>
            </button>
          )}
        </div>
      </td>
    </tr>
  );
};

const OrdersTable: FC<{
  orders: ActiveOrderDto[];
  onOrderStatusChanged: (id: string) => any;
}> = ({ orders, onOrderStatusChanged }) => {
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
            return (
              <OrderTableRow
                key={order.id}
                order={order}
                onOrderStatusChanged={onOrderStatusChanged}
              />
            );
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

  const [hasNewOrders, setHasNewOrders] = useState(false);

  const onOrderStatusChanged = (orderId: string) => {
    refetch();
  };

  const {
    isLoading: isLoadingOrders,
    isFetching: isFetchingOrders,
    isError: isOrdersError,
    refetch,
  } = useActiveRestaurantOrders(
    {
      // confirmedAfter: sortedOrders[0]?.placedAt,
    },
    {
      onSuccess: (orders) => {
        const ordersMap: { [id: string]: ActiveOrderDto } = {};

        for (const order of orders) {
          ordersMap[order.id] = order;
        }

        setOrdersMap(ordersMap);
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
        refetch();
      });

      connection.on("order-rejected", (orderId) => {
        refetch();
      });

      connection.on("order-delivered", (orderId) => {
        refetch();
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
            <OrdersTable
              orders={sortedOrders}
              onOrderStatusChanged={onOrderStatusChanged}
            />
          ) : (
            <p>No orders yet.</p>
          )}
        </div>
      )}
    </div>
  );
};

const ActiveOrdersPageLayout: FC = () => {
  return (
    <DashboardLayout>
      <ActiveOrdersPage />
    </DashboardLayout>
  );
};

export default ActiveOrdersPageLayout;
