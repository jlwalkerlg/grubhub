import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, useCallback, useEffect, useRef, useState } from "react";
import { ApiError } from "~/api/Api";
import { OrderDto, OrderItemDto } from "~/api/orders/OrderDto";
import useActiveOrder from "~/api/orders/useActiveOrder";
import useRemoveFromOrder from "~/api/orders/useRemoveFromOrder";
import useAuth from "~/api/users/useAuth";
import CartIcon from "~/components/Icons/CartIcon";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import TrashIcon from "~/components/Icons/TrashIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import useEscapeKeyListener from "~/services/useEscapeKeyListener";
import useFocusTrap from "~/services/useFocusTrap";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import { OrderItemModal } from "./Menu";

const OrderItem: FC<{ restaurantId: string; item: OrderItemDto }> = ({
  restaurantId,
  item,
}) => {
  const { addToast } = useToasts();

  const [remove, { isLoading }] = useRemoveFromOrder();

  const onRemove = () => {
    if (isLoading) return;

    remove(
      {
        restaurantId,
        menuItemId: item.menuItemId,
      },
      {
        onError: (error) => {
          addToast(`Failed to remove item from order: ${error.message}`);
        },
      }
    );
  };

  const [isModalOpen, setIsModalOpen] = useState(false);

  const openModal = () => setIsModalOpen(true);
  const closeModal = () => setIsModalOpen(false);

  const price = item.menuItemPrice * item.quantity;

  return (
    <li className="flex items-center mt-2">
      <span className="bg-primary-200 text-primary font-semibold flex items-center justify-center h-6 w-6 flex-0">
        {item.quantity}
      </span>

      <button
        className="ml-4 text-primary text-sm flex-1 mr-4 text-left"
        onClick={openModal}
      >
        {item.menuItemName}
      </button>

      <div className="flex-0 flex items-center">
        {isLoading ? (
          <SpinnerIcon className="w-5 h-5 animate-spin" />
        ) : (
          <button onClick={onRemove}>
            <TrashIcon className="w-5 h-5 text-primary" />
          </button>
        )}
      </div>

      <span className="ml-2 w-14 text-right flex-0 text-gray-700">
        £{price.toFixed(2)}
      </span>

      {isModalOpen && (
        <OrderItemModal
          restaurantId={restaurantId}
          menuItemId={item.menuItemId}
          menuItemName={item.menuItemName}
          menuItemDescription={item.menuItemDescription}
          menuItemPrice={item.menuItemPrice}
          closeModal={closeModal}
        />
      )}
    </li>
  );
};

const MobileOrderModal: FC<{
  order: OrderDto;
  subtotal: number;
  close: () => any;
}> = ({ order, subtotal, close }) => {
  const closeButtonRef = useRef<HTMLButtonElement>();
  const paymentLinkRef = useRef<HTMLAnchorElement>();
  useFocusTrap(true, closeButtonRef, paymentLinkRef);

  usePreventBodyScroll();

  useEscapeKeyListener(close);

  return (
    <div className="bg-white border border-gray-200 fixed h-screen w-screen top-0 left-0 flex flex-col z-40">
      <div className="shadow-lg p-4 text-center flex items-center justify-between flex-0">
        <button ref={closeButtonRef} onClick={close}>
          <ChevronIcon direction="left" className="w-10 h-10 text-primary" />
        </button>

        <h2 className="font-bold text-xl tracking-wider text-gray-800">
          Your order
        </h2>

        <div aria-hidden className="w-10 h-10"></div>
      </div>

      <div className="flex-1 overflow-y-auto px-4 py-8">
        <ul>
          {order.items.map((item) => {
            return (
              <OrderItem
                key={item.menuItemId}
                restaurantId={order.restaurantId}
                item={item}
              />
            );
          })}
        </ul>

        <hr className="my-8 border-gray-300" />

        <p className="text-gray-800 text-sm font-semibold flex items-center justify-between">
          <span>Subtotal</span>
          <span>£{subtotal.toFixed(2)}</span>
        </p>
      </div>

      <div className="p-4 -shadow-lg flex-0">
        <Link href="/checkout">
          <a
            ref={paymentLinkRef}
            className="btn btn-primary w-full block text-center"
          >
            Go to payment
          </a>
        </Link>
      </div>
    </div>
  );
};

const OrderAside: FC<{
  isLoading: boolean;
  isError: boolean;
  error: ApiError;
  order: OrderDto;
  subtotal: number;
}> = ({ isLoading, isError, error, order, subtotal }) => {
  const { isLoggedIn } = useAuth();

  const router = useRouter();

  console.log(router);

  if (!isLoggedIn) {
    return (
      <div className="sticky top-20 -mt-36 bg-white rounded border border-gray-200 shadow-lg p-4 hidden md:block">
        <h2 className="font-bold text-xl tracking-wider text-gray-800">
          Your order
        </h2>

        <hr className="my-3 border-gray-300" />

        <p>
          Please{" "}
          <Link href={`/login?redirect_to=${router.asPath}`}>
            <a className="text-primary">login</a>
          </Link>{" "}
          to begin an order.
        </p>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div className="sticky top-20 -mt-36 bg-white rounded border border-gray-200 shadow-lg p-4 hidden md:block">
        <h2 className="font-bold text-xl tracking-wider text-gray-800">
          Your order
        </h2>

        <hr className="my-3 border-gray-300" />

        <p>Loading order...</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="sticky top-20 -mt-36 bg-white rounded border border-gray-200 shadow-lg p-4 hidden md:block">
        <h2 className="font-bold text-xl tracking-wider text-gray-800">
          Your order
        </h2>

        <hr className="my-3 border-gray-300" />

        <p>Problem loading order: {error.message}</p>
      </div>
    );
  }

  return (
    <div className="sticky top-20 -mt-36 bg-white rounded border border-gray-200 shadow-lg p-4 hidden md:block">
      <h2 className="font-bold text-xl tracking-wider text-gray-800">
        Your order
      </h2>

      <hr className="my-3 border-gray-300" />

      {order?.items.length > 0 ? (
        <ul>
          {order.items.map((item) => {
            return (
              <OrderItem
                key={item.menuItemId}
                restaurantId={order.restaurantId}
                item={item}
              />
            );
          })}
        </ul>
      ) : (
        <p>
          You haven't added any items to your order...{" "}
          <span className="font-medium italic">yet</span>.
        </p>
      )}

      <hr className="my-6 border-gray-300" />

      <p className="text-gray-800 text-sm font-semibold flex items-center justify-between">
        <span>Subtotal</span>
        <span>£{subtotal.toFixed(2)}</span>
      </p>

      {order?.items.length > 0 && (
        <Link href="/checkout">
          <a className="btn btn-primary w-full block text-center mt-6">
            Go to payment
          </a>
        </Link>
      )}
    </div>
  );
};

const Order: FC<{ restaurantId: string }> = ({ restaurantId }) => {
  const { data: order, isLoading, isError, error } = useActiveOrder(
    restaurantId
  );

  const subtotal =
    order?.items.reduce(
      (carry, item) => carry + item.menuItemPrice * item.quantity,
      0
    ) || 0;

  const totalItems =
    order?.items.reduce((carry, item) => carry + item.quantity, 0) || 0;

  const [isModalOpen, setIsModalOpen] = useState(false);

  const openModal = useCallback(() => setIsModalOpen(true), []);
  const closeModal = useCallback(() => setIsModalOpen(false), []);

  useEffect(() => {
    if (totalItems === 0) {
      setIsModalOpen(false);
    }
  }, [totalItems]);

  return (
    <>
      <OrderAside
        isLoading={isLoading}
        isError={isError}
        error={error}
        order={order}
        subtotal={subtotal}
      />

      {order?.items.length > 0 && (
        <>
          <button
            onClick={openModal}
            className="fixed left-0 bottom-0 w-full bg-white -shadow-lg flex items-center p-4 md:hidden"
          >
            <div className="relative pr-2">
              <CartIcon className="w-8 h-8" />
              <span className="absolute top-0 right-0 border border-white rounded-full px-1 bg-black text-white text-xs font-semibold">
                {totalItems}
              </span>
            </div>
            <span className="ml-4">£{subtotal.toFixed(2)}</span>
          </button>

          {isModalOpen && (
            <MobileOrderModal
              order={order}
              subtotal={subtotal}
              close={closeModal}
            />
          )}
        </>
      )}
    </>
  );
};

export default Order;
