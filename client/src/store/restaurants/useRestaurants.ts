import { useDispatch } from "react-redux";
import { MenuCategoryDto, MenuItemDto } from "~/api/restaurants/MenuDto";
import restaurantsApi, {
  AddMenuCategoryRequest,
  AddMenuItemRequest,
  RegisterRestaurantCommand,
  UpdateMenuItemRequest,
} from "~/api/restaurants/restaurantsApi";
import { Result } from "~/services/Result";
import {
  createAddMenuCategoryAction,
  createAddMenuItemAction,
  createUpdateMenuItemAction,
} from "../auth/authActionCreators";
import useAuth from "../auth/useAuth";

export default function useRestaurants() {
  const { menu } = useAuth();
  const dispatch = useDispatch();

  const register = async (
    command: RegisterRestaurantCommand
  ): Promise<Result> => {
    const response = await restaurantsApi.register(command);

    if (response.isSuccess) {
      return Result.ok();
    }

    return Result.fail(response.error);
  };

  const addMenuCategory = async (
    request: AddMenuCategoryRequest
  ): Promise<Result> => {
    const response = await restaurantsApi.addMenuCategory(menu.id, request);

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const category: MenuCategoryDto = {
      name: request.name,
      items: [],
    };

    dispatch(createAddMenuCategoryAction(category));

    return Result.ok();
  };

  const addMenuItem = async (request: AddMenuItemRequest): Promise<Result> => {
    const response = await restaurantsApi.addMenuItem(menu.id, request);

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const category = menu.categories.find((x) => x.name === request.category);

    const item: MenuItemDto = {
      name: request.name,
      description: request.description,
      price: request.price,
    };

    dispatch(createAddMenuItemAction(category.name, item));

    return Result.ok();
  };

  const updateMenuItem = async (
    categoryName: string,
    itemName: string,
    request: UpdateMenuItemRequest
  ): Promise<Result> => {
    const response = await restaurantsApi.updateMenuItem(
      menu.id,
      categoryName,
      itemName,
      request
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const category = menu.categories.find((x) => x.name === categoryName);

    const item: MenuItemDto = {
      name: request.name,
      description: request.description,
      price: request.price,
    };

    dispatch(createUpdateMenuItemAction(category.name, itemName, item));

    return Result.ok();
  };

  return { register, addMenuCategory, addMenuItem, updateMenuItem };
}
