export interface BasketDto {
  userId: string;
  restaurantId: string;
  items: BasketItemDto[];
}

export interface BasketItemDto {
  menuItemId: string;
  menuItemName: string;
  menuItemDescription: string;
  menuItemPrice: number;
  quantity: number;
}
