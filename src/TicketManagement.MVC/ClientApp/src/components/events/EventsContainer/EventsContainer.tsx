import { Button, Card } from "semantic-ui-react";
import { Link } from "react-router-dom";
import {
  EventMainPageModel,
  PageInfo,
} from "../../../interfaces/event.interface";
import { EventsContainerItem } from "../EventsContainerItem/EventsContainerItem";
import cn from "classnames";
import styles from "./EventsContainer.module.css";

interface Props {
  events: EventMainPageModel[];
  pageInfo: PageInfo;
}

export const EventsContainer = ({ events, pageInfo }: Props): JSX.Element => {
  return (
    <>
      <div className={cn(styles["card-group"])}>
        <Card.Group>
          {events.map((eventItem) => (
            <Card key={eventItem.id} href={`/main/${eventItem.id}`}>
              <EventsContainerItem event={eventItem} />
            </Card>
          ))}
        </Card.Group>
      </div>
      <Button
        as={Link}
        to={`/main?pageIndex=${pageInfo.currentPage - 1}`}
        disabled={!pageInfo.hasPreviousPage}
        type="button"
        content="Previous"
      />
      <Button
        as={Link}
        to={`/main?pageIndex=${pageInfo.currentPage + 1}`}
        disabled={!pageInfo.hasNextPage}
        type="button"
        content="Next"
      />
    </>
  );
};
