import { makeAutoObservable, runInAction } from "mobx";
import eventAgent from "../helpers/api/eventAgent";
import orderAgent from "../helpers/api/orderAgent";
import {
  EventAreaWithSeats,
  EventSeatBuyList,
  EventWithLayouts,
} from "../interfaces/event.interface";
import { OrderWithDetailsModel } from "../interfaces/order.interface";

export default class EventPurchaseStore {
  selectedEvent: EventWithLayouts | undefined = undefined;

  eventAreas: EventAreaWithSeats[] = [];
  selectedEventArea: EventAreaWithSeats | undefined = undefined;

  eventSeats: EventSeatBuyList[] = [];
  selectedEventSeats: EventSeatBuyList[] = [];

  boughtTickets: OrderWithDetailsModel[] = [];

  loading = false;
  loadingInitial = true;

  constructor() {
    makeAutoObservable(this);
  }

  loadEventAreas = async (eventId: number) => {
    try {
      const eventAreasList = await eventAgent.EventAreas.buyList(eventId);
      runInAction(() => {
        this.eventAreas = eventAreasList;
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
    this.selectedEventArea = this.eventAreas.find((ea) => ea.id === id);
  };

  loadEventSeats = async (eventAreaId: number) => {
    try {
      const eventSeatsList = await eventAgent.EventSeats.buyList(eventAreaId);
      runInAction(() => {
        this.eventSeats = eventSeatsList;
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
    if (this.selectedEventSeats.find((es) => es.id === id) === undefined) {
      this.selectedEventSeats.push(this.eventSeats.find((es) => es.id === id)!);
    } else {
      const seats = this.selectedEventSeats.filter(
        (es) => es !== this.eventSeats.find((es) => es.id === id)!
      );
      this.selectedEventSeats = seats;
    }
  };

  buyTickets = async (id: string) => {
    const tickets = this.selectedEventSeats.map((seat) => {
      return {
        eventSeatId: seat.id,
        boughtDate: new Date(),
        price: this.selectedEventArea!.price,
        userId: id,
      };
    });

    this.boughtTickets = [];

    await Promise.all(
      tickets.map(async (ticket) => {
        const response = await orderAgent.Orders.buy(ticket);
        runInAction(() => {
          this.boughtTickets.push(response);
        });
      })
    );
  };
}
