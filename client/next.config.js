module.exports = {
  images: {
    domains: ["d3bvhdd3xj1ghi.cloudfront.net"],
  },

  async redirects() {
    return [
      {
        source: "/dashboard",
        destination: "/dashboard/active-orders",
        permanent: false,
      },
    ];
  },
};
