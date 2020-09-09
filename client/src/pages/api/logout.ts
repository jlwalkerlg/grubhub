import { NextApiRequest, NextApiResponse } from "next";
import { clearAuthCookies } from "~/helpers/auth";

export default async (req: NextApiRequest, res: NextApiResponse) => {
  clearAuthCookies(res);

  return res.status(200).send(null);
};
