import { useMutation } from "react-query";
import Api, { ApiError } from "../Api";

interface AcceptOrderCommand {
  orderId: string;
}

export function useAcceptOrder() {
  return useMutation<string, ApiError, AcceptOrderCommand, null>(
    async ({ orderId }) => {
      const response = await Api.put(`/orders/${orderId}/accept`);
      return response.data;
    }
  );
}
