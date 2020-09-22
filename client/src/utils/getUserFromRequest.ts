import cookie from "cookie";
import { IncomingMessage } from "http";
import { UserDto } from "~/api/users/UserDto";

export const getUserFromRequest = (req: IncomingMessage) => {
  const cookies = cookie.parse(req.headers.cookie || "");
  return JSON.parse(cookies["auth_data"] || null) as UserDto;
};
