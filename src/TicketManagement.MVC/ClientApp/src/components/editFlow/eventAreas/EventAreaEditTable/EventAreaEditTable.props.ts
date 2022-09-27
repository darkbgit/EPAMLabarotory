import { EventAreaWithTotalSeats } from "../../../../interfaces/event.interface";

export interface EventAreaEditTableProps {
  renderItem?: (item: EventAreaWithTotalSeats) => JSX.Element;
  eventId: string | undefined;
}
