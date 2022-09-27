import { makeAutoObservable, runInAction } from "mobx";
import eventAgent from "../helpers/api/eventAgent";
import {
  CreateEventModel,
  EventArea,
  EventAreaWithTotalSeats,
  EventEditListModel,
  EventModel,
  EventSeat,
  EventWithLayouts,
  PageInfo,
} from "../interfaces/event.interface";

export default class EventStore {
  pageInfo: PageInfo = {
    currentPage: 0,
    pageSize: 0,
    totalCount: 0,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  };

  eventsEditRegistry = new Map<number, EventEditListModel>();
  selectedEvent: EventWithLayouts | undefined = undefined;
  createEventMode = false;
  editEventMode = false;

  eventAreasEditRegistry = new Map<number, EventAreaWithTotalSeats>();
  selectedEventArea: EventArea | undefined = undefined;
  editEventAreaMode = false;

  eventSeatsEditRegistry = new Map<number, EventSeat>();
  selectedEventSeat: EventSeat | undefined = undefined;
  editEventSeatMode = false;

  loading = false;
  loadingInitial = true;

  constructor() {
    makeAutoObservable(this);
  }

  get eventsEditList() {
    return Array.from(this.eventsEditRegistry.values());
  }

  get eventAreasEditList() {
    return Array.from(this.eventAreasEditRegistry.values());
  }

  get eventSeatsEditList() {
    return Array.from(this.eventSeatsEditRegistry.values());
  }

  setEvent = (event: EventEditListModel) => {
    event.startDate = new Date(event.startDate);
    this.eventsEditRegistry.set(event.id, event);
  };

  loadEvents = async (params: URLSearchParams) => {
    try {
      const pagedEvents = await eventAgent.Events.editList(params);
      runInAction(() => {
        const pageInfo: PageInfo = pagedEvents as PageInfo;
        this.pageInfo = pageInfo;
        this.eventsEditRegistry.clear();
        pagedEvents.items.forEach((event) => {
          this.setEvent(event);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  updateEvent = async (event: EventModel) => {
    this.loading = true;
    try {
      await eventAgent.Events.update(event);
      runInAction(() => {
        if (event.id) {
          let updatedEvent = {
            ...this.eventsEditRegistry.get(event.id),
            ...event,
          };
          this.eventsEditRegistry.set(
            event.id,
            updatedEvent as EventEditListModel
          );
        }
        this.selectedEvent = undefined;
        this.editEventMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  createEvent = async (event: CreateEventModel) => {
    this.loading = true;
    try {
      await eventAgent.Events.create(event);
      runInAction(() => {
        this.createEventMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteEvent = async (id: number) => {
    this.loading = true;
    try {
      await eventAgent.Events.delete(id);
      runInAction(() => {
        this.eventsEditRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  selectEvent = async (id: number) => {
    this.loading = true;
    try {
      const event = await eventAgent.Events.withLayouts(id);
      runInAction(() => {
        this.selectedEvent = event;
        this.loading = false;
      });
      return event;
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setSelectedEvent = (event: EventWithLayouts) => {
    this.selectedEvent = event;
  };

  setCreateEventMode = (state: boolean) => {
    this.createEventMode = state;
  };

  cancelSelectEvent = () => {
    this.selectedEvent = undefined;
  };

  openEditForm = async (id: number) => {
    await this.selectEvent(id);
    runInAction(() => {
      this.editEventMode = true;
    });
  };

  closeEditForm = () => {
    this.cancelSelectEvent();
    this.editEventMode = false;
  };

  openEditEventAreaForm = async (id: number) => {
    await this.selectEventArea(id);
    runInAction(() => {
      this.editEventAreaMode = true;
    });
  };

  closeEditEventAreaForm = () => {
    this.selectedEventArea = undefined;
    this.editEventAreaMode = false;
  };

  loadEventAreas = async (eventId: number, params: URLSearchParams) => {
    try {
      const pagedEventAreas = await eventAgent.EventAreas.editList(
        eventId,
        params
      );
      runInAction(() => {
        const pageInfo: PageInfo = pagedEventAreas as PageInfo;
        this.pageInfo = pageInfo;
        this.eventAreasEditRegistry.clear();
        pagedEventAreas.items.forEach((eventArea) => {
          this.eventAreasEditRegistry.set(eventArea.id, eventArea);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  selectEventArea = async (id: number) => {
    this.loading = true;
    try {
      const eventArea = await eventAgent.EventAreas.eventArea(id);
      runInAction(() => {
        this.selectedEventArea = eventArea;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  updateEventArea = async (eventArea: EventArea) => {
    this.loading = true;
    try {
      await eventAgent.EventAreas.update(eventArea);
      runInAction(() => {
        if (eventArea.id) {
          let updatedEventArea = {
            ...this.eventAreasEditRegistry.get(eventArea.id),
            ...eventArea,
          };
          this.eventAreasEditRegistry.set(
            eventArea.id,
            updatedEventArea as EventAreaWithTotalSeats
          );
        }
        this.selectedEventArea = undefined;
        this.editEventAreaMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteEventArea = async (id: number) => {
    this.loading = true;
    try {
      await eventAgent.EventAreas.delete(id);
      runInAction(() => {
        this.eventAreasEditRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  openEditEventSeatForm = async (id: number) => {
    await this.selectEventSeat(id);
    runInAction(() => {
      this.editEventSeatMode = true;
    });
  };

  closeEditEventSeatForm = () => {
    this.selectedEventSeat = undefined;
    this.editEventSeatMode = false;
  };

  loadEventSeats = async (eventAreaId: number, params: URLSearchParams) => {
    try {
      const pagedEventSeats = await eventAgent.EventSeats.editList(
        eventAreaId,
        params
      );
      runInAction(() => {
        const pageInfo: PageInfo = pagedEventSeats as PageInfo;
        this.pageInfo = pageInfo;
        this.eventSeatsEditRegistry.clear();
        pagedEventSeats.items.forEach((eventSeat) => {
          this.eventSeatsEditRegistry.set(eventSeat.id, eventSeat);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  selectEventSeat = async (id: number) => {
    this.loading = true;
    try {
      const eventSeat = await eventAgent.EventSeats.eventSeat(id);
      runInAction(() => {
        this.selectedEventSeat = eventSeat;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  updateEventSeat = async (eventSeat: EventSeat) => {
    this.loading = true;
    try {
      await eventAgent.EventSeats.update(eventSeat);
      runInAction(() => {
        if (eventSeat.id) {
          let updatedEventSeat = {
            ...this.eventSeatsEditRegistry.get(eventSeat.id),
            ...eventSeat,
          };
          this.eventSeatsEditRegistry.set(
            eventSeat.id,
            updatedEventSeat as EventSeat
          );
        }
        this.selectedEventSeat = undefined;
        this.editEventSeatMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteEventSeat = async (id: number) => {
    this.loading = true;
    try {
      await eventAgent.EventSeats.delete(id);
      runInAction(() => {
        this.eventSeatsEditRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
