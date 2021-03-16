import { useQuery } from "react-query";

export default function useIsAppMounted() {
  const { data } = useQuery("isAppMounted", () => true, {
    staleTime: Infinity,
    cacheTime: Infinity,
  });

  return Boolean(data);
}
