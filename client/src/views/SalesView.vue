<script setup lang="ts">
import { onMounted, computed } from 'vue';
import SalesSummary from '@/components/SalesSummary.vue';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue';
import LoadingSpinner from '@/components/ui/LoadingSpinner.vue';
import { useEventStore } from '@/stores/eventStore';
import { Button } from 'primevue/button';

const eventStore = useEventStore();

const summaryProps = computed(() => ({
  topSales: eventStore.topSales,
  topRevenue: eventStore.topRevenue,
  loading: eventStore.loading,
  error: eventStore.error
}));

onMounted(() => {
  if (eventStore.topSales.length === 0) {
    eventStore.loadTopSales();
  }
  if (eventStore.topRevenue.length === 0) {
    eventStore.loadTopRevenue();
  }
});

const handleRetry = () => {
  eventStore.loadTopSales();
  eventStore.loadTopRevenue();
};
</script>

<template>
  <div class="py-6">
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between mb-6">
        <div>
          <h1 class="text-2xl font-semibold text-gray-900">Sales Summary</h1>
          <p class="mt-2 text-sm text-gray-500">Top performing events by sales and revenue</p>
        </div>
        <LoadingSpinner v-if="eventStore.loading" class="h-6 w-6 text-blue-500" />
      </div>

      <ErrorDisplay
        v-if="eventStore.error"
        :error="eventStore.error"
        @retry="handleRetry"
        class="mb-6"
      />

      <div v-if="eventStore.loading && eventStore.topSales.length === 0 && eventStore.topRevenue.length === 0" class="py-12 text-center">
        <LoadingSpinner class="mx-auto h-12 w-12 text-blue-500" />
        <p class="mt-4 text-gray-500">Loading sales data...</p>
      </div>

      <template v-else>
        <SalesSummary v-bind="summaryProps" />
      </template>
    </div>
  </div>
</template>
