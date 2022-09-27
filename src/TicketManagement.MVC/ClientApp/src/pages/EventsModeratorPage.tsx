import { Page } from "../components";
import {
  EventCreateForm,
  EventEditForm,
  EventEditTable,
} from "../components/editFlow";
import { Loading } from "../layout/Loading/Loading";
import { useStore } from "../stores/store";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useSearchParams } from "react-router-dom";
import { EventsPerEditList } from "../helpers/AppSettings";

const EventsModeratorPage = () => {
  const { eventStore } = useStore();
  const {
    eventsEditList: events,
    createEventMode,
    editEventMode,
    loadingInitial,
    loadEvents,
  } = eventStore;

  const [searchParams] = useSearchParams();

  useEffect(() => {
    const params = new URLSearchParams();
    params.append("pageSize", EventsPerEditList);
    if (searchParams.has("pageIndex")) {
      params.append("pageIndex", searchParams.get("pageIndex")!);
    }

    loadEvents(params);
  }, [searchParams, loadEvents]);

  if (loadingInitial) return <Loading />;

  return (
    <Page title="Events list">
      {events && <EventEditTable />}

      {!editEventMode && createEventMode && <EventCreateForm />}

      {!createEventMode && editEventMode && <EventEditForm />}
    </Page>
  );
};

export default observer(EventsModeratorPage);
