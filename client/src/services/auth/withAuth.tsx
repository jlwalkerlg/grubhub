import { NextPage } from "next";

import useAuth from "~/store/auth/useAuth";

export const withAuth = <T extends {}>(Page: NextPage<T>): NextPage<T> => {
  return (props: T) => {
    const { isLoggedIn } = useAuth();

    if (!isLoggedIn) {
      return null;
    }

    return <Page {...props} />;
  };
};
