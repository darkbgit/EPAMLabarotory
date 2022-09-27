import { EventEditListModel } from "../../../../interfaces/event.interface";

export interface EventEditTableProps {
  renderItem?: (item: EventEditListModel) => JSX.Element;
}
