import {
  MenuCategoryDto,
  MenuDto,
  MenuItemDto,
} from "~/api/restaurants/MenuDto";
import { RestaurantDto } from "~/api/restaurants/RestaurantDto";
import { UpdateRestaurantDetailsRequest } from "~/api/restaurants/restaurantsApi";
import { UpdateUserDetailsCommand } from "~/api/users/userApi";
import { UserDto } from "~/api/users/UserDto";

export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";
export const SET_AUTH_RESTAURANT = "SET_AUTH_RESTAURANT";
export const SET_AUTH_RESTAURANT_MENU = "SET_AUTH_RESTAURANT_MENU";
export const UPDATE_RESTAURANT_DETAILS = "UPDATE_RESTAURANT_DETAILS";
export const UPDATE_USER_DETAILS = "UPDATE_USER_DETAILS";
export const ADD_MENU_CATEGORY = "ADD_MENU_CATEGORY";
export const ADD_MENU_ITEM = "ADD_MENU_ITEM";
export const UPDATE_MENU_ITEM = "UPDATE_MENU_ITEM";
export const REMOVE_MENU_CATEGORY = "REMOVE_MENU_CATEGORY";
export const REMOVE_MENU_ITEM = "REMOVE_MENU_ITEM";

export interface LoginAction {
  type: typeof LOGIN;
  payload: {
    user: UserDto;
  };
}

export interface LogoutAction {
  type: typeof LOGOUT;
}

export interface SetAuthRestaurantAction {
  type: typeof SET_AUTH_RESTAURANT;
  payload: {
    restaurant: RestaurantDto;
  };
}

export interface SetAuthRestaurantMenuAction {
  type: typeof SET_AUTH_RESTAURANT_MENU;
  payload: {
    menu: MenuDto;
  };
}

export interface UpdateRestaurantDetailsAction {
  type: typeof UPDATE_RESTAURANT_DETAILS;
  payload: {
    request: UpdateRestaurantDetailsRequest;
  };
}

export interface UpdateUserDetailsAction {
  type: typeof UPDATE_USER_DETAILS;
  payload: {
    command: UpdateUserDetailsCommand;
  };
}

export interface AddMenuCategoryAction {
  type: typeof ADD_MENU_CATEGORY;
  payload: {
    category: MenuCategoryDto;
  };
}

export interface RemoveMenuCategoryAction {
  type: typeof REMOVE_MENU_CATEGORY;
  payload: {
    categoryName: string;
  };
}

export interface AddMenuItemAction {
  type: typeof ADD_MENU_ITEM;
  payload: {
    category: string;
    item: MenuItemDto;
  };
}

export interface UpdateMenuItemAction {
  type: typeof UPDATE_MENU_ITEM;
  payload: {
    categoryName: string;
    oldItemName: string;
    item: MenuItemDto;
  };
}

export interface RemoveMenuItemAction {
  type: typeof REMOVE_MENU_ITEM;
  payload: {
    categoryName: string;
    itemName: string;
  };
}

type AuthAction =
  | LoginAction
  | LogoutAction
  | SetAuthRestaurantAction
  | SetAuthRestaurantMenuAction
  | UpdateRestaurantDetailsAction
  | UpdateUserDetailsAction
  | AddMenuCategoryAction
  | RemoveMenuCategoryAction
  | AddMenuItemAction
  | UpdateMenuItemAction
  | RemoveMenuItemAction;

export interface AuthState {
  user: UserDto;
  restaurant: RestaurantDto;
  menu: MenuDto;
}

const initialState = {
  user: null,
  restaurant: null,
  menu: null,
};

export default function authReducer(
  state: AuthState = { ...initialState },
  action: AuthAction
): AuthState {
  if (action.type === LOGIN) {
    return {
      ...state,
      user: action.payload.user,
    };
  }

  if (action.type === LOGOUT) {
    return {
      ...state,
      user: null,
      restaurant: null,
      menu: null,
    };
  }

  if (action.type === SET_AUTH_RESTAURANT) {
    return {
      ...state,
      restaurant: action.payload.restaurant,
    };
  }

  if (action.type === SET_AUTH_RESTAURANT_MENU) {
    return {
      ...state,
      menu: action.payload.menu,
    };
  }

  if (action.type === UPDATE_RESTAURANT_DETAILS) {
    return {
      ...state,
      restaurant: {
        ...state.restaurant,
        ...action.payload.request,
      },
    };
  }

  if (action.type === UPDATE_USER_DETAILS) {
    return {
      ...state,
      user: {
        ...state.user,
        ...action.payload.command,
      },
    };
  }

  if (action.type === ADD_MENU_CATEGORY) {
    return {
      ...state,
      menu: {
        ...state.menu,
        categories: [action.payload.category, ...state.menu.categories],
      },
    };
  }

  if (action.type === REMOVE_MENU_CATEGORY) {
    return {
      ...state,
      menu: {
        ...state.menu,
        categories: state.menu.categories.filter((category) => {
          return category.name !== action.payload.categoryName
        }),
      },
    };
  }

  if (action.type === ADD_MENU_ITEM) {
    return {
      ...state,
      menu: {
        ...state.menu,
        categories: state.menu.categories.map((category) => {
          if (category.name !== action.payload.category) {
            return category;
          }

          return {
            ...category,
            items: [...category.items, action.payload.item],
          };
        }),
      },
    };
  }

  if (action.type === UPDATE_MENU_ITEM) {
    return {
      ...state,
      menu: {
        ...state.menu,
        categories: state.menu.categories.map((category) => {
          if (category.name !== action.payload.categoryName) {
            return category;
          }

          return {
            ...category,
            items: category.items.map((item) => {
              if (item.name !== action.payload.oldItemName) {
                return item;
              }

              return action.payload.item;
            }),
          };
        }),
      },
    };
  }

  if (action.type === REMOVE_MENU_ITEM) {
    return {
      ...state,
      menu: {
        ...state.menu,
        categories: state.menu.categories.map((category) => {
          if (category.name !== action.payload.categoryName) {
            return category;
          }

          return {
            ...category,
            items: category.items.filter((item) => {
              return item.name !== action.payload.itemName;
            }),
          };
        }),
      },
    };
  }

  return state;
}
