<script setup lang="ts">
import { ref, computed, watch } from 'vue'; // <-- you need computed
import { useEventStore } from '@/stores/eventStore';
import EventTicketLookup from '@/components/EventTicketLookup.vue';

const eventStore = useEventStore();

const eventId = ref('');

const tickets = computed(() => eventStore.tickets);
const totalRecords = computed(() => eventStore.totalRecords);
const loading = computed(() => eventStore.loading);
const page = computed(() => eventStore.page);
const pageSize = computed(() => eventStore.pageSize);

watch(eventId, (newId) => {
  if (newId) {
    eventStore.setTicketsPage(1);
    eventStore.loadTicketsForEvent(newId);
  }
});

function onSearch() {
  if (!eventId.value) return;
  eventStore.setTicketsPage(1);
  eventStore.loadTicketsForEvent(eventId.value);
}

function onPageChange(page: number) {
  eventStore.setTicketsPage(page);
  eventStore.loadTicketsForEvent(eventId.value);
}

function onPageSizeChange(size: number) {
  eventStore.setTicketsPageSize(size);
  eventStore.loadTicketsForEvent(eventId.value);
}
</script>

<template>
  <div>
    <EventTicketLookup
      :eventId="eventId"
      :tickets="tickets"
      :totalRecords="totalRecords"
      :loading="loading"
      :page="page"
      :pageSize="pageSize"
      @update:eventId="eventId = $event"
      @search="onSearch"
      @page-change="onPageChange"
      @page-size-change="onPageSizeChange"
    />
  </div>
</template>
