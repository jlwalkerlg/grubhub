import { useQuery } from "react-query";

export default function useIsAppMounted() {
  const { data } = useQuery<boolean>("isAppMounted", () => true, {
    staleTime: Infinity,
    initialStale: true,
    initialData: false,
  });

  return data;
}
