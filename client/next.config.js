module.exports = {
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
