import { useMutation } from "react-query";
import api, { ApiError } from "../api";

interface AcceptOrderCommand {
  orderId: string;
}

export function useAcceptOrder() {
  return useMutation<string, ApiError, AcceptOrderCommand, null>(
    async ({ orderId }) => {
      const response = await api.put(`/orders/${orderId}/accept`);
      return response.data;
    }
  );
}
