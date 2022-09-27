export interface EventEditListModel {
  id: number;
  venueDescription: string;
  layoutDescription: string;
  name: string;
  startDate: Date;
  eventAreasCount: number;
}

export interface EventWithDetails {
  id: number;
  venueDescription: string;
  venueAddress: string;
  venuePhone: string;
  layoutDescription: string;
  description: string;
  imageUrl: string;
  name: string;
  startDate: Date;
  freeSeats: number;
}

export interface EventMainPageModel {
  id: number;
  venueDescription: string;
  layoutDescription: string;
  imageUrl: string;
  name: string;
  startDate: Date;
  freeSeats: number;
}

export interface PaginatedList<T> {
  items: T[];
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PageInfo {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface GetPaginatedSortSearchRequest {
  SortOrder?: string;
  SearchString?: string;
  PageSize: number;
  PageIndex?: number;
}

export interface EventModel {
  id: number;
  name: string;
  description: string;
  layoutId: number;
  startDate: Date;
  endDate: Date;
  imageUrl: string;
}

export interface CreateEventModel {
  name: string;
  description: string;
  layoutId: number;
  startDate: Date;
  endDate: Date;
  imageUrl: string;
}

export interface LayoutModel {
  id: number;
  venueId: number;
  description: string;
}

export interface EventWithLayouts {
  id: number;
  name: string;
  description: string;
  layoutId: number;
  startDate: Date;
  endDate: Date;
  imageUrl: string;
  venueTimeZoneId: string;
  venueId: number;
  layouts: LayoutModel[];
}

export interface EventMainPageModel {
  id: number;
  venueDescription: string;
  layoutDescription: string;
  imageUrl: string;
  name: string;
  startDate: Date;
  freeSeats: number;
}

export interface EventAreaWithTotalSeats {
  id: number;
  eventId: number;
  description: string;
  coordX: number;
  coordY: number;
  price: number;
  totalSeats: number;
}

export interface EventArea {
  id: number;
  eventId: number;
  description: string;
  coordX: number;
  coordY: number;
  price?: number;
}

export interface EventAreaWithSeats {
  id: number;
  eventId: number;
  description: string;
  coordX: number;
  coordY: number;
  price: number;
  totalSeats: number;
  freeSeats: number;
}

export interface EventSeat {
  id: number;
  eventAreaId: number;
  row: number;
  number: number;
  state: number;
}

export interface EventSeatBuyList {
  id: number;
  row: number;
  number: number;
  state: number;
}

export interface VenueModel {
  id: number;
  description: string;
  address: string;
  phone: string;
  timeZoneId: string;
}
