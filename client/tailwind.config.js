const defaults = require("tailwindcss/defaultTheme");
const colors = require("tailwindcss/colors");

const red = colors.red;

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
  content: ["./src/views/**/*.tsx", "./src/components/**/*.tsx"],
  theme: {
    // colors: {
    //   ...colors,
    //   current: "currentColor",
    // },
    container: {
      center: true,
      padding: defaults.spacing["4"],
    },
    extend: {
      inset: {
        100: "100%",
      },
      colors: {
        current: "currentColor",
        primary: {
          ...red,
          DEFAULT: red["700"],
          darker: red["800"],
          disabled: "rgba(197,48,48,0.58)",
        },
        secondary: green,
        green: green,
      },
    },
  },
  plugins: [],
};
