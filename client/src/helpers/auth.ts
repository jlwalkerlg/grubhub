import { NextPageContext } from "next";
import cookie from "cookie";
import jwt from "jsonwebtoken";
import { AxiosResponse } from "axios";
import { AuthData } from "~/api/AuthApi";
import { ServerResponse } from "http";
import { DataEnvelope } from "~/api/dtos/DataEnvelope";

export const getAuthToken = (context: NextPageContext) => {
  return cookie.parse(context.req.headers.cookie || "")["auth_token"] || null;
};

export const getAuthDataFromContext = async (
  context: NextPageContext
): Promise<AuthData> => {
  const authUserCacheToken = cookie.parse(context.req.headers.cookie || "")[
    "auth_jwt"
  ];
  if (!authUserCacheToken) return null;

  try {
    return jwt.verify(authUserCacheToken, process.env.JWT_SECRET) as AuthData;
  } catch (e) {
    clearAuthCookies(context.res);
    return null;
  }
};

export const getSignInCookies = (
  response: AxiosResponse<DataEnvelope<AuthData>>
) => {
  const cookies: string[] = response.headers["set-cookie"];
  const authCookie = cookies.find((x) => x.startsWith("auth_token="));

  const authToken = authCookie.split(";")[0].split("=")[1];

  const decoded = jwt.decode(authToken);
  const expiry = decoded["exp"];

  const data: AuthData = response.data.data;
  const authUserCacheToken = jwt.sign(data, process.env.JWT_SECRET);
  const authUserCacheCookie = cookie.serialize("auth_jwt", authUserCacheToken, {
    expires: new Date(expiry * 1000),
    httpOnly: true,
    path: "/",
  });

  return [authCookie, authUserCacheCookie];
};

export const clearAuthCookies = (res: ServerResponse) => {
  res.setHeader("Set-Cookie", [
    cookie.serialize("auth_token", "", {
      expires: new Date(0),
      httpOnly: true,
      path: "/",
    }),
    cookie.serialize("auth_jwt", "", {
      expires: new Date(0),
      httpOnly: true,
      path: "/",
    }),
  ]);
};
