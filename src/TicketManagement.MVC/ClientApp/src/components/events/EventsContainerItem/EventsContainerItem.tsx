import { Card, Image } from "semantic-ui-react";
import { EventMainPageModel } from "../../../interfaces/event.interface";
import { format } from "date-fns";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../stores/store";

interface Props {
  event: EventMainPageModel;
}

export const EventsContainerItem = observer(({ event }: Props): JSX.Element => {
  const { userStore } = useStore();
  return (
    <>
      <Image src={event.imageUrl} wrapped ui={false} />
      <Card.Content>
        <Card.Header>{event.name}</Card.Header>
        <Card.Description>
          {"Start at " +
            format(new Date(event.startDate), "Pp", {
              locale: userStore.user?.locale,
            })}
        </Card.Description>
        <Card.Description>
          {event.venueDescription}, {event.layoutDescription}
        </Card.Description>
      </Card.Content>
      <Card.Content extra>
        {event.freeSeats === 0 ? "Sold out" : `Free seats ${event.freeSeats}`}
      </Card.Content>
    </>
  );
});
