import { useQuery } from "react-query";

export const daysOfWeek = [
  "sunday",
  "monday",
  "tuesday",
  "wednesday",
  "thursday",
  "friday",
  "saturday",
];

export function getDayOfWeek(date?: Date) {
  return daysOfWeek[(date ?? new Date()).getDay()];
}

export default function useDate() {
  const { data: date } = useQuery("useDate", () => new Date(), {
    staleTime: 30 * 1000,
    initialData: () => new Date(),
  });

  const dayOfWeek = getDayOfWeek(date);

  return { date, dayOfWeek };
}
