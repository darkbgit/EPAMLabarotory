import { Button, Table, TableFooter } from "semantic-ui-react";
import { EventSeatEditTableItem } from "../EventSeatEditTableItem/EventSeatEditTableItem";
import { EventSeatEditTableProps } from "./EventSeatEditTable.props";
import cn from "classnames";
import styles from "./EventSeatEditTable.module.css";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

export const EventSeatEditTable = observer(
  ({ renderItem, eventId, eventAreaId }: EventSeatEditTableProps) => {
    const { eventStore } = useStore();
    const {
      selectedEventSeat,
      eventSeatsEditList: eventSeats,
      editEventSeatMode,
      pageInfo,
    } = eventStore;

    return (
      <div className={cn(styles["page"])}>
        <Table>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Row</Table.HeaderCell>
              <Table.HeaderCell>Number</Table.HeaderCell>
              <Table.HeaderCell>State</Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body
            className={cn({
              [styles["table-edit-mode"]]: editEventSeatMode,
            })}
          >
            {eventSeats.map((eventSeatItem) => (
              <Table.Row
                key={eventSeatItem.id}
                className={cn({
                  [styles["table-row-edit-mode"]]:
                    selectedEventSeat &&
                    eventSeatItem.id === selectedEventSeat?.id,
                })}
              >
                {renderItem ? (
                  renderItem(eventSeatItem)
                ) : (
                  <EventSeatEditTableItem eventSeat={eventSeatItem} />
                )}
              </Table.Row>
            ))}
          </Table.Body>
          <TableFooter></TableFooter>
        </Table>
        <Button
          as={Link}
          to={`/edit-list/${eventId}/event-areas/${eventAreaId}/event-seats?pageIndex=${
            pageInfo.currentPage - 1
          }`}
          disabled={!pageInfo.hasPreviousPage}
          type="button"
          content="Previous"
        />
        <Button
          as={Link}
          to={`/edit-list/${eventId}/event-areas/${eventAreaId}/event-seats?pageIndex=${
            pageInfo.currentPage + 1
          }`}
          disabled={!pageInfo.hasNextPage}
          type="button"
          content="Next"
        />
      </div>
    );
  }
);
