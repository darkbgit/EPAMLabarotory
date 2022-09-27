import {
  Button,
  Container,
  Grid,
  GridColumn,
  Header,
  Image,
  Label,
} from "semantic-ui-react";
import { Link } from "react-router-dom";
import { EventWithDetails } from "../../../interfaces/event.interface";
import { format } from "date-fns";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../stores/store";

interface Props {
  event: EventWithDetails;
}

export const EventsDetailed = observer(({ event }: Props): JSX.Element => {
  const { userStore } = useStore();
  return (
    <>
      <Grid>
        <Grid.Row columns={2}>
          <GridColumn>
            <Image src={event.imageUrl} size={"medium"} ui={false} />
          </GridColumn>
          <GridColumn>
            <Header>{event.name}</Header>
            <Container>{event.description}</Container>
            <Container>
              {"Start at " +
                format(new Date(event.startDate), "Pp", {
                  locale: userStore.user?.locale,
                })}
            </Container>
            <Button
              disabled={event.freeSeats === 0}
              as="div"
              labelPosition="right"
            >
              <Button as={Link} to={`/main/${event.id}/buy`}>
                Buy ticket
              </Button>
              <Label basic pointing="left">
                {event.freeSeats}
              </Label>
            </Button>
          </GridColumn>
        </Grid.Row>
      </Grid>
    </>
  );
});
