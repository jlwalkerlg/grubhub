import { useQuery } from "react-query";
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

export function getAuthUserQueryKey() {
  return "auth.user";
}

export async function getAuthUser() {
  try {
    const response = await api.get<UserDto>("/auth/user");
    return response.data;
  } catch (e) {
    localStorage.removeItem("isLoggedIn");

    if (e instanceof ApiError && e.status === 401) {
      return null;
    }

    throw e;
  }
}

export function useAuthUser() {
  return useQuery<UserDto, ApiError>(
    getAuthUserQueryKey(),
    () => {
      if (!localStorage.getItem("isLoggedIn")) {
        return null;
      }

      return getAuthUser();
    },
    {
      staleTime: Infinity,
      retry: false,
      refetchOnWindowFocus: false,
    }
  );
}

export default function useAuth() {
  const { data: user, isLoading } = useAuthUser();

  const isLoggedIn = !!user;

  return {
    user,
    isLoggedIn,
    isLoading,
  };
}
