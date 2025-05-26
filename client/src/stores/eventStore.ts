import { defineStore } from 'pinia';
import { ref } from 'vue';
import {
  fetchUpcomingEvents,
  fetchTopEventsBySales,
  fetchTopEventsByRevenue,
  fetchTicketsForEvent,
} from '@/api/eventService';

import type {
  Event,
  EventSalesSummary,
  EventsQueryParams,
  TicketSalesDto,
  TopEventsQueryParams
} from '@/models/apiTypes';

export const useEventStore = defineStore('events', () => {
  const events = ref<Event[]>([]);
  const topSales = ref<EventSalesSummary[]>([]);
  const topRevenue = ref<EventSalesSummary[]>([]);
  const loading = ref(false);
  const error = ref<Error | null>(null);
  const pagination = ref({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });
  const tickets = ref<TicketSalesDto[]>([]);
  const ticketsLoading = ref(false);
  const ticketsError = ref<Error | null>(null);
  const ticketsPagination = ref({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });
  const sortField = ref<'name' | 'startDate' | null>(null);
  const sortDirection = ref<'asc' | 'desc' | null>(null);

  const setSorting = (field: 'name' | 'startDate', direction: 'asc' | 'desc') => {
    sortField.value = field;
    sortDirection.value = direction;
  };

  const loadUpcomingEvents = async (days: number) => {
    loading.value = true;
    error.value = null;
    try {
      const params: EventsQueryParams = {
        days,
        pageNumber: pagination.value.pageNumber,
        pageSize: pagination.value.pageSize
      };
      const response = await fetchUpcomingEvents(params);
      events.value = response.items as unknown as Event[];
      pagination.value = {
        pageNumber: response.pageNumber,
        pageSize: response.pageSize,
        totalCount: response.totalCount,
      };
    } catch (err) {
      error.value = err as Error;
    } finally {
      loading.value = false;
    }
  };

  const loadTopSales = async (count: number = 5) => {
    loading.value = true;
    error.value = null;
    try {
      const params: TopEventsQueryParams = { count };
      topSales.value = await fetchTopEventsBySales(params);
    } catch (err) {
      error.value = err as Error;
    } finally {
      loading.value = false;
    }
  };

  const loadTopRevenue = async (count: number = 5) => {
    loading.value = true;
    error.value = null;
    try {
      const params: TopEventsQueryParams = { count };
      topRevenue.value = await fetchTopEventsByRevenue(params);
    } catch (err) {
      error.value = err as Error;
    } finally {
      loading.value = false;
    }
  };

  const setPage = (page: number) => {
    pagination.value.pageNumber = page;
  };

  const setPageSize = (size: number) => {
    pagination.value.pageSize = size;
    pagination.value.pageNumber = 1;
  };

  const loadTicketsForEvent = async (eventId: string) => {
    if (!eventId) return;

    ticketsLoading.value = true;
    ticketsError.value = null;

    try {
      const params: EventsQueryParams = {
        pageNumber: ticketsPagination.value.pageNumber,
        pageSize: ticketsPagination.value.pageSize,
      };

      const response = await fetchTicketsForEvent(eventId, params);
      tickets.value = response.items as unknown as TicketSalesDto[];
      ticketsPagination.value = {
        pageNumber: response.pageNumber,
        pageSize: response.pageSize,
        totalCount: response.totalCount,
      };
    } catch (err) {
      ticketsError.value = err as Error;
    } finally {
      ticketsLoading.value = false;
    }
  };

  const setTicketsPage = (page: number) => {
    ticketsPagination.value.pageNumber = page;
  };

  const setTicketsPageSize = (size: number) => {
    ticketsPagination.value.pageSize = size;
    ticketsPagination.value.pageNumber = 1;
  };

 return {
    events,
    topSales,
    topRevenue,
    loading,
    error,
    pagination,
    sortDirection,
    sortField,
    loadUpcomingEvents,
    loadTopSales,
    loadTopRevenue,
    setPage,
    setPageSize,
    setSorting,

    tickets,
    ticketsLoading,
    ticketsError,
    ticketsPagination,
    loadTicketsForEvent,
    setTicketsPage,
    setTicketsPageSize,
  };
});