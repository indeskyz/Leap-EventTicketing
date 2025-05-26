<script setup lang="ts">
import { ref } from 'vue';
import EventDataProvider from '@/components/events/EventDataProvider.vue';
import EventsTable from '@/components/events/EventsTable.vue';
import type { EventSortField, SortDirection } from '@/services/types';

const sortField = ref<EventSortField>('startDate');
const sortDirection = ref<SortDirection>('asc');
</script>

<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold text-gray-900 mb-8">Events</h1>
    
    <EventDataProvider
      :sort-field="sortField"
      :sort-direction="sortDirection"
    >
      <EventsTable
        v-model:sort-field="sortField"
        v-model:sort-direction="sortDirection"
      />
    </EventDataProvider>
  </div>
</template>