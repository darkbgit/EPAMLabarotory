import { Page } from "../components";
import { EventSeatEditForm, EventSeatEditTable } from "../components/editFlow";
import { Loading } from "../layout/Loading/Loading";
import { useStore } from "../stores/store";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Link, useParams, useSearchParams } from "react-router-dom";
import { Button } from "semantic-ui-react";
import { EventAreasPerEditList } from "../helpers/AppSettings";

const EventSeatsModeratorPage = () => {
  const { eventStore } = useStore();
  const {
    eventSeatsEditList: eventSeats,
    editEventSeatMode,
    loadingInitial,
    loadEventSeats,
  } = eventStore;

  const { id, eventId } = useParams<{ eventId: string; id: string }>();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const idNumber = Number(id);

    const params = new URLSearchParams();
    params.append("pageSize", EventAreasPerEditList);
    if (searchParams.has("pageIndex")) {
      params.append("pageIndex", searchParams.get("pageIndex")!);
    }

    idNumber && loadEventSeats(idNumber, params);
  }, [searchParams, id, loadEventSeats]);

  if (loadingInitial) return <Loading />;

  return (
    <Page title="Event Seats list">
      {eventSeats && <EventSeatEditTable eventId={eventId} eventAreaId={id} />}
      {editEventSeatMode && <EventSeatEditForm />}
      <Button
        as={Link}
        to={`/edit-list/${eventId}/event-areas`}
        primary
        content="Back"
      />
    </Page>
  );
};

export default observer(EventSeatsModeratorPage);
