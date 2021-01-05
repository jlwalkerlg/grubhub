import { useRouter } from "next/router";
import useIsAppMounted from "~/services/useIsAppMounted";

export default function useIsRouterReady() {
  const isAppMounted = useIsAppMounted();
  const router = useRouter();

  if (!isAppMounted || typeof window === "undefined") {
    return false;
  }

  return window.location.search === "" || Object.keys(router.query).length > 0;
}
