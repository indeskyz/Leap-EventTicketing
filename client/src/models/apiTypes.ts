export interface ApiResponse<T> {
  items: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
}

export interface Event {
  id: string;
  name: string;
  startDate: string;
  endDate: string;
  description: string;
  location: string;
}

export interface EventSalesSummary {
  eventId: string;
  eventName: string;
  ticketsSold: number;
  totalRevenue: number;
}

export interface EventsQueryParams {
  days: number;
  pageNumber?: number;
  pageSize?: number;
  sortField?: 'name' | 'startDate';
  sortDirection?: 'asc' | 'desc';
}

export interface TopEventsQueryParams {
  count?: number;
}