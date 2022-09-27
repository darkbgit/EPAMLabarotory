import styles from "./EventSeatEditTableItem.module.css";
import { EventSeatEditTableItemProps } from "./EventSeatEditTableItem.props";
import cn from "classnames";
import { Button, Table } from "semantic-ui-react";
import { SyntheticEvent, useState } from "react";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { eventSeatState } from "../../../../interfaces/options/eventSeatState";

export const EventSeatEditTableItem = observer(
  ({ eventSeat }: EventSeatEditTableItemProps): JSX.Element => {
    const { eventStore } = useStore();
    const {
      openEditEventSeatForm,
      deleteEventSeat,
      editEventSeatMode,
      loading,
    } = eventStore;

    const [target, setTarget] = useState("");

    const handleDeleteEvent = (
      e: SyntheticEvent<HTMLButtonElement>,
      id: number
    ) => {
      setTarget(e.currentTarget.name);
      deleteEventSeat(id);
    };

    return (
      <>
        <Table.Cell>{eventSeat.row}</Table.Cell>
        <Table.Cell>{eventSeat.number}</Table.Cell>
        <Table.Cell>
          {eventSeatState.find((s) => s.value === eventSeat.state)?.text}
        </Table.Cell>
        <Table.Cell>
          <Button
            className={cn({
              [styles["edit-mode"]]: editEventSeatMode,
            })}
            onClick={() => openEditEventSeatForm(eventSeat.id)}
            content="Edit"
          />
        </Table.Cell>
        <Table.Cell>
          <Button
            name={eventSeat.id.toString()}
            loading={loading && target === eventSeat.id.toString()}
            color="red"
            content="Delete"
            onClick={(e) => handleDeleteEvent(e, eventSeat.id)}
            className={cn({
              [styles["edit-mode"]]: editEventSeatMode,
            })}
          />
        </Table.Cell>
      </>
    );
  }
);
