<script setup lang="ts">
import { onMounted, computed, ref } from 'vue';
import EventsTable from '@/components/EventsTable.vue';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue';
import LoadingSpinner from '@/components/ui/LoadingSpinner.vue';
import { useEventStore } from '@/stores/eventStore';

const eventStore = useEventStore();

// Days selector state
const selectedDays = ref<30 | 60 | 180>(30);
const daysOptions = [
  { value: 30, label: '30 days' },
  { value: 60, label: '60 days' },
  { value: 180, label: '180 days' }
] as const;

const topEventsProps = computed(() => ({
  topSales: eventStore.topSales,
  topRevenue: eventStore.topRevenue,
}));

const tableProps = computed(() => ({
  events: eventStore.events,
  loading: eventStore.loading,
  error: eventStore.error,
  pagination: eventStore.pagination,
  // Remove these if not implemented in store:
  // sortField: eventStore.sortField,
  // sortDirection: eventStore.sortDirection
}));

onMounted(() => {
  if (eventStore.events.length === 0) {
    eventStore.loadUpcomingEvents(selectedDays.value);
  }
  
  eventStore.loadTopSales();
  eventStore.loadTopRevenue();
});

const handleDaysChange = (days: 30 | 60 | 180) => {
  selectedDays.value = days;
  // Reset to first page when changing days filter
  eventStore.setPage(1);
  eventStore.loadUpcomingEvents(days);
};

const handlePageChange = (page: number) => {
  eventStore.setPage(page);
  eventStore.loadUpcomingEvents(selectedDays.value);
};

const handlePageSizeChange = (size: number) => {
  eventStore.setPageSize(size);
  eventStore.loadUpcomingEvents(selectedDays.value);
};

// Remove or implement in store if needed
/*
const handleSort = (field: 'name' | 'startDate') => {
  eventStore.setSort(field);
};
*/

const handleRetry = () => {
  eventStore.loadUpcomingEvents(selectedDays.value);
};
</script>

<template>
  <div class="min-h-full py-8">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-900">Event Management</h1>
        <p class="mt-2 text-sm text-gray-500">Browse and manage upcoming events</p>
      </div>
      
      <div class="bg-white shadow rounded-xl p-6">
        <div class="flex flex-col space-y-6">
          <div class="flex items-center justify-between">
            <div class="flex items-center space-x-6">
              <h2 class="text-xl font-semibold text-gray-800">Upcoming Events</h2>
              
              <!-- Days Filter -->
              <div class="flex items-center space-x-3">
                <label class="text-sm font-medium text-gray-700">Show events for:</label>
                <div class="flex rounded-lg border border-gray-200 bg-gray-50 p-1">
                  <button
                    v-for="option in daysOptions"
                    :key="option.value"
                    @click="handleDaysChange(option.value)"
                    :class="{
                      'bg-white shadow-sm text-primary-600 border-primary-200': selectedDays === option.value,
                      'text-gray-600 hover:text-gray-800 hover:bg-gray-100': selectedDays !== option.value
                    }"
                    class="px-3 py-1.5 text-sm font-medium rounded-md border transition-all duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-primary-500 focus:ring-offset-1"
                    :disabled="eventStore.loading"
                  >
                    {{ option.label }}
                  </button>
                </div>
              </div>
            </div>
            
            <div class="flex items-center space-x-4">
              <LoadingSpinner v-if="eventStore.loading" class="h-5 w-5 text-primary-500" />
              <button
                v-if="eventStore.error"
                @click="handleRetry"
                class="inline-flex items-center px-3 py-1.5 border border-transparent text-xs font-medium rounded shadow-sm text-white bg-primary-500 hover:bg-primary-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
              >
                Retry
              </button>
            </div>
          </div>
          
          <ErrorDisplay
            v-if="eventStore.error"
            :error="eventStore.error"
            @retry="handleRetry"
          />
          
          <EventsTable
            v-bind="tableProps"
            @page-change="handlePageChange"
            @page-size-change="handlePageSizeChange"
            @sort="handleSort"
          />
        </div>
      </div>
    </div>
  </div>
</template>