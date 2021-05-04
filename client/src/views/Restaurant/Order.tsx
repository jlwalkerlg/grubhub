import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, useEffect, useRef, useState } from "react";
import useBasket, { BasketDto, BasketItemDto } from "~/api/baskets/useBasket";
import useRemoveFromBasket from "~/api/baskets/useRemoveFromBasket";
import { useUpdateBasketItemQuantity } from "~/api/baskets/useUpdateBasketItemQuantity";
import { RestaurantDto } from "~/api/restaurants/useRestaurant";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import CartIcon from "~/components/Icons/CartIcon";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import PlusIcon from "~/components/Icons/PlusIcon";
import SpinnerIcon from "~/components/Icons/SpinnerIcon";
import TrashIcon from "~/components/Icons/TrashIcon";
import { useToasts } from "~/components/Toaster/Toaster";
import useEscapeKeyListener from "~/services/useEscapeKeyListener";
import useFocusTrap from "~/services/useFocusTrap";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import { isRestaurantOpen } from "~/services/utils";

const BasketItemModal: FC<{
  basket: BasketDto;
  item: BasketItemDto;
  close: () => any;
}> = ({ basket, item, close }) => {
  const closeButtonRef = useRef<HTMLButtonElement>();
  const updateBasketItemButtonRef = useRef<HTMLButtonElement>();
  useFocusTrap(true, closeButtonRef, updateBasketItemButtonRef);

  usePreventBodyScroll(true);

  const [quantity, setQuantity] = useState(() => {
    return basket.items.find((x) => x.menuItemId === item.menuItemId).quantity;
  });

  const incrementQuantity = () => setQuantity(quantity + 1);
  const decrementQuantity = () => setQuantity(quantity - 1 || quantity);

  const {
    mutate: updateItemQuantity,
    isLoading,
    isError,
    error,
  } = useUpdateBasketItemQuantity();

  const submit = () => {
    if (isLoading) return;

    updateItemQuantity(
      {
        restaurantId: basket.restaurantId,
        menuItemId: item.menuItemId,
        quantity,
      },
      {
        onSuccess: close,
      }
    );
  };

  const onClose = () => {
    if (!isLoading) close();
  };

  useEscapeKeyListener(onClose);

  return (
    <div className="fixed w-screen h-screen md:py-8 top-0 left-0 flex items-center justify-center z-50">
      <div
        className="fixed w-screen h-screen top-0 left-0"
        style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
        onClick={onClose}
      ></div>

      <div className="relative max-h-full h-full sm:h-auto w-full sm:w-3/4 sm:max-w-md sm:rounded overflow-hidden flex flex-col bg-gray-50">
        <div className="bg-white shadow-lg h-12 flex items-center justify-between p-4">
          <div className="h-6 w-6" aria-hidden></div>

          <p className="font-semibold text-gray-800">{item.menuItemName}</p>

          <button ref={closeButtonRef} onClick={onClose}>
            <CloseIcon className="h-6 w-6 text-primary" />
          </button>
        </div>

        <div className="flex-1 overflow-y-auto flex flex-col justify-center items-center text-center px-4 py-12">
          {isError && (
            <div className="mb-4">
              <ErrorAlert message={error.message} />
            </div>
          )}

          <p className="font-semibold">£{item.menuItemPrice.toFixed(2)}</p>

          {item.menuItemDescription && (
            <p className="mt-4 text-sm">{item.menuItemDescription}</p>
          )}

          <div className="flex items-center mt-4">
            <button
              onClick={decrementQuantity}
              disabled={quantity === 1}
              className={
                quantity === 1
                  ? "text-primary-disabled cursor-default"
                  : "text-primary"
              }
            >
              <PlusIcon className="w-10 h-10" />
            </button>

            <div className="mx-4 text-3xl">{quantity}</div>

            <button onClick={incrementQuantity} className="text-primary">
              <PlusIcon className="w-10 h-10" />
            </button>
          </div>

          <div className="mt-8">
            <button
              ref={updateBasketItemButtonRef}
              onClick={submit}
              className={`btn text-red-700 w-full ${
                isLoading ? "disabled" : ""
              }`}
            >
              Update £{(item.menuItemPrice * quantity).toFixed(2)}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

const BasketItem: FC<{
  restaurantId: string;
  item: BasketItemDto;
  openUpdateModal: () => any;
}> = ({ restaurantId, item, openUpdateModal }) => {
  const { addToast } = useToasts();

  const { mutate: removeBasketItem, isLoading } = useRemoveFromBasket();

  const onRemove = () => {
    if (isLoading) return;

    removeBasketItem(
      {
        restaurantId,
        menuItemId: item.menuItemId,
      },
      {
        onError: (error) => {
          addToast(error.message);
        },
      }
    );
  };

  const price = item.menuItemPrice * item.quantity;

  return (
    <li className="flex items-center mt-2">
      <span className="bg-primary-200 text-primary font-semibold flex items-center justify-center h-6 w-6 flex-0">
        {item.quantity}
      </span>

      <button
        className="ml-4 text-primary text-sm flex-1 mr-4 text-left"
        onClick={openUpdateModal}
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
    </li>
  );
};

const MobileBasketModal: FC<{
  restaurant: RestaurantDto;
  basket: BasketDto;
  subtotal: number;
  close: () => any;
}> = ({ restaurant, basket, subtotal, close }) => {
  const closeButtonRef = useRef<HTMLButtonElement>();
  const paymentLinkRef = useRef<HTMLAnchorElement>();
  useFocusTrap(true, closeButtonRef, paymentLinkRef);

  usePreventBodyScroll();

  const [selectedBasketItem, setSelectedBasketItem] = useState<BasketItemDto>();

  useEscapeKeyListener(() => {
    if (!selectedBasketItem) close();
  });

  return (
    <div className="bg-white border border-gray-200 fixed h-screen w-screen top-0 left-0 z-40">
      <div className="shadow-lg p-4 text-center flex items-center justify-between">
        <button ref={closeButtonRef} onClick={close}>
          <ChevronIcon direction="left" className="w-10 h-10 text-primary" />
        </button>

        <h2 className="font-bold text-xl tracking-wider text-gray-800">
          Your order
        </h2>

        <div aria-hidden className="w-10 h-10"></div>
      </div>

      <div className="px-4 py-8">
        <ul>
          {basket.items.map((item) => {
            return (
              <BasketItem
                key={item.menuItemId}
                restaurantId={basket.restaurantId}
                item={item}
                openUpdateModal={() => setSelectedBasketItem(item)}
              />
            );
          })}
        </ul>

        {selectedBasketItem && (
          <BasketItemModal
            basket={basket}
            item={selectedBasketItem}
            close={() => setSelectedBasketItem(null)}
          />
        )}

        <hr className="my-8 border-gray-300" />

        <p className="text-gray-800 text-sm font-semibold flex items-center justify-between">
          <span>Subtotal</span>
          <span>£{subtotal.toFixed(2)}</span>
        </p>

        {subtotal < restaurant.minimumDeliverySpend ? (
          <p className="text-gray-800 text-sm mt-4">
            Spend £{(restaurant.minimumDeliverySpend - subtotal).toFixed(2)}{" "}
            more for delivery.
          </p>
        ) : (
          <>
            <p className="mt-2 text-gray-800 text-sm flex items-center justify-between">
              <span>Delivery fee</span>
              <span>£{restaurant.deliveryFee.toFixed(2)}</span>
            </p>

            <p className="mt-3 text-gray-800 text-sm font-semibold flex items-center justify-between">
              <span>Total</span>
              <span>£{(subtotal + restaurant.deliveryFee).toFixed(2)}</span>
            </p>
          </>
        )}

        {subtotal >= restaurant.minimumDeliverySpend && (
          <div className="mt-8">
            <Link href={`/restaurants/${basket.restaurantId}/checkout`}>
              <a
                ref={paymentLinkRef}
                className="btn text-red-700 w-full block text-center"
              >
                Go to payment
              </a>
            </Link>
          </div>
        )}
      </div>
    </div>
  );
};

const OrderDetails: FC<{ restaurant: RestaurantDto; basket: BasketDto }> = ({
  restaurant,
  basket,
}) => {
  const router = useRouter();

  const { isLoggedIn } = useAuth();

  const [selectedBasketItem, setSelectedBasketItem] = useState<BasketItemDto>();

  if (!isLoggedIn) {
    return (
      <p>
        Please{" "}
        <Link href={`/login?redirect_to=${router.asPath}`}>
          <a className="text-primary">login</a>
        </Link>{" "}
        to order.
      </p>
    );
  }

  const subtotal =
    basket.items.reduce(
      (carry, item) => carry + item.menuItemPrice * item.quantity,
      0
    ) || 0;

  return (
    <>
      <ul>
        {basket.items.map((item) => {
          return (
            <BasketItem
              key={item.menuItemId}
              restaurantId={basket.restaurantId}
              item={item}
              openUpdateModal={() => setSelectedBasketItem(item)}
            />
          );
        })}
      </ul>

      {selectedBasketItem && (
        <BasketItemModal
          basket={basket}
          item={selectedBasketItem}
          close={() => setSelectedBasketItem(null)}
        />
      )}

      <hr className="my-3 border-gray-300" />

      <p className="text-gray-800 text-sm font-semibold flex items-center justify-between">
        <span>Subtotal</span>
        <span>£{subtotal.toFixed(2)}</span>
      </p>

      {subtotal >= restaurant.minimumDeliverySpend ? (
        <>
          <p className="mt-2 text-gray-800 text-sm flex items-center justify-between">
            <span>Delivery fee</span>
            <span>£{restaurant.deliveryFee.toFixed(2)}</span>
          </p>

          <p className="mt-3 text-gray-800 text-sm font-semibold flex items-center justify-between">
            <span>Total</span>
            <span>£{(subtotal + restaurant.deliveryFee).toFixed(2)}</span>
          </p>

          <Link href={`/restaurants/${basket.restaurantId}/checkout`}>
            <a className="btn btn-primary w-full block text-center mt-6">
              Go to payment
            </a>
          </Link>
        </>
      ) : (
        <p className="mt-6 text-gray-800 text-sm">
          Spend £{(restaurant.minimumDeliverySpend - subtotal).toFixed(2)} more
          for delivery.
        </p>
      )}
    </>
  );
};

const OrderAsideBox: FC = ({ children }) => {
  return (
    <div className="sticky top-20 -mt-36 bg-white rounded border border-gray-200 shadow-lg p-4 hidden md:block">
      <h2 className="font-bold text-xl tracking-wider text-gray-800">
        Your order
      </h2>

      <hr className="my-3 border-gray-300" />

      {children}
    </div>
  );
};

const OrderMobile: FC<{ restaurant: RestaurantDto; basket: BasketDto }> = ({
  restaurant,
  basket,
}) => {
  const subtotal =
    basket.items.reduce(
      (carry, item) => carry + item.menuItemPrice * item.quantity,
      0
    ) || 0;

  const totalItems =
    basket?.items.reduce((carry, item) => carry + item.quantity, 0) || 0;

  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    if (totalItems === 0) {
      setIsModalOpen(false);
    }
  }, [totalItems]);

  return (
    <>
      <button
        onClick={() => setIsModalOpen(true)}
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
        <MobileBasketModal
          restaurant={restaurant}
          basket={basket}
          subtotal={subtotal}
          close={() => setIsModalOpen(false)}
        />
      )}
    </>
  );
};

const OrderContainer: FC<{ restaurant: RestaurantDto }> = ({ restaurant }) => {
  const { data: basket, isLoading, isError } = useBasket(restaurant.id);

  const restaurantIsOpen = isRestaurantOpen(restaurant);

  if (isLoading) {
    return (
      <OrderAsideBox>
        <SpinnerIcon className="w-6 h-6 animate-spin" />
      </OrderAsideBox>
    );
  }

  if (isError) {
    return (
      <OrderAsideBox>
        <p>There was an unexpected error while loaded your order.</p>
      </OrderAsideBox>
    );
  }

  if (!restaurantIsOpen) {
    return (
      <OrderAsideBox>
        <p>Restaurant is closed.</p>
      </OrderAsideBox>
    );
  }

  if (!basket?.items.length) {
    return (
      <OrderAsideBox>
        <p className="text-gray-800 text-sm">
          Spend £{restaurant.minimumDeliverySpend.toFixed(2)} for delivery.
        </p>
      </OrderAsideBox>
    );
  }

  return (
    <>
      <OrderAsideBox>
        <OrderDetails restaurant={restaurant} basket={basket} />
      </OrderAsideBox>

      <OrderMobile restaurant={restaurant} basket={basket} />
    </>
  );
};

export default OrderContainer;
