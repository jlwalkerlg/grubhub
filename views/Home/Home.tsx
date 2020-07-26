import Head from "next/head";
import React, { FC } from "react";
import Layout from "~/components/Layout/Layout";

export const Home: FC = () => {
  return (
    <Layout title="Home">
      <h1 className="text-center uppercase">Home</h1>
    </Layout>
  );
};
