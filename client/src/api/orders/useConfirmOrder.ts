import { useMutation } from "react-query";
import api, { ApiError } from "../api";

interface ConfirmOrderCommand {
  orderId: string;
}

export async function confirmOrder({ orderId }: ConfirmOrderCommand) {
  await api.put(`/orders/${orderId}/confirm`);
}

export function useConfirmOrder() {
  return useMutation<void, ApiError, ConfirmOrderCommand, null>(confirmOrder);
}
