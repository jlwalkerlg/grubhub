import { NextApiRequest, NextApiResponse } from "next";
import Axios from "axios";
import { AuthData } from "~/api/AuthApi";
import { getSignInCookies } from "~/helpers/auth";
import { DataEnvelope } from "~/api/dtos/DataEnvelope";

export default async (req: NextApiRequest, res: NextApiResponse) => {
  try {
    const response = await Axios.post<DataEnvelope<AuthData>>(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/login`,
      req.body
    );

    res.setHeader("set-cookie", getSignInCookies(response));

    return res.status(response.status).json(response.data);
  } catch (error) {
    return res.status(error.response.status).json(error.response.data || null);
  }
};
