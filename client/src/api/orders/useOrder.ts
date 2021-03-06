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
  addressLine1: string;
  addressLine2: string;
  city: string;
  postcode: string;
  placedAt: string;
  confirmedAt?: string;
  acceptedAt?: string;
  deliveredAt?: string;
  rejectedAt?: string;
  cancelledAt?: string;
  estimatedDeliveryTime: string;
  restaurantName: string;
  restaurantAddressLine1: string;
  restaurantAddressLine2: string;
  restaurantCity: string;
  restaurantPostcode: string;
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
  | "Cancelled"
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
