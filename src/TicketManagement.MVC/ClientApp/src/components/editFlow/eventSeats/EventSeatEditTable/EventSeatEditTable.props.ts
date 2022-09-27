import { EventSeat } from "../../../../interfaces/event.interface";

export interface EventSeatEditTableProps {
  renderItem?: (item: EventSeat) => JSX.Element;
  eventId: string | undefined;
  eventAreaId: string | undefined;
}
