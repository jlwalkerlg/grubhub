import restaurantsApi, {
  RegisterRestaurantCommand,
  UpdateRestaurantDetailsCommand,
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

  const updateDetails = async (
    id: string,
    command: UpdateRestaurantDetailsCommand
  ): Promise<Result<null, ApiError>> => {
    const response = await restaurantsApi.updateDetails(id, command);

    if (response.isSuccess) {
      return Result.ok<null, ApiError>(null);
    }

    return Result.fail(new ApiError(response));
  };

  return { register, updateDetails };
}
