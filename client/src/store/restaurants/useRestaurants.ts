import { useDispatch } from "react-redux";
import { MenuCategoryDto, MenuItemDto } from "~/api/restaurants/MenuDto";
import restaurantsApi, {
  AddMenuCategoryRequest,
  AddMenuItemRequest,
  RegisterRestaurantCommand,
  RenameMenuCategoryRequest,
  UpdateMenuItemRequest,
} from "~/api/restaurants/restaurantsApi";
import { Result } from "~/services/Result";
import {
  createAddMenuCategoryAction,
  createAddMenuItemAction,
  createRemoveMenuCategoryAction,
  createRemoveMenuItemAction,
  createRenameMenuCategoryAction,
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
    const response = await restaurantsApi.addMenuCategory(
      menu.restaurantId,
      request
    );

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

  const removeMenuCategory = async (categoryName: string): Promise<Result> => {
    const response = await restaurantsApi.removeMenuCategory(
      menu.restaurantId,
      categoryName
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    dispatch(createRemoveMenuCategoryAction(categoryName));

    return Result.ok();
  };

  const renameMenuCategory = async (
    oldName: string,
    request: RenameMenuCategoryRequest
  ): Promise<Result> => {
    const response = await restaurantsApi.renameMenuCategory(
      menu.restaurantId,
      oldName,
      request
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    dispatch(createRenameMenuCategoryAction(oldName, request.newName));

    return Result.ok();
  };

  const addMenuItem = async (request: AddMenuItemRequest): Promise<Result> => {
    const response = await restaurantsApi.addMenuItem(
      menu.restaurantId,
      request
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const category = menu.categories.find(
      (x) => x.name === request.categoryName
    );

    const item: MenuItemDto = {
      name: request.itemName,
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
      menu.restaurantId,
      categoryName,
      itemName,
      request
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const item: MenuItemDto = {
      name: request.newItemName,
      description: request.description,
      price: request.price,
    };

    dispatch(createUpdateMenuItemAction(categoryName, itemName, item));

    return Result.ok();
  };

  const removeMenuItem = async (
    categoryName: string,
    itemName: string
  ): Promise<Result> => {
    const response = await restaurantsApi.removeMenuItem(
      menu.restaurantId,
      categoryName,
      itemName
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    dispatch(createRemoveMenuItemAction(categoryName, itemName));

    return Result.ok();
  };

  return {
    register,
    addMenuCategory,
    removeMenuCategory,
    renameMenuCategory,
    addMenuItem,
    updateMenuItem,
    deleteMenuItem: removeMenuItem,
  };
}
