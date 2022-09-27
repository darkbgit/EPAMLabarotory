import { Page } from "../components";
import { Loading } from "../layout/Loading/Loading";
import { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useSearchParams } from "react-router-dom";
import { EventsPerMainPage } from "../helpers/AppSettings";
import eventAgent from "../helpers/api/eventAgent";
import { EventMainPageModel, PageInfo } from "../interfaces/event.interface";
import { EventsContainer } from "../components/events/EventsContainer/EventsContainer";

const EventsMainPage = () => {
  const [searchParams] = useSearchParams();
  const [events, setEvents] = useState<EventMainPageModel[]>([]);

  const initPageInfo: PageInfo = {
    currentPage: 0,
    pageSize: 0,
    totalCount: 0,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  };
  const [pageInfo, setPageInfo] = useState<PageInfo>(initPageInfo);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const params = new URLSearchParams();
    params.append("pageSize", EventsPerMainPage);
    if (searchParams.has("pageIndex")) {
      params.append("pageIndex", searchParams.get("pageIndex")!);
    }
    const doGetEvents = async (params: URLSearchParams) => {
      setLoading(true);
      const pagedEvents = await eventAgent.Events.mainList(params);
      const pageInf = pagedEvents as PageInfo;
      setPageInfo(pageInf);
      setEvents(pagedEvents.items);
    };

    doGetEvents(params);
    setLoading(false);
  }, [searchParams]);

  if (loading) return <Loading />;

  return (
    <Page title="Upcoming Events">
      {events && <EventsContainer events={events} pageInfo={pageInfo} />}
    </Page>
  );
};

export default observer(EventsMainPage);
