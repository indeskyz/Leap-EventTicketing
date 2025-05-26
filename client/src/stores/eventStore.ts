import { defineStore } from 'pinia';
import { ref } from 'vue';
import {
  fetchUpcomingEvents,
  fetchTopEventsBySales,
  fetchTopEventsByRevenue,
} from '@/api/eventService';

import type {
  Event,
  EventSalesSummary,
  EventsQueryParams,
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
      events.value = response.items;
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

  return {
    events,
    topSales,
    topRevenue,
    loading,
    error,
    pagination,
    loadUpcomingEvents,
    loadTopSales,
    loadTopRevenue,
    setPage,
    setPageSize,
  };
});