import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

interface RejectOrderCommand {
  orderId: string;
}

export function useRejectOrder() {
  return useMutation<string, ApiError, RejectOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/reject`);
      return response.data;
    }
  );
}
