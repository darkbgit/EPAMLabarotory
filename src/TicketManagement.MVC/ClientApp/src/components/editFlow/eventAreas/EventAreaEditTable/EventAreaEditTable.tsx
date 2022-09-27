import { Button, Table, TableFooter } from "semantic-ui-react";
import { EventAreaEditTableItem } from "../EventAreaEditTableItem/EventAreaEditTableItem";
import { EventAreaEditTableProps } from "./EventAreaEditTable.props";
import cn from "classnames";
import styles from "./EventAreaEditTable.module.css";
import { useStore } from "../../../../stores/store";
import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";

export const EventAreaEditTable = observer(
  ({ renderItem, eventId }: EventAreaEditTableProps) => {
    const { eventStore } = useStore();
    const {
      selectedEventArea,
      eventAreasEditList: eventAreas,
      editEventMode,
      pageInfo,
    } = eventStore;

    return (
      <div className={cn(styles["page"])}>
        <Table>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Description</Table.HeaderCell>
              <Table.HeaderCell>CoordX</Table.HeaderCell>
              <Table.HeaderCell>CoordY</Table.HeaderCell>
              <Table.HeaderCell>Price</Table.HeaderCell>
              <Table.HeaderCell>Number of Seats</Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body
            className={cn({
              [styles["table-edit-mode"]]: editEventMode,
            })}
          >
            {eventAreas.map((eventAreaItem) => (
              <Table.Row
                key={eventAreaItem.id}
                className={cn({
                  [styles["table-row-edit-mode"]]:
                    selectedEventArea &&
                    eventAreaItem.id === selectedEventArea?.id,
                })}
              >
                {renderItem ? (
                  renderItem(eventAreaItem)
                ) : (
                  <EventAreaEditTableItem eventArea={eventAreaItem} />
                )}
              </Table.Row>
            ))}
          </Table.Body>
          <TableFooter></TableFooter>
        </Table>
        <Button
          as={Link}
          to={`/edit-list/${eventId}/event-areas?pageIndex=${
            pageInfo.currentPage - 1
          }`}
          disabled={!pageInfo.hasPreviousPage}
          type="button"
          content="Previous"
        />
        <Button
          as={Link}
          to={`/edit-list/${eventId}/event-areas?pageIndex=${
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
