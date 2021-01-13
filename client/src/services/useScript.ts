import { useCallback } from "react";
import { useQuery } from "react-query";

export default function useScript(src: string) {
  const { refetch } = useQuery(
    `script:${src}`,
    () => {
      return new Promise<void>((resolve) => {
        if (document.querySelector(`script[src="${src}"]`) !== null) {
          resolve();
          return;
        }

        const script = document.createElement("script");
        script.src = src;
        script.addEventListener("load", () => resolve());
        document.body.appendChild(script);
      });
    },
    {
      staleTime: Infinity,
      cacheTime: Infinity,
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false,
    }
  );

  const load = useCallback(() => refetch(), []);

  return { load };
}
