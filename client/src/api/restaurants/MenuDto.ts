export interface MenuDto {
  id: string;
  restaurantId: string;
  categories: MenuCategoryDto[];
}

export interface MenuCategoryDto {
  name: string;
  items: MenuItemDto[];
}

export interface MenuItemDto {
  name: string;
  description: string;
  price: number;
}
