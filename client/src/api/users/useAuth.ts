import { useQuery, useQueryClient } from "react-query";
import api, { ApiError } from "~/api/api";

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

  const { data: user, isLoading, refetch } = useQuery<UserDto, ApiError>(
    getAuthUserQueryKey(),
    async () => {
      if (!localStorage.getItem("XSRF-TOKEN")) {
        return null;
      }

      try {
        return await getAuthUser();
      } catch (e) {
        if (api.isApiError(e) && e.status === 401) {
          localStorage.removeItem("XSRF-TOKEN");
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
  };

  const removeUser = () => {
    queryClient.setQueryData(getAuthUserQueryKey(), null);
    localStorage.removeItem("XSRF-TOKEN");
  };

  return {
    user,
    isLoggedIn,
    isLoading,
    setUser,
    removeUser,
  };
}
