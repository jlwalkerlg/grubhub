import cookie from "cookie";
import jwt from "jsonwebtoken";
import { UserDto } from "~/api/dtos/UserDto";
import { NextPageContext } from "next";
import { AxiosResponse } from "axios";
import { LoginResponse } from "~/api/AuthApi";

export const getUserFromContext = async (
  context: NextPageContext
): Promise<UserDto> => {
  const authUserCacheToken = cookie.parse(context.req.headers.cookie || "")[
    "auth_jwt"
  ];
  if (!authUserCacheToken) return null;

  try {
    const user: UserDto = jwt.verify(
      authUserCacheToken,
      process.env.JWT_SECRET
    ) as UserDto;

    return user;
  } catch (e) {
    context.res.setHeader("Set-Cookie", [
      cookie.serialize("auth_token", "", {
        expires: new Date(0),
      }),
      cookie.serialize("auth_jwt", "", {
        expires: new Date(0),
      }),
    ]);

    return null;
  }
};

export const getSignInCookies = (response: AxiosResponse<LoginResponse>) => {
  const cookies: string[] = response.headers["set-cookie"];
  const authCookie = cookies.find((x) => x.startsWith("auth_token="));

  const authToken = authCookie.split(";")[0].split("=")[1];

  const decoded = jwt.decode(authToken);
  const expiry = decoded["exp"];

  const user = response.data.data;
  const authUserCacheToken = jwt.sign(user, process.env.JWT_SECRET);
  const authUserCacheCookie = cookie.serialize("auth_jwt", authUserCacheToken, {
    expires: new Date(expiry * 1000),
    httpOnly: true,
    path: "/",
  });

  return [authCookie, authUserCacheCookie];
};
