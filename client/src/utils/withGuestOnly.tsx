import { NextPage } from "next";
import { useRouter } from "next/router";
import useAuth from "~/store/auth/useAuth";

export const withGuestOnly = (Page: NextPage): NextPage => {
  return (props: any) => {
    const router = useRouter();

    const { isLoggedIn } = useAuth();

    if (isLoggedIn) {
      router.push("/");
      return null;
    }

    return <Page {...props} />;
  };
};
