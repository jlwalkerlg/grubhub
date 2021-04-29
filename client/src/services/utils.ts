import { format, parse } from "date-fns";
import { upperFirst } from "lodash";
import { OpeningTimes, RestaurantDto } from "~/api/restaurants/useRestaurant";
import Coordinates from "./geolocation/Coordinates";
import { daysOfWeek, getDayOfWeek } from "./useDate";

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

export function isRestaurantOpen(restaurant: RestaurantDto) {
  const now = new Date();
  const dayOfWeek = getDayOfWeek(now);
  const openingHours = restaurant.openingTimes[dayOfWeek];

  const opensAt = parse(openingHours.open, "HH:mm", new Date());

  if (opensAt > now) return false;

  if (openingHours.close === null) return true;

  const closesAt = parse(openingHours.close, "HH:mm", new Date());

  return (
    closesAt.getTime() >
    now.getTime() + restaurant.estimatedDeliveryTimeInMinutes * 60 * 1000
  );
}

export function nextOpenDay(openingTimes: OpeningTimes) {
  const now = new Date();
  const dayOfWeek = now.getDay();

  const days = [
    ...daysOfWeek.slice(dayOfWeek),
    ...daysOfWeek.slice(0, dayOfWeek),
  ].slice(1);

  for (const day of days) {
    if (openingTimes[day].open !== null) {
      return upperFirst(day);
    }
  }

  return null;
}

type DateFormat = "dd/MM/yyyy" | "HH:mm:ss" | "HH:mm" | "dd/MM/yyyy HH:mm";

export function formatDate(date: Date, formatString: DateFormat) {
  return format(date, formatString);
}

export function formatAddress(
  line1: string,
  line2: string,
  city: string,
  postcode: string
) {
  return [line1, line2, city, postcode].filter((x) => x ?? false).join(", ");
}

export function megabytesToBytes(mb: number) {
  return mb * 2 ** 20;
}
