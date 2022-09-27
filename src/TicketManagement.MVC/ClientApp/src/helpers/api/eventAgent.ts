import {
  CreateEventModel,
  EventArea,
  EventAreaWithSeats,
  EventAreaWithTotalSeats,
  EventEditListModel,
  EventMainPageModel,
  EventModel,
  EventSeat,
  EventSeatBuyList,
  EventWithDetails,
  EventWithLayouts,
  LayoutModel,
  PaginatedList,
  VenueModel,
} from "../../interfaces/event.interface";
import { createAxios, requests } from "./agentFactory";

const baseUrl = process.env.REACT_APP_EVENT_MANAGEMENT;

const eventAgentInstance = createAxios(baseUrl!);

const Events = {
  mainList: (params: URLSearchParams) =>
    requests.get<PaginatedList<EventMainPageModel>>(
      eventAgentInstance,
      "/events/main-page",
      {
        params,
      }
    ),
  editList: (params: URLSearchParams) =>
    requests.get<PaginatedList<EventEditListModel>>(
      eventAgentInstance,
      "/events/edit-list",
      {
        params,
      }
    ),
  withLayouts: (id: number) =>
    requests.get<EventWithLayouts>(
      eventAgentInstance,
      `/events/with-layouts/${id}`
    ),
  withDetails: (id: number) =>
    requests.get<EventWithDetails>(
      eventAgentInstance,
      `/events/with-details/${id}`
    ),
  create: (event: CreateEventModel) =>
    requests.post<EventModel>(eventAgentInstance, "/events", event),
  update: (event: EventModel) =>
    requests.put<void>(eventAgentInstance, `/events/${event.id}`, event),
  delete: (id: number) =>
    requests.delete<void>(eventAgentInstance, `/events/${id}`),
};

const EventAreas = {
  editList: (id: number, params: URLSearchParams) =>
    requests.get<PaginatedList<EventAreaWithTotalSeats>>(
      eventAgentInstance,
      `/events/${id}/event-areas/paginated-with-seat`,
      {
        params,
      }
    ),
  buyList: (id: number) =>
    requests.get<EventAreaWithSeats[]>(
      eventAgentInstance,
      `/events/${id}/event-areas/with-seats`
    ),
  eventArea: (id: number) =>
    requests.get<EventArea>(eventAgentInstance, `/event-areas/${id}`),
  update: (eventArea: EventArea) =>
    requests.put<void>(
      eventAgentInstance,
      `/event-areas/${eventArea.id}`,
      eventArea
    ),
  delete: (id: number) =>
    requests.delete<void>(eventAgentInstance, `/event-areas/${id}`),
};

const EventSeats = {
  editList: (id: number, params: URLSearchParams) =>
    requests.get<PaginatedList<EventSeat>>(
      eventAgentInstance,
      `/event-areas/${id}/event-seats/paginated`,
      {
        params,
      }
    ),
  buyList: (id: number) =>
    requests.get<EventSeatBuyList[]>(
      eventAgentInstance,
      `/event-areas/${id}/event-seats`
    ),
  eventSeat: (id: number) =>
    requests.get<EventSeat>(eventAgentInstance, `/event-seats/${id}`),
  update: (eventSeat: EventSeat) =>
    requests.put<void>(
      eventAgentInstance,
      `/event-seats/${eventSeat.id}`,
      eventSeat
    ),
  delete: (id: number) =>
    requests.delete<void>(eventAgentInstance, `/event-seats/${id}`),
};

const Venues = {
  venues: () => requests.get<VenueModel[]>(eventAgentInstance, "/venues"),
};

const Layouts = {
  layouts: (id: number) =>
    requests.get<LayoutModel[]>(eventAgentInstance, `/venues/${id}/layouts`),
};

const eventAgent = {
  Events,
  EventAreas,
  EventSeats,
  Venues,
  Layouts,
};

export default eventAgent;
