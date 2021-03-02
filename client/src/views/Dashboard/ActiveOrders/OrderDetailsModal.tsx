import React, { FC, MutableRefObject, useMemo, useRef } from "react";
import { ActiveOrderDto } from "~/api/orders/useActiveRestaurantOrders";
import useOrder, { OrderDto } from "~/api/orders/useOrder";
import CloseIcon from "~/components/Icons/CloseIcon";
import useEscapeKeyListener from "~/services/useEscapeKeyListener";
import useFocusTrap from "~/services/useFocusTrap";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import { formatDate } from "~/services/utils";

const OrderDetails: FC<{
  order: OrderDto;
  closeButtonRef: MutableRefObject<HTMLButtonElement>;
  onClose: () => any;
}> = ({ order, closeButtonRef, onClose }) => {
  const placedAt = useMemo(() => {
    const date = new Date(order.placedAt);
    return formatDate(date, "hh:mm");
  }, [order.placedAt]);

  const estimatedDeliveryTime = useMemo(() => {
    const date = new Date(order.estimatedDeliveryTime);
    return formatDate(date, "hh:mm");
  }, [order.estimatedDeliveryTime]);

  return (
    <div>
      <div className="flex items-center justify-between">
        <h2 className="font-semibold text-2xl">Order #{order.number}</h2>

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
              <span className="ml-2">{order.address}</span>
            </p>
            <p className="mt-2">
              <span className="font-semibold">Placed At</span>
              <span className="ml-2">{placedAt}</span>
            </p>
            <p className="mt-2">
              <span className="font-semibold">Estimated Delivery Time</span>
              <span className="ml-2">{estimatedDeliveryTime}</span>
            </p>
          </div>
        </div>

        <div className="flex-1 mt-4 md:mt-0 md:ml-8">
          <p className="font-semibold text-xl">Customer Details</p>

          <div className="mt-1">
            <p className="mt-2">
              <span className="font-semibold">Name</span>
              <span className="ml-2">{order.customerName}</span>
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
                  key={item.menuItemId}
                  className="flex items-center justify-between mt-2"
                >
                  <p>
                    {item.quantity > 1 ? `${item.quantity}x ` : ""}
                    {item.menuItemName}
                  </p>

                  <p className="font-semibold text-lg">
                    {item.menuItemPrice.toFixed(2)}
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

const OrderDetailsModal: FC<{ order: ActiveOrderDto; onClose: () => any }> = ({
  order,
  onClose,
}) => {
  useEscapeKeyListener(onClose);

  const closeButtonRef = useRef<HTMLButtonElement>();
  useFocusTrap(true, closeButtonRef);

  usePreventBodyScroll(true);

  const { data, isLoading, isError } = useOrder(order.id);

  return (
    <div className="fixed inset-y-0 left-0 w-full z-50 flex items-center md:px-8">
      <div
        className="fixed inset-0"
        style={{ backgroundColor: "rgba(0,0,0,0.7)" }}
        onClick={onClose}
      ></div>

      <div className="relative flex-1 max-w-3xl mx-auto h-full overflow-y-auto md:h-auto bg-white md:rounded-lg p-4">
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
