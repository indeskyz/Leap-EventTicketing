<script setup lang="ts">
import { onMounted, computed } from 'vue';
import EventsTable from '@/components/EventsTable.vue';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue';
import LoadingSpinner from '@/components/ui/LoadingSpinner.vue';
import { useEventStore } from '@/stores/eventStore';

const eventStore = useEventStore();

const tableProps = computed(() => ({
  events: eventStore.events,
  loading: eventStore.loading,
  error: eventStore.error,
  pagination: eventStore.pagination,
  sortField: eventStore.sortField,
  sortDirection: eventStore.sortDirection
}));

onMounted(() => {
  if (eventStore.events.length === 0) {
    eventStore.loadEvents();
  }
});

const handlePageChange = (page: number) => {
  eventStore.setPage(page);
};

const handlePageSizeChange = (size: number) => {
  eventStore.setPageSize(size);
};

const handleSort = (field: 'name' | 'startDate') => {
  eventStore.setSort(field);
};

const handleRetry = () => {
  eventStore.loadEvents();
};
</script>

<template>
  <div class="py-6">
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-semibold text-gray-900">Upcoming Events</h1>
        <LoadingSpinner v-if="eventStore.loading" class="h-6 w-6 text-blue-500" />
      </div>

      <ErrorDisplay 
        v-if="eventStore.error" 
        :error="eventStore.error"
        @retry="handleRetry"
        class="mb-6"
      />

      <div v-if="eventStore.loading && eventStore.events.length === 0" class="py-12">
        <LoadingSpinner class="mx-auto h-12 w-12 text-blue-500" />
        <p class="mt-4 text-center text-gray-500">Loading events...</p>
      </div>

      <template v-else>
        <EventsTable
          v-bind="tableProps"
          @page-change="handlePageChange"
          @page-size-change="handlePageSizeChange"
          @sort="handleSort"
        />
      </template>
    </div>
  </div>
</template>