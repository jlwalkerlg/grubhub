import { useQuery, UseQueryOptions } from "react-query";
import api, { ApiError } from "../apii";

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
  restaurantThumbnail: string;
  paymentIntentClientSecret: string;
  customerFirstName: string;
  customerLastName: string;
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
  config?: UseQueryOptions<OrderDto, ApiError>
) {
  return useQuery<OrderDto, ApiError>(
    getOrderQueryKey(orderId),
    async () => {
      const response = await api.get<OrderDto>(`/orders/${orderId}`);
      return response.data;
    },
    config
  );
}
