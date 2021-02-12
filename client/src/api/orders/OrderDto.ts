export interface OrderDto {
  id: string;
  userId: string;
  restaurantId: string;
  subtotal: number;
  deliveryFee: number;
  serviceFee: number;
  status: OrderStatus;
  address: string;
  placedAt: string;
  restaurantAddress: string;
  paymentIntentClientSecret: string;
  items: OrderItemDto[];
}

type OrderStatus = "Placed" | "PaymentConfirmed";

export interface OrderItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemDescription: string;
  menuItemPrice: number;
  quantity: number;
}
