module.exports = {
  images: {
    domains: ["d30v2pzvrfyzpo.cloudfront.net"],
  },

  async redirects() {
    return [
      {
        source: "/dashboard",
        destination: "/dashboard/restaurant-details",
        permanent: true,
      },
    ];
  },
};
