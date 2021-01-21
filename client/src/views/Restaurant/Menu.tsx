import Link from "next/link";
import { useRouter } from "next/router";
import React, { FC, useCallback, useEffect, useRef, useState } from "react";
import { MenuDto, MenuItemDto } from "~/api/menu/MenuDto";
import { useAddToOrder } from "~/api/orders/useAddToOrder";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import useAuth from "~/api/users/useAuth";
import { ErrorAlert } from "~/components/Alert/Alert";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import useEscapeKeyListener from "~/services/useEscapeKeyListener";
import useFocusTrap from "~/services/useFocusTrap";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import useScroll from "~/services/useScroll";

const OrderItemModal: FC<{
  menu: MenuDto;
  item: MenuItemDto;
  closeModal: () => any;
}> = ({ menu, item, closeModal }) => {
  const closeButtonRef = useRef<HTMLButtonElement>();
  const addToOrderButtonRef = useRef<HTMLButtonElement>();
  useFocusTrap(true, closeButtonRef, addToOrderButtonRef);

  usePreventBodyScroll(true);

  const [addToOrder, { isLoading, isError, error }] = useAddToOrder();

  const submit = () => {
    if (isLoading) return;

    addToOrder(
      {
        restaurantId: menu.restaurantId,
        menuItemId: item.id,
      },
      {
        onSuccess: closeModal,
      }
    );
  };

  const close = useCallback(() => {
    if (!isLoading) closeModal();
  }, [isLoading, closeModal]);

  useEscapeKeyListener(close, [close]);

  const router = useRouter();

  const { isLoggedIn } = useAuth();

  return (
    <div className="fixed w-screen h-screen md:py-8 top-0 left-0 flex items-center justify-center z-50">
      <div
        className="fixed w-screen h-screen top-0 left-0"
        style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
        onClick={close}
      ></div>

      <div className="relative max-h-full h-full sm:h-auto w-full sm:w-3/4 sm:max-w-md sm:rounded overflow-hidden flex flex-col bg-gray-50">
        <div className="bg-white shadow-lg h-12 flex items-center justify-between p-4">
          <div className="h-6 w-6" aria-hidden></div>

          <p className="font-semibold text-gray-800">{item.name}</p>

          <button ref={closeButtonRef} onClick={close}>
            <CloseIcon className="h-6 w-6 text-primary" />
          </button>
        </div>

        <div className="flex-1 overflow-y-auto flex flex-col justify-center text-center px-4 py-12">
          {isError && (
            <div className="mb-4">
              <ErrorAlert message={error.message} />
            </div>
          )}

          <p className="font-semibold">£{item.price.toFixed(2)}</p>

          {item.description && (
            <p className="mt-4 text-sm">{item.description}</p>
          )}
        </div>

        <div className="p-4 bg-white -shadow-lg">
          {isLoggedIn ? (
            <button
              ref={addToOrderButtonRef}
              onClick={submit}
              className={`btn btn-primary w-full ${
                isLoading ? "disabled" : ""
              }`}
            >
              Add to order £{item.price.toFixed(2)}
            </button>
          ) : (
            <p>
              Please{" "}
              <Link href={`/login?redirect_to=${router.asPath}`}>
                <a className="text-primary">login</a>
              </Link>{" "}
              to start an order.
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

const MenuItem: FC<{ menu: MenuDto; item: MenuItemDto }> = ({ menu, item }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const openModal = useCallback(() => setIsModalOpen(true), []);
  const closeModal = useCallback(() => setIsModalOpen(false), []);

  return (
    <li>
      <button
        onClick={openModal}
        className="bg-white rounded border border-gray-200 p-4 w-full text-left mt-2"
      >
        <p className="font-bold text-gray-700 text-xl">{item.name}</p>
        {item.description && (
          <p className="text-gray-700 text-sm mt-1 whitespace-pre-line">
            {item.description}
          </p>
        )}
        <p className="text-primary mt-2">£{item.price.toFixed(2)}</p>
      </button>

      {isModalOpen && (
        <OrderItemModal menu={menu} item={item} closeModal={closeModal} />
      )}
    </li>
  );
};

export const Menu: FC<{
  restaurant: RestaurantDto;
  setHash: (hash: string) => any;
}> = ({ restaurant, setHash }) => {
  const [listEl, setListEl] = useState<HTMLElement>();
  const [elements, setElements] = useState<HTMLElement[]>([]);

  useEffect(() => {
    setListEl(document.getElementById("categoryList"));

    setElements(
      Array.prototype.slice.call(
        document.querySelectorAll("#categoryList > div")
      )
    );
  }, []);

  useScroll(
    () => {
      if (!listEl || elements.length === 0) return;

      if (listEl.getBoundingClientRect().y > window.innerHeight) {
        setHash(elements[0]?.id ?? "");
        return;
      }

      for (let i = 0; i < elements.length; i++) {
        const el = elements[i];

        const rect = el.getBoundingClientRect();

        if (rect.y >= 64 && rect.y <= window.innerHeight / 2) {
          setHash(el.id);
          return;
        }

        if (rect.y > window.innerHeight / 2) {
          setHash(elements[i - 1]?.id ?? elements[0].id);
          return;
        }
      }

      setHash(elements[elements.length - 1].id);
    },
    500,
    [listEl, elements]
  );

  return (
    <div id="categoryList">
      {restaurant.menu.categories
        .filter((category) => category.items.length > 0)
        .map((category) => {
          return (
            <div key={category.name} id={category.name} className="py-4">
              <h3 className="font-bold text-gray-700 text-2xl">
                {category.name}
              </h3>

              <ul className="mt-4">
                {category.items.map((item) => {
                  return (
                    <MenuItem
                      key={item.name}
                      menu={restaurant.menu}
                      item={item}
                    />
                  );
                })}
              </ul>
            </div>
          );
        })}
    </div>
  );
};

export const MobileMenu: FC<{ restaurant: RestaurantDto }> = ({
  restaurant,
}) => {
  const [openCategories, setOpenCategories] = useState<string[]>([]);

  const openCategory = (category: string) => {
    setOpenCategories([category, ...openCategories]);
  };

  const closeCategory = (category: string) => {
    setOpenCategories(openCategories.filter((x) => x !== category));
  };

  return (
    <div>
      {restaurant.menu.categories
        .filter((category) => category.items.length > 0)
        .map((category) => {
          const isCategoryOpen = openCategories.includes(category.name);

          return (
            <div
              key={category.name}
              id={category.name}
              className="border-b border-gray-400"
            >
              <button
                className="flex items-center justify-between p-4 w-full"
                onClick={
                  isCategoryOpen
                    ? () => closeCategory(category.name)
                    : () => openCategory(category.name)
                }
              >
                <h3 className="font-bold text-gray-700 text-2xl">
                  {category.name}
                </h3>

                {isCategoryOpen ? (
                  <ChevronIcon
                    direction="up"
                    className="h-8 w-8 text-primary"
                  />
                ) : (
                  <ChevronIcon
                    direction="down"
                    className="h-8 w-8 text-primary"
                  />
                )}
              </button>

              {isCategoryOpen && (
                <ul className="px-4 pb-4">
                  {category.items.map((item) => {
                    return (
                      <MenuItem
                        key={item.name}
                        menu={restaurant.menu}
                        item={item}
                      />
                    );
                  })}
                </ul>
              )}
            </div>
          );
        })}
    </div>
  );
};
