import { Page } from "../components";
import { Loading } from "../layout/Loading/Loading";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import eventAgent from "../helpers/api/eventAgent";
import { EventWithDetails } from "../interfaces/event.interface";
import { EventsDetailed } from "../components/events/EventDetailed/EventDetailed";

const EventDetailsMainPage = () => {
  const [event, setEvent] = useState<EventWithDetails | undefined>(undefined);
  const [loading, setLoading] = useState(false);
  const { id } = useParams();

  useEffect(() => {
    const idNumber = Number(id);

    const doGetEvent = async (id: number) => {
      const idNumber = Number(id);
      const event = await eventAgent.Events.withDetails(idNumber);
      setEvent(event);
    };

    doGetEvent(idNumber);
    setLoading(false);
  }, [id]);

  if (loading) return <Loading />;

  return (
    <Page title="Event Detailst">
      {event && <EventsDetailed event={event} />}
    </Page>
  );
};

export default EventDetailsMainPage;
