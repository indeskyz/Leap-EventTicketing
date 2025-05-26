import { defineStore } from 'pinia';
import { ref } from 'vue';
import { fetchEvents, fetchTopEventsBySales } from '@/api/eventService';
import type { Event, SalesSummary, PaginatedResponse } from '@/api/eventService';

export const useEventStore = defineStore('events', () => {
  // State
  const events = ref<Event[]>([]);
  const salesSummary = ref<SalesSummary[]>([]);
  const loading = ref(false);
  const error = ref<Error | null>(null);
  const pagination = ref({
    page: 1,
    pageSize: 10,
    totalCount: 0,
  });
  const sortField = ref<'name' | 'startDate'>('startDate');
  const sortDirection = ref<'asc' | 'desc'>('asc');

  // Actions
  const loadEvents = async () => {
    loading.value = true;
    error.value = null;
    try {
      const response = await fetchEvents(
        pagination.value.page,
        pagination.value.pageSize,
        sortField.value,
        sortDirection.value
      );
      events.value = response.data;
      pagination.value = {
        page: response.page,
        pageSize: response.pageSize,
        totalCount: response.totalCount,
      };
    } catch (err) {
      error.value = err instanceof Error ? err : new Error(String(err));
    } finally {
      loading.value = false;
    }
  };

  const loadTopSales = async () => {
    loading.value = true;
    error.value = null;
    try {
      const response = await fetchTopEventsBySales(5);
      salesSummary.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err : new Error(String(err));
    } finally {
      loading.value = false;
    }
  };

  const setPage = (page: number) => {
    pagination.value.page = page;
    loadEvents();
  };

  const setPageSize = (size: number) => {
    pagination.value.pageSize = size;
    pagination.value.page = 1;
    loadEvents();
  };

  const setSort = (field: 'name' | 'startDate') => {
    if (sortField.value === field) {
      sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
    } else {
      sortField.value = field;
      sortDirection.value = 'asc';
    }
    loadEvents();
  };

  // Return state and actions
  return {
    // State
    events,
    salesSummary,
    loading,
    error,
    pagination,
    sortField,
    sortDirection,
    
    // Actions
    loadEvents,
    loadTopSales,
    setPage,
    setPageSize,
    setSort,
  };
});