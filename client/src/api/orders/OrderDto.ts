export interface OrderDto {
  id: string;
  userId: string;
  restaurantId: string;
  status: string;
  items: OrderItemDto[];
}

export interface OrderItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemPrice: number;
  quantity: number;
}
