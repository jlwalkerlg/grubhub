import { ServerResponse } from "http";

export const redirect = (res: ServerResponse, location: string) => {
  res
    .writeHead(307, {
      Location: location,
    })
    .end();
};
