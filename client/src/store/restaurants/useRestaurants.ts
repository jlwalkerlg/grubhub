import restaurantsApi, {
  RegisterRestaurantCommand,
} from "~/api/restaurants/restaurantsApi";
import { Result } from "~/services/Result";

export default function useRestaurants() {
  const register = async (
    command: RegisterRestaurantCommand
  ): Promise<Result> => {
    const response = await restaurantsApi.register(command);

    if (response.isSuccess) {
      return Result.ok();
    }

    return Result.fail(response.error);
  };

  return { register };
}
