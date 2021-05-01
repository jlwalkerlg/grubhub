import React, { FC, MouseEvent } from "react";
import { RestaurantDto } from "~/api/restaurants/useRestaurant";

const CuisineList: FC<{
  restaurant: RestaurantDto;
  hash: string;
  setHash: (hash: string) => any;
}> = ({ restaurant, hash, setHash }) => {
  const onClickCategoryLink = (e: MouseEvent<HTMLAnchorElement>) => {
    e.preventDefault();

    const id = e.currentTarget.dataset.target;

    const el = document.getElementById(id);
    const nav = document.getElementById("nav");

    window.scrollBy({
      top: el.getBoundingClientRect().top - nav.offsetHeight,
      behavior: "smooth",
    });

    setHash(id);
  };

  return (
    <ul className="sticky top-20 mt-8 text-gray-600">
      {restaurant.menu?.categories.map((category) => {
        return (
          <li key={category.name}>
            <a
              href={`#${category.name}`}
              data-target={category.name}
              onClick={onClickCategoryLink}
              className={`pl-2 py-2 block hover:text-gray-900 hover:font-semibold border-l border-gray-400 hover:border-gray-900 ${
                hash === category.name
                  ? "text-gray-900 font-semibold border-gray-900"
                  : ""
              }`}
            >
              {category.name}
            </a>
          </li>
        );
      })}
    </ul>
  );
};

export default CuisineList;
