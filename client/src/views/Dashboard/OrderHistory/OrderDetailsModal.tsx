import React, { FC, MutableRefObject, useMemo, useRef } from "react";
import useOrder, { OrderDto } from "~/api/orders/useOrder";
import { OrderModel } from "~/api/orders/useRestaurantOrderHistory";
import CloseIcon from "~/components/Icons/CloseIcon";
import useEscapeKeyListener from "~/services/useEscapeKeyListener";
import useFocusTrap from "~/services/useFocusTrap";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import { formatAddress, formatDate } from "~/services/utils";

const StatusBadge: FC<{ order: OrderDto }> = ({ order }) => {
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

const OrderDetails: FC<{
  order: OrderDto;
  closeButtonRef: MutableRefObject<HTMLButtonElement>;
  onClose: () => any;
}> = ({ order, closeButtonRef, onClose }) => {
  const placedAt = useMemo(() => {
    const date = new Date(order.placedAt);
    return formatDate(date, "dd/mm/yyyy hh:mm");
  }, [order.placedAt]);

  const deliveredAt = useMemo(() => {
    if (order.status !== "Delivered") return null;

    const date = new Date(order.deliveredAt);
    return formatDate(date, "dd/mm/yyyy hh:mm");
  }, [order.status, order.deliveredAt]);

  const rejectedAt = useMemo(() => {
    if (order.status !== "Rejected") return null;

    const date = new Date(order.rejectedAt);
    return formatDate(date, "dd/mm/yyyy hh:mm");
  }, [order.status, order.rejectedAt]);

  const cancelledAt = useMemo(() => {
    if (order.status !== "Cancelled") return null;

    const date = new Date(order.cancelledAt);
    return formatDate(date, "dd/mm/yyyy hh:mm");
  }, [order.status, order.cancelledAt]);

  return (
    <div>
      <div className="flex items-center justify-between">
        <div className="flex items-center justify-start">
          <h2 className="font-semibold text-2xl">Order #{order.number}</h2>

          <div className="ml-4">
            <StatusBadge order={order} />
          </div>
        </div>

        <button
          className="p-1 text-primary"
          ref={closeButtonRef}
          onClick={onClose}
        >
          <CloseIcon className="h-6" />
        </button>
      </div>

      <hr className="border-gray-150 my-3" />

      <div className="mt-4 md:flex">
        <div className="flex-1">
          <p className="font-semibold text-xl">Delivery Details</p>

          <div className="mt-1">
            <p className="mt-2">
              <span className="font-semibold">Address</span>
              <span className="ml-2">
                {formatAddress(
                  order.addressLine1,
                  order.addressLine2,
                  order.city,
                  order.postcode
                )}
              </span>
            </p>
            <p className="mt-2">
              <span className="font-semibold">Placed At</span>
              <span className="ml-2">{placedAt}</span>
            </p>
            {deliveredAt && (
              <p className="mt-2">
                <span className="font-semibold">Delivered At</span>
                <span className="ml-2">{deliveredAt}</span>
              </p>
            )}
            {rejectedAt && (
              <p className="mt-2">
                <span className="font-semibold">Rejected At</span>
                <span className="ml-2">{rejectedAt}</span>
              </p>
            )}
            {cancelledAt && (
              <p className="mt-2">
                <span className="font-semibold">Cancelled At</span>
                <span className="ml-2">{cancelledAt}</span>
              </p>
            )}
          </div>
        </div>

        <div className="flex-1 mt-4 md:mt-0 md:ml-8">
          <p className="font-semibold text-xl">Customer Details</p>

          <div className="mt-1">
            <p className="mt-2">
              <span className="font-semibold">Name</span>
              <span className="ml-2">
                {order.customerFirstName} {order.customerLastName}
              </span>
            </p>
            <p className="mt-2">
              <span className="font-semibold">Phone Number</span>
              <span className="ml-2">{order.customerMobile}</span>
            </p>
            <p className="mt-2">
              <span className="font-semibold">Email</span>
              <span className="ml-2">{order.customerEmail}</span>
            </p>
          </div>
        </div>
      </div>

      <hr className="border-gray-150 my-3" />

      <div>
        <p className="font-semibold text-xl">Items</p>

        <div className="mt-1">
          <ul>
            {order.items.map((item) => {
              return (
                <li
                  key={item.id}
                  className="flex items-center justify-between mt-2"
                >
                  <p>
                    {item.quantity > 1 ? `${item.quantity}x ` : ""}
                    {item.name}
                  </p>

                  <p className="font-semibold text-lg">
                    {item.price.toFixed(2)}
                  </p>
                </li>
              );
            })}
          </ul>
        </div>
      </div>

      <hr className="border-gray-150 my-3" />

      <div className="py-2 flex items-center justify-between border-b border-gray-150">
        <div>Subtotal</div>
        <div className="font-semibold text-lg ml-4">
          {order.subtotal.toFixed(2)}
        </div>
      </div>
      <div className="py-2 flex items-center justify-between border-b border-gray-150">
        <div>Service fee</div>
        <div className="font-semibold text-lg ml-4">
          {order.serviceFee.toFixed(2)}
        </div>
      </div>
      <div className="py-2 flex items-center justify-between border-b border-gray-150">
        <div>Delivery fee</div>
        <div className="font-semibold text-lg ml-4">
          {order.deliveryFee.toFixed(2)}
        </div>
      </div>
      <div className="py-2 flex items-center justify-between">
        <div className="font-semibold text-xl">Total</div>
        <div className="font-semibold text-xl ml-4">
          £ {(order.subtotal + order.serviceFee + order.deliveryFee).toFixed(2)}
        </div>
      </div>
    </div>
  );
};

const OrderDetailsModal: FC<{ order: OrderModel; onClose: () => any }> = ({
  order,
  onClose,
}) => {
  useEscapeKeyListener(onClose);

  const closeButtonRef = useRef<HTMLButtonElement>();
  useFocusTrap(true, closeButtonRef);

  usePreventBodyScroll(true);

  const { data, isLoading, isError } = useOrder(order.id);

  return (
    <div className="fixed inset-y-0 left-0 w-full z-50 flex items-center md:px-8 md:py-4">
      <div
        className="fixed inset-0"
        style={{ backgroundColor: "rgba(0,0,0,0.7)" }}
        onClick={onClose}
      ></div>

      <div className="relative flex-1 max-w-3xl mx-auto h-full overflow-y-auto bg-white md:rounded-lg p-4">
        {isLoading && <p>Loading order...</p>}
        {!isLoading && isError && <p>Error loading the order...</p>}
        {!isLoading && !isError && (
          <OrderDetails
            order={data}
            closeButtonRef={closeButtonRef}
            onClose={onClose}
          />
        )}
      </div>
    </div>
  );
};

export default OrderDetailsModal;
