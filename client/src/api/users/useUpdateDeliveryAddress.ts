import { useMutation, useQueryClient } from "react-query";
import Api, { ApiError } from "../api";
import { getAuthUserQueryKey } from "./useAuth";

interface UpdateDeliveryAddressCommand {
  addressLine1: string;
  addressLine2?: string;
  city: string;
  postcode: string;
}

export default function useUpdateDeliveryAddress() {
  const queryClient = useQueryClient();

  return useMutation<void, ApiError, UpdateDeliveryAddressCommand, null>(
    async (command) => {
      await Api.put("/account/delivery-address", command);
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries(getAuthUserQueryKey());
      },
    }
  );
}
