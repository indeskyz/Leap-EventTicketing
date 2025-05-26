// Basic API response structure
export interface ApiResponse<T> {
  items: T;
  success: boolean;
  message?: string;
}

// Paginated API response
export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
}

// Event model
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

// Query parameters
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