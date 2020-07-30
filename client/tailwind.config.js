module.exports = {
  purge: ["./views/**/*.tsx", "./components/**/*.tsx"],
  theme: {
    extend: {
      colors: {
        primary: {
          default: "#c53030",
          darker: "#9b2c2c",
        },
        green: {
          100: "#dbf5db",
          200: "#b7ebb7",
          300: "#94e292",
          400: "#70d86e",
          500: "#4cce4a",
          600: "#44b943",
          700: "#3da53b",
          800: "#2e7c2c",
          900: "#173e16",
        },
      },
    },
  },
  variants: {},
  plugins: [],
};
