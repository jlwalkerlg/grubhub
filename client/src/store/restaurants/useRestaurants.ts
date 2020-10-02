import { useDispatch } from "react-redux";
import { MenuItemDto } from "~/api/restaurants/MenuDto";
import restaurantsApi, {
  AddMenuItemRequest,
  RegisterRestaurantCommand,
} from "~/api/restaurants/restaurantsApi";
import { Result } from "~/services/Result";
import { createAddMenuItemAction } from "../auth/authActionCreators";
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

  const addMenuItem = async (
    categoryId: string,
    request: AddMenuItemRequest
  ): Promise<Result> => {
    const response = await restaurantsApi.addMenuItem(
      menu.id,
      categoryId,
      request
    );

    if (!response.isSuccess) {
      return Result.fail(response.error);
    }

    const category = menu.categories.find((x) => x.id === categoryId);

    const item: MenuItemDto = {
      id: response.data,
      menuCategoryId: category.id,
      ...request,
    };

    dispatch(createAddMenuItemAction(item));

    return Result.ok();
  };

  return { register, addMenuItem };
}
