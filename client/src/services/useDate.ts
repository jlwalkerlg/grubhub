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

export default function useDate() {
  const { data: date } = useQuery("useDate", () => new Date(), {
    staleTime: 30 * 1000,
    initialData: () => new Date(),
  });

  const dayOfWeek = daysOfWeek[date.getDay()];

  return { date, dayOfWeek };
}
