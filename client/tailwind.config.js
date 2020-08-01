const defaults = require("tailwindcss/defaultTheme");

const green = {
  100: "#dbf5db",
  200: "#b7ebb7",
  300: "#94e292",
  400: "#70d86e",
  500: "#4cce4a",
  600: "#44b943",
  700: "#3da53b",
  800: "#2e7c2c",
  900: "#173e16",
};

module.exports = {
  purge: ["./views/**/*.tsx", "./components/**/*.tsx"],
  theme: {
    container: {
      center: true,
      padding: defaults.spacing["4"],
    },
    extend: {
      colors: {
        primary: {
          ...defaults.colors.red,
          default: defaults.colors.red["700"],
          darker: defaults.colors.red["800"],
        },
        secondary: green,
        green: green,
      },
    },
  },
  variants: {},
  plugins: [],
};
