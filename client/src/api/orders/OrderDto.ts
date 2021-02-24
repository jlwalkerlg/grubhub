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
  restaurantName: string;
  restaurantAddress: string;
  restaurantPhoneNumber: string;
  paymentIntentClientSecret: string;
  items: OrderItemDto[];
}

export type OrderStatus = "Placed" | "PaymentConfirmed";

export interface OrderItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemDescription: string;
  menuItemPrice: number;
  quantity: number;
}
