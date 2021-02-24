module.exports = {
  images: {
    domains: ["d30v2pzvrfyzpo.cloudfront.net"],
  },

  async redirects() {
    return [
      {
        source: "/dashboard",
        destination: "/dashboard/orders",
        permanent: false,
      },
    ];
  },
};
