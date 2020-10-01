export interface MenuDto {
  id: string;
  restaurantId: string;
  categories: MenuCategoryDto[];
}

export interface MenuCategoryDto {
  id: string;
  menuId: string;
  name: string;
  items: MenuItemDto[];
}

export interface MenuItemDto {
  id: string;
  menuCategoryId: string;
  name: string;
  description: string;
  price: number;
}
