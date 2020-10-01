import { ServerResponse } from "http";

export const redirect = (res: ServerResponse, location: string) => {
  res
    .writeHead(307, {
      Location: location,
    })
    .end();
};

export const loadScript = (src: string): Promise<void> => {
  return new Promise((resolve) => {
    if (document.querySelector(`script[src="${src}"]`) !== null) {
      resolve();
      return;
    }

    const script = document.createElement("script");
    script.src = src;
    script.addEventListener("load", () => resolve());
    document.body.appendChild(script);
  });
};
