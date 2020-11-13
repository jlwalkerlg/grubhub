export interface MenuDto {
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
