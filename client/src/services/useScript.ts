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
        script.defer = true;
        script.addEventListener("load", () => resolve());
        document.head.appendChild(script);
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
