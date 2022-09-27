import styles from "./EventEditTableItem.module.css";
import { EventItemProps } from "./EventEditTableItem.props";
import cn from "classnames";
import { Button, Table } from "semantic-ui-react";
import { SyntheticEvent, useState } from "react";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { format } from "date-fns";

export const EventEditTableItem = observer(
  ({ event }: EventItemProps): JSX.Element => {
    const { eventStore } = useStore();
    const { openEditForm, deleteEvent, editEventMode, loading } = eventStore;

    const [target, setTarget] = useState("");

    const handleDeleteEvent = (
      e: SyntheticEvent<HTMLButtonElement>,
      id: number
    ) => {
      setTarget(e.currentTarget.name);
      deleteEvent(id);
    };

    return (
      <>
        <Table.Cell>{event.venueDescription}</Table.Cell>
        <Table.Cell>{event.layoutDescription}</Table.Cell>
        <Table.Cell>{event.name}</Table.Cell>
        <Table.Cell>{format(event.startDate, "yyyy-MM-dd HH:mm")}</Table.Cell>
        <Table.Cell>
          <Link to={`/edit-list/${event.id}/event-areas`}>
            {event.eventAreasCount}
          </Link>
        </Table.Cell>
        <Table.Cell>
          <Button
            className={cn({
              [styles["edit-mode"]]: editEventMode,
            })}
            onClick={() => openEditForm(event.id)}
            content="Edit"
          />
        </Table.Cell>
        <Table.Cell>
          <Button
            name={event.id.toString()}
            loading={loading && target === event.id.toString()}
            color="red"
            content="Delete"
            onClick={(e) => handleDeleteEvent(e, event.id)}
            className={cn({
              [styles["edit-mode"]]: editEventMode,
            })}
          />
        </Table.Cell>
      </>
    );
  }
);
