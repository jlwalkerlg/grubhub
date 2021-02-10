export interface OrderDto {
  id: string;
  userId: string;
  restaurantId: string;
  status: OrderStatus;
  address: string;
  placedAt: string;
  items: OrderItemDto[];
}

type OrderStatus = "Active" | "Placed" | "Cancelled";

export interface OrderItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemDescription: string;
  menuItemPrice: number;
  quantity: number;
}
