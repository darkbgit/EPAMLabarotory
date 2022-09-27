import { Page } from "../components";
import { EventAreaEditForm, EventAreaEditTable } from "../components/editFlow";
import { Loading } from "../layout/Loading/Loading";
import { useStore } from "../stores/store";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Link, useParams, useSearchParams } from "react-router-dom";
import { Button } from "semantic-ui-react";
import { EventSeatsPerEditList } from "../helpers/AppSettings";

const EventAreasModeratorPage = () => {
  const { eventStore } = useStore();
  const {
    eventAreasEditList: eventAreas,
    editEventAreaMode,
    loadingInitial,
    loadEventAreas,
  } = eventStore;

  const { id } = useParams<{ id: string }>();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const idNumber = Number(id);

    const params = new URLSearchParams();
    params.append("pageSize", EventSeatsPerEditList);
    if (searchParams.has("pageIndex")) {
      params.append("pageIndex", searchParams.get("pageIndex")!);
    }

    idNumber && loadEventAreas(idNumber, params);
  }, [searchParams, id, loadEventAreas]);

  if (loadingInitial) return <Loading />;

  return (
    <Page title="Event Areas list">
      {eventAreas && <EventAreaEditTable eventId={id} />}
      {editEventAreaMode && <EventAreaEditForm />}
      <Button as={Link} to={`/edit-list`} primary content="Back" />
    </Page>
  );
};

export default observer(EventAreasModeratorPage);
