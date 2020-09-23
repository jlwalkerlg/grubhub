import restaurantsApi, {
  RegisterRestaurantCommand,
} from "~/api/restaurants/restaurantsApi";
import { Result } from "~/lib/Result";

export default function useRestaurants() {
  const register = async (
    command: RegisterRestaurantCommand
  ): Promise<Result<null>> => {
    const response = await restaurantsApi.register(command);

    if (response.isSuccess) {
      return Result.ok(null);
    }

    return Result.fail(response.error);
  };

  return { register };
}
