import { NextApiRequest, NextApiResponse } from "next";
import Axios from "axios";
import cookie from "cookie";
import jwt from "jsonwebtoken";
import { LoginResponse } from "~/api/AuthApi";

export default async (req: NextApiRequest, res: NextApiResponse) => {
  try {
    const response = await Axios.post<LoginResponse>(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/login`,
      req.body
    );

    const cookies: string[] = response.headers["set-cookie"];
    const authCookie = cookies.find((x) => x.startsWith("auth_token="));

    const authToken = authCookie.split(";")[0].split("=")[1];

    const decoded = jwt.decode(authToken);
    const expiry = decoded["exp"];

    const user = response.data.data;
    const authUserCacheToken = jwt.sign(user, process.env.JWT_SECRET);
    const authUserCacheCookie = cookie.serialize(
      "auth_jwt",
      authUserCacheToken,
      {
        expires: new Date(expiry * 1000),
        httpOnly: true,
        path: "/",
      }
    );

    res.setHeader("set-cookie", [authCookie, authUserCacheCookie]);

    return res.status(response.status).json(response.data);
  } catch (error) {
    return res.status(error.response.status).json(error.response.data || null);
  }
};
