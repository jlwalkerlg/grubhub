import { QueryConfig, useQuery } from "react-query";
import Api, { ApiError } from "../Api";

export interface OrderDto {
  id: string;
  number: number;
  userId: string;
  restaurantId: string;
  subtotal: number;
  deliveryFee: number;
  serviceFee: number;
  status: OrderStatus;
  address: string;
  placedAt: string;
  confirmedAt?: string;
  acceptedAt?: string;
  deliveredAt?: string;
  estimatedDeliveryTime: string;
  restaurantName: string;
  restaurantAddress: string;
  restaurantPhoneNumber: string;
  paymentIntentClientSecret: string;
  customerName: string;
  customerEmail: string;
  customerMobile: string;
  items: OrderItemDto[];
}

export type OrderStatus =
  | "Placed"
  | "PaymentConfirmed"
  | "Accepted"
  | "Rejected"
  | "Delivered";

export interface OrderItemDto {
  id: number;
  name: string;
  price: number;
  quantity: number;
}

export function getOrderQueryKey(orderId: string) {
  return `/orders/${orderId}`;
}

export default function useOrder(
  orderId: string,
  config?: QueryConfig<OrderDto, ApiError>
) {
  return useQuery<OrderDto, ApiError>(
    getOrderQueryKey(orderId),
    async () => {
      const response = await Api.get<OrderDto>(`/orders/${orderId}`);
      return response.data;
    },
    config
  );
}
