import Coordinates from "./geolocation/Coordinates";

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

const days = [
  "sunday",
  "monday",
  "tuesday",
  "wednesday",
  "thursday",
  "friday",
  "saturday",
];

export function getCurrentDayOfWeek() {
  return days[new Date().getDay()];
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
