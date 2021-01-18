export interface MenuDto {
  restaurantId: string;
  categories: MenuCategoryDto[];
}

export interface MenuCategoryDto {
  id: string;
  name: string;
  items: MenuItemDto[];
}

export interface MenuItemDto {
  id: string;
  name: string;
  description?: string;
  price: number;
}
