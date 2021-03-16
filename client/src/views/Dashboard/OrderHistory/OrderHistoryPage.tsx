import { useRouter } from "next/router";
import React, { FC, useMemo, useState } from "react";
import useRestaurantOrderHistory, {
  OrderModel,
} from "~/api/orders/useRestaurantOrderHistory";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import useIsRouterReady from "~/services/useIsRouterReady";
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

  if (order.status === "Cancelled") {
    return (
      <span className="bg-red-200 text-red-900 py-1 px-3 rounded-full text-xs font-semibold">
        Cancelled
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
  const router = useRouter();

  const perPage = 15;
  const [currentPage, setCurrentPage] = useState(1);
  // TODO: causes errors on reload
  // const [currentPage, setCurrentPage] = useState(() => +router.query.page || 1);
  const [totalPages, setTotalPages] = useState(1);
  const [orders, setOrders] = useState<OrderModel[]>([]);

  // TODO: feels hacky, follow react-query guide instead
  const {
    isFetching,
    fetchPreviousPage,
    fetchNextPage,
    isError,
    isLoading,
  } = useRestaurantOrderHistory(
    { perPage },
    {
      onSuccess: (data) => {
        const { pages } = data;
        const { count, orders } = pages[currentPage - 1];
        setOrders(orders);
        setTotalPages(Math.ceil(count / perPage) || 1);
      },
      getPreviousPageParam: () => currentPage,
      getNextPageParam: () => currentPage,
    }
  );

  const loadNextPage = () => {
    const nextPage = currentPage + 1;
    fetchNextPage({ pageParam: nextPage });
    setCurrentPage(nextPage);
    // TODO: causes errors on reload
    // router.replace({
    //   query: { ...router.query, page: nextPage },
    // });
  };

  const loadPreviousPage = () => {
    const previousPage = currentPage - 1;
    fetchPreviousPage({ pageParam: previousPage });
    setCurrentPage(previousPage);
    // TODO: causes errors on reload
    // router.replace({
    //   query: { ...router.query, page: previousPage },
    // });
  };

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
            <>
              <OrdersTable orders={orders} />

              <div className="flex items-center justify-between">
                <p>
                  Page {currentPage} of {totalPages}
                </p>

                <nav className="mt-4 flex" aria-label="Pagination">
                  <button
                    disabled={isFetching || currentPage === 1}
                    onClick={loadPreviousPage}
                    className={`px-4 py-2 border text-sm font-medium rounded-sm ${
                      currentPage > 1
                        ? "text-red-700 border-red-700"
                        : "text-gray-700 border-gray-500"
                    }`}
                  >
                    Previous
                  </button>
                  <button
                    disabled={isFetching || currentPage === totalPages}
                    onClick={loadNextPage}
                    className={`px-4 py-2 border text-sm font-medium rounded-sm ml-2 ${
                      currentPage < totalPages
                        ? "text-red-700 border-red-700"
                        : "text-gray-700 border-gray-500"
                    }`}
                  >
                    Next
                  </button>
                </nav>
              </div>
            </>
          ) : (
            <p>No orders yet.</p>
          )}
        </div>
      )}
    </div>
  );
};

const OrderHistoryPageLayout: FC = () => {
  const isRouterReady = useIsRouterReady();

  return (
    <DashboardLayout>{isRouterReady && <OrderHistoryPage />}</DashboardLayout>
  );
};

export default OrderHistoryPageLayout;
