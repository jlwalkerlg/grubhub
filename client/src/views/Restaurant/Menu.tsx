import React, { FC, useCallback, useEffect, useState } from "react";
import { MenuDto, MenuItemDto } from "~/api/menu/MenuDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import ChevronIcon from "~/components/Icons/ChevronIcon";
import useScroll from "~/services/useScroll";
import { OrderItemModal } from "./Order";

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
        <OrderItemModal
          restaurantId={menu.restaurantId}
          menuItemId={item.id}
          menuItemName={item.name}
          menuItemDescription={item.description}
          menuItemPrice={item.price}
          closeModal={closeModal}
        />
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
      {restaurant.menu?.categories
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
      {restaurant.menu?.categories
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
