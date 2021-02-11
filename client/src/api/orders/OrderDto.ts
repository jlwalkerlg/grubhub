export interface OrderDto {
  id: string;
  userId: string;
  restaurantId: string;
  subtotal: string;
  delivery_fee: string;
  service_fee: string;
  status: OrderStatus;
  address: string;
  placedAt: string;
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
