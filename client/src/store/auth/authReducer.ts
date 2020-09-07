export const LOGIN = "AUTH_LOGIN";
export const LOGOUT = "AUTH_LOGOUT";

interface User {
  id: string;
  name: string;
  email: string;
  password: string;
  role: string;
}

interface State {
  isLoggedIn: boolean;
  user: User;
}

const initialState = {
  isLoggedIn: false,
  user: null,
};

export default function (state: State = initialState, action) {
  if (action.type === LOGIN) {
    return {
      isLoggedIn: true,
      user: action.payload.user,
    };
  }

  if (action.type === LOGOUT) {
    return {
      isLoggedIn: false,
      user: null,
    };
  }

  return state;
}
