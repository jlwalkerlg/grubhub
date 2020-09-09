import { GetServerSideProps, GetServerSidePropsContext } from "next";
import Axios from "axios";
import cookie from "cookie";
import { GetAuthUserResponse } from "~/api/AuthApi";
import { UserDto } from "~/api/dtos/UserDto";
import { initializeStore, State } from "~/store/store";
import { UserRole } from "~/store/auth/User";

export { Home as default } from "~/views/Home/Home";

export const getServerSideProps: GetServerSideProps = async (
  context: GetServerSidePropsContext
) => {
  const user = await getUser(context);

  let state: State = null;
  if (user !== null) {
    const store = initializeStore();
    state = store.getState();
    state.auth.user = {
      id: user.id,
      name: user.name,
      email: user.email,
      role: UserRole[user.role],
    };
  }

  return {
    props: {
      initialReduxState: state,
    },
  };
};

const getUser = async (
  context: GetServerSidePropsContext
): Promise<UserDto> => {
  const token = cookie.parse(context.req.headers.cookie || "")["auth_token"];
  if (!token) return null;

  try {
    const response = await Axios.get<GetAuthUserResponse>(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/auth/user`,
      {
        headers: {
          Cookie: `auth_token=${token}`,
        },
        withCredentials: true,
      }
    );

    return response.data.data;
  } catch (e) {
    context.res.setHeader(
      "Set-Cookie",
      cookie.serialize("auth_token", "", {
        expires: new Date(0),
      })
    );

    return null;
  }
};
