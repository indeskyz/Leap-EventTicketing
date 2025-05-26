<!-- components/events/EventDataProvider.vue -->
<script setup lang="ts">
import { ref, provide, watch } from 'vue';
import { fetchEvents } from '@/services/eventService';
import type { Event, SortDirection, EventSortField, Pagination } from '@/services/types';

const props = defineProps<{
  sortField?: EventSortField;
  sortDirection?: SortDirection;
  page?: number;
  pageSize?: number;
}>();

const emit = defineEmits<{
  (e: 'update:page', value: number): void;
  (e: 'update:pageSize', value: number): void;
}>();

const events = ref<Event[]>([]);
const pagination = ref<Pagination>({
  page: props.page || 1,
  pageSize: props.pageSize || 10,
  totalItems: 0,
});
const loading = ref<boolean>(false);
const error = ref<Error | null>(null);

const loadEvents = async () => {
  try {
    loading.value = true;
    error.value = null;
    const response = await fetchEvents(
      pagination.value.page,
      pagination.value.pageSize,
      props.sortField,
      props.sortDirection
    );
    events.value = response.data;
    pagination.value = response.pagination;
  } catch (err) {
    error.value = err as Error;
  } finally {
    loading.value = false;
  }
};

// Watch for changes in pagination or sorting
watch(
  () => [props.sortField, props.sortDirection, pagination.value.page, pagination.value.pageSize],
  () => {
    loadEvents();
  },
  { immediate: true }
);

const handlePageChange = (page: number) => {
  pagination.value.page = page;
  emit('update:page', page);
};

const handlePageSizeChange = (size: number) => {
  pagination.value.pageSize = size;
  emit('update:pageSize', size);
};

provide('events', events);
provide('loading', loading);
provide('error', error);
provide('pagination', pagination);
provide('refresh', loadEvents);
provide('handlePageChange', handlePageChange);
provide('handlePageSizeChange', handlePageSizeChange);
</script>

<template>
  <slot v-bind="{ events, loading, error, pagination }" />
</template>