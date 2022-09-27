import { Button, Table } from "semantic-ui-react";
import { EventEditTableItem } from "../EventEditTableItem/EventEditTableItem";
import { EventEditTableProps } from "./EventEditTable.props";
import cn from "classnames";
import styles from "./EventEditTable.module.css";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

export const EventEditTable = observer(
  ({ renderItem }: EventEditTableProps) => {
    const { eventStore } = useStore();
    const {
      selectedEvent,
      eventsEditList: events,
      createEventMode,
      editEventMode,
      setCreateEventMode,
      pageInfo,
    } = eventStore;

    return (
      <>
        <Button
          primary
          onClick={() => setCreateEventMode(!createEventMode)}
          content="Create new event"
          type="button"
          className={cn({
            [styles["button-create-mode"]]: createEventMode,
            [styles["button-edit-mode"]]: editEventMode,
          })}
        />
        <Table>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Venue Description</Table.HeaderCell>
              <Table.HeaderCell>Layout Description</Table.HeaderCell>
              <Table.HeaderCell>Name</Table.HeaderCell>
              <Table.HeaderCell>Start date</Table.HeaderCell>
              <Table.HeaderCell>Number of EventAreas</Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body
            className={cn({
              [styles["table-create-mode"]]: createEventMode,
            })}
          >
            {events.map((eventItem) => (
              <Table.Row
                key={eventItem.id}
                className={cn({
                  [styles["table-row-edit-mode"]]:
                    selectedEvent && eventItem.id === selectedEvent?.id,
                })}
              >
                {renderItem ? (
                  renderItem(eventItem)
                ) : (
                  <EventEditTableItem event={eventItem} />
                )}
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
        <Button
          as={Link}
          to={`/edit-list?pageIndex=${pageInfo.currentPage - 1}`}
          disabled={!pageInfo.hasPreviousPage}
          type="button"
          content="Previous"
        />
        <Button
          as={Link}
          to={`/edit-list?pageIndex=${pageInfo.currentPage + 1}`}
          disabled={!pageInfo.hasNextPage}
          type="button"
          content="Next"
        />
      </>
    );
  }
);
