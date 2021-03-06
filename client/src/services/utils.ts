import { padStart } from "lodash";
import { OpeningHours, OpeningTimes } from "~/api/restaurants/OpeningTimes";
import Coordinates from "./geolocation/Coordinates";
import { daysOfWeek } from "./useDate";

export async function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

export function haversine(p1: Coordinates, p2: Coordinates) {
  const R = 6371e3; // metres
  const φ1 = (p1.latitude * Math.PI) / 180; // φ, λ in radians
  const φ2 = (p2.latitude * Math.PI) / 180;
  const Δφ = ((p2.latitude - p1.latitude) * Math.PI) / 180;
  const Δλ = ((p2.longitude - p1.longitude) * Math.PI) / 180;

  const a =
    Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
    Math.cos(φ1) * Math.cos(φ2) * Math.sin(Δλ / 2) * Math.sin(Δλ / 2);
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

  const d = R * c; // in metres

  return d / 1000;
}

export function url(
  path: string,
  params: { [key: string]: string | string[] } = {}
) {
  if (Object.keys(params).length === 0) {
    return path;
  }

  return (
    path +
    "?" +
    Object.keys(params)
      .map((key) => `${key}=${params[key]}`)
      .join("&")
  );
}

export function isRestaurantOpen(openingHours: OpeningHours) {
  const currentDate = new Date();

  const openingDate = new Date();
  openingDate.setUTCHours(+openingHours.open.slice(0, 2));
  openingDate.setUTCMinutes(+openingHours.open.slice(3));

  if (openingDate > currentDate) return false;

  if (openingHours.close === null) return true;

  const closingDate = new Date();
  closingDate.setUTCHours(+openingHours.close.slice(0, 2));
  closingDate.setUTCMinutes(+openingHours.close.slice(3));

  return closingDate > currentDate;
}

export function nextOpenDay(openingTimes: OpeningTimes) {
  const currentDay = new Date().getDay();

  const days = [
    ...daysOfWeek.slice(currentDay),
    ...daysOfWeek.slice(0, currentDay),
  ];

  for (const day of days) {
    if (openingTimes[day].open !== null) {
      return day;
    }
  }

  return null;
}

type DateFormat = "dd/mm/yyyy" | "hh:mm:ss" | "hh:mm" | "dd/mm/yyyy hh:mm";

export function formatDate(date: Date, format: DateFormat = "dd/mm/yyyy") {
  if (format === "dd/mm/yyyy") {
    return (
      padStart(date.getDate().toString(), 2, "0") +
      "/" +
      padStart((date.getMonth() + 1).toString(), 2, "0") +
      "/" +
      date.getFullYear().toString()
    );
  }

  if (format === "hh:mm") {
    return (
      padStart(date.getHours().toString(), 2, "0") +
      ":" +
      padStart(date.getMinutes().toString(), 2, "0")
    );
  }

  if (format === "dd/mm/yyyy hh:mm") {
    return formatDate(date, "dd/mm/yyyy") + " " + formatDate(date, "hh:mm");
  }

  return (
    formatDate(date, "hh:mm") +
    ":" +
    padStart(date.getSeconds().toString(), 2, "0")
  );
}

export function formatAddress(
  line1: string,
  line2: string,
  city: string,
  postcode: string
) {
  return [line1, line2, city, postcode].filter((x) => x ?? false).join(", ");
}
