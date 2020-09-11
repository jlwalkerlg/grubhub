import restaurantsApi, {
  RegisterRestaurantCommand,
} from "~/api/restaurants/restaurantsApi";
import { ApiError } from "~/lib/Error";
import { Result } from "~/lib/Result";

export default function useRestaurants() {
  const register = async (
    command: RegisterRestaurantCommand
  ): Promise<Result<null, ApiError>> => {
    const response = await restaurantsApi.register(command);

    if (response.isSuccess) {
      return Result.ok<null, ApiError>(null);
    }

    return Result.fail(new ApiError(response));
  };

  return { register };
}
