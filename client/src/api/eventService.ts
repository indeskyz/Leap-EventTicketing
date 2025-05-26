import api from './api';
import type { AxiosError, AxiosResponse } from 'axios';

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

export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export type SortDirection = 'asc' | 'desc';
export type EventSortField = 'name' | 'startDate';

export const fetchEvents = async (
  page: number = 1,
  pageSize: number = 10,
  sortField?: EventSortField,
  sortDirection?: SortDirection
): Promise<PaginatedResponse<Event>> => {
  try {
    const response: AxiosResponse<PaginatedResponse<Event>> = await api.get('/events', {
      params: {
        page,
        pageSize,
        sortField,
        sortDirection,
      },
    });
    return response.data;
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};

export const fetchTopEventsBySales = async (
  limit: number = 5
): Promise<PaginatedResponse<SalesSummary>> => {
  try {
    const response: AxiosResponse<PaginatedResponse<SalesSummary>> = await api.get('/events/top-sales', {
      params: { limit },
    });
    return response.data;
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};

const handleApiError = (error: AxiosError): never => {
  if (error.response) {
    throw new Error(
      `API Error: ${error.response.status} - ${(error.response.data as any)?.message || 'Unknown error'}`
    );
  }
  if (error.request) {
    throw new Error('Network Error: Could not connect to the server');
  }
  throw new Error(`Request Error: ${error.message}`);
};