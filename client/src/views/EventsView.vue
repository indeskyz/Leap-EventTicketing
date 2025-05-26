<script setup lang="ts">
import { onMounted, computed, ref, watch } from 'vue';
import EventsTable from '@/components/EventsTable.vue';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue';
import LoadingSpinner from '@/components/ui/LoadingSpinner.vue';
import { useEventStore } from '@/stores/eventStore';
import Button from 'primevue/button';
import ButtonGroup from 'primevue/buttongroup';

const eventStore = useEventStore();

const selectedDays = ref<30 | 60 | 180>(30);
const daysOptions = [
  { value: 30, label: '30 days' },
  { value: 60, label: '60 days' },
  { value: 180, label: '180 days' }
] as const;

const tableProps = computed(() => ({
  events: eventStore.events,
  loading: eventStore.loading,
  error: eventStore.error,
  pagination: eventStore.pagination,
  sortField: eventStore.sortField || null,
  sortDirection: eventStore.sortDirection || null
}));

onMounted(() => {
  if (eventStore.events.length === 0) {
    eventStore.loadUpcomingEvents(selectedDays.value);
  }
});

const handleDaysChange = (days: 30 | 60 | 180) => {
  if (selectedDays.value === days) return;
  selectedDays.value = days;
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

const handleSort = (field: 'name' | 'startDate', order: 1 | -1) => {
  eventStore.setSort(field, order === 1 ? 'asc' : 'desc');
  eventStore.loadUpcomingEvents(selectedDays.value);
};

const handleRetry = () => {
  eventStore.loadUpcomingEvents(selectedDays.value);
};
</script>

<template>
  <div>
    <div>
      <div class="px-2">Show events for:</div>
      <ButtonGroup> 
        <Button
          v-for="option in daysOptions"
          :key="option.value"
          :label="option.label"
          :class="{ 'p-button-outlined': selectedDays !== option.value }"
          :disabled="eventStore.loading"
          @click="handleDaysChange(option.value)"
          :aria-pressed="selectedDays === option.value"
        />
      </ButtonGroup>
    </div>

    <ErrorDisplay v-if="error" :error="error" />
    
    <EventsTable
      v-bind="tableProps"
      @page-change="handlePageChange"
      @page-size-change="handlePageSizeChange"
      @sort="handleSort"
    />
  </div>
</template>

<style>
 .px-2 {
  padding: 10px 10px 10px 10px;
 }
</style>

