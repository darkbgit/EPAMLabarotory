import { EventModel } from "../../../../interfaces/event.interface";

export interface EventCreateFormProps {
  createEvent: (event: EventModel) => void;
}
