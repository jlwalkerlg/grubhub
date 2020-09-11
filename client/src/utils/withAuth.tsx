import { NextPage } from "next";
import { useRouter } from "next/router";
import useAuth from "~/store/auth/useAuth";

export const withAuth = (Page: NextPage): NextPage => {
  return (props: any) => {
    const router = useRouter();

    const { isLoggedIn } = useAuth();

    if (!isLoggedIn) {
      router.push("/login");
      return null;
    }

    return <Page {...props} />;
  };
};
