import { NextApiRequest, NextApiResponse } from "next";
import Axios from "axios";
import { LoginResponse } from "~/api/AuthApi";

export default async (req: NextApiRequest, res: NextApiResponse) => {
  try {
    const response = await Axios.post<LoginResponse>(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/login`,
      req.body
    );

    const cookies: string[] = response.headers["set-cookie"];
    const authCookie = cookies.find((x) => x.startsWith("auth_token="));

    res.setHeader("set-cookie", authCookie);

    return res.status(response.status).json(response.data);
  } catch (error) {
    return res.status(error.response.status).json(error.response.data || null);
  }
};
