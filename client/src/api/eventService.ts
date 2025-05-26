import type { EventsQueryParams, PaginatedResponse, TopEventsQueryParams, EventSalesSummary, ApiResponse, TicketSalesDto } from '@/models/apiTypes';
import { serializeParams } from '@/utils/paramsSerializer';
import type { AxiosError } from 'axios';
import api, { handleApiError } from './api';

export const fetchUpcomingEvents = async (
  params: EventsQueryParams
): Promise<PaginatedResponse<Event>> => {
  try {
    const { days, ...queryParams } = params;
    const queryString = serializeParams(queryParams);
    const url = `/api/events/upcoming/${days}${queryString ? `?${queryString}` : ''}`;

    const response = await api.get<PaginatedResponse<Event>>(url);

    return {
      items: response.items,
      pageNumber: response.pageNumber,
      pageSize: response.pageSize,
      totalCount: response.totalCount
    };
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};


export const fetchTopEventsBySales = async (
  params: TopEventsQueryParams = { count: 5 }
): Promise<EventSalesSummary[]> => {
  try {
    const queryString = serializeParams(params);
    const url = `/api/tickets/top/sales${queryString ? `?${queryString}` : ''}`;
    const response = await api.get<ApiResponse<EventSalesSummary[]>>(url);
    return response.items;
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};

export const fetchTopEventsByRevenue = async (
  params: TopEventsQueryParams = { count: 5 }
): Promise<EventSalesSummary[]> => {
  try {
    const queryString = serializeParams(params);
    const url = `/api/tickets/top/revenue${queryString ? `?${queryString}` : ''}`;
    const response = await api.get<ApiResponse<EventSalesSummary[]>>(url);
    return response.items;
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};

export const fetchTicketsForEvent = async (
  eventId: string,
  params: EventsQueryParams
): Promise<PaginatedResponse<TicketSalesDto>> => {
  try {
    const queryString = serializeParams(params);
    const url = `/api/tickets/event/${eventId}${queryString ? `?${queryString}` : ''}`;
    const response = await api.get<PaginatedResponse<TicketSalesDto>>(url);
    return response;
  } catch (error) {
    throw handleApiError(error as AxiosError);
  }
};