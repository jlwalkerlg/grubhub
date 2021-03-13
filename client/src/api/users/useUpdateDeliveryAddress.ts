import { useMutation, useQueryCache } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUserQueryKey } from "./useAuth";

interface UpdateDeliveryAddressCommand {
  addressLine1: string;
  addressLine2?: string;
  city: string;
  postcode: string;
}

export default function useUpdateDeliveryAddress() {
  const cache = useQueryCache();

  return useMutation<void, ApiError, UpdateDeliveryAddressCommand, null>(
    async (command) => {
      await Api.put("/account/delivery-address", command);
    },
    {
      onSuccess: () => {
        cache.invalidateQueries(getAuthUserQueryKey());
      },
    }
  );
}
