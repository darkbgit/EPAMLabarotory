export interface OrderBoughtModel {
  eventSeatId: number;
  userId: string;
  boughtDate?: Date;
  price: number;
}

export interface OrderWithDetailsModel {
  id: number;
  row: number;
  number: number;
  venueName: string;
  layoutName: string;
  eventName: string;
  eventAreaName: string;
  startDate: Date;
  price: number;
}
