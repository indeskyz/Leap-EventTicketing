export interface Event {
  id: string;
  name: string;
  startDate: string;
  endDate: string;
  description: string;
  location: string;
}

export interface SalesSummary {
  eventId: string;
  eventName: string;
  ticketsSold: number;
  totalRevenue: number;
}

export interface Pagination {
  page: number;
  pageSize: number;
  totalItems: number;
}

export interface PaginatedResponse<T> {
  data: T[];
  pagination: Pagination;
}

export type SortDirection = 'asc' | 'desc';
export type EventSortField = 'name' | 'startDate';