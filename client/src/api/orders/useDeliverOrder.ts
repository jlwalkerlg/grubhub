import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

interface DeliverOrderCommand {
  orderId: string;
}

export function useDeliverOrder() {
  return useMutation<string, ApiError, DeliverOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/deliver`);
      return response.data;
    }
  );
}
