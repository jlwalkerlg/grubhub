import { useQuery, useQueryClient } from "react-query";
import api, { ApiError } from "~/api/apii";

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  mobileNumber?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  postcode?: string;
  restaurantId?: string;
  restaurantName?: string;
}

export type UserRole = "RestaurantManager" | "Customer" | "Admin";

export const getAuthUser = async () => {
  const { data: user } = await api.get<UserDto>("/auth/user");
  return user;
};

export const getAuthUserQueryKey = () => "auth.user";

export default function useAuth() {
  const queryClient = useQueryClient();

  const { data: user, isLoading } = useQuery<UserDto, ApiError>(
    getAuthUserQueryKey(),
    async () => {
      if (!localStorage.getItem("isLoggedIn")) {
        return null;
      }

      try {
        return await getAuthUser();
      } catch (e) {
        if (e instanceof ApiError && e.status === 401) {
          localStorage.removeItem("isLoggedIn");
          return null;
        }

        throw e;
      }
    },
    {
      staleTime: Infinity,
      retry: false,
      refetchOnWindowFocus: false,
    }
  );

  const isLoggedIn = !!user;

  const setUser = (user: UserDto) => {
    queryClient.setQueryData(getAuthUserQueryKey(), user);
    if (user) {
      localStorage.setItem("isLoggedIn", "true");
    } else {
      localStorage.removeItem("isLoggedIn");
    }
  };

  return {
    user,
    isLoggedIn,
    isLoading,
    setUser,
  };
}
