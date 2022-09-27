import styles from "./EventAreaEditTableItem.module.css";
import { EventAreaEditTableItemProps } from "./EventAreaEditTableItem.props";
import cn from "classnames";
import { Button, Table } from "semantic-ui-react";
import { SyntheticEvent, useState } from "react";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

export const EventAreaEditTableItem = observer(
  ({ eventArea }: EventAreaEditTableItemProps): JSX.Element => {
    const { eventStore } = useStore();
    const {
      openEditEventAreaForm,
      deleteEventArea,
      editEventAreaMode,
      loading,
    } = eventStore;

    const [target, setTarget] = useState("");

    const handleDeleteEvent = (
      e: SyntheticEvent<HTMLButtonElement>,
      id: number
    ) => {
      setTarget(e.currentTarget.name);
      deleteEventArea(id);
    };

    return (
      <>
        <Table.Cell>{eventArea.description}</Table.Cell>
        <Table.Cell>{eventArea.coordX}</Table.Cell>
        <Table.Cell>{eventArea.coordY}</Table.Cell>
        <Table.Cell>{eventArea.price}</Table.Cell>
        <Table.Cell>
          <Link
            to={`/edit-list/${eventArea.eventId}/event-areas/${eventArea.id}/event-seats`}
          >
            {eventArea.totalSeats}
          </Link>
        </Table.Cell>
        <Table.Cell>
          <Button
            className={cn({
              [styles["edit-mode"]]: editEventAreaMode,
            })}
            onClick={() => openEditEventAreaForm(eventArea.id)}
            content="Edit"
          />
        </Table.Cell>
        <Table.Cell>
          <Button
            name={eventArea.id.toString()}
            loading={loading && target === eventArea.id.toString()}
            color="red"
            content="Delete"
            onClick={(e) => handleDeleteEvent(e, eventArea.id)}
            className={cn({
              [styles["edit-mode"]]: editEventAreaMode,
            })}
          />
        </Table.Cell>
      </>
    );
  }
);
