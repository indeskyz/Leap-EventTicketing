<!-- SalesSummaryView.vue -->
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import SalesSummary from '@/components/SalesSummary.vue';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue';
import LoadingSpinner from '@/components/ui/LoadingSpinner.vue';
import { useEventStore } from '@/stores/eventStore';

const eventStore = useEventStore();
const error = ref<Error | null>(null);
const loading = ref(false);

onMounted(async () => {
  try {
    loading.value = true;
    await Promise.all([
      eventStore.loadTopSales(),
      eventStore.loadTopRevenue()
    ]);
  } catch (err) {
    error.value = err as Error;
  } finally {
    loading.value = false;
  }
});

const handleRetry = async () => {
  error.value = null;
  loading.value = true;
  try {
    await Promise.all([
      eventStore.loadTopSales(),
      eventStore.loadTopRevenue()
    ]);
  } catch (err) {
    error.value = err as Error;
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <div>
    <ErrorDisplay v-if="error" :error="error" @retry="handleRetry" />
    <LoadingSpinner v-else-if="loading" />
    <SalesSummary 
      v-else
      :top-sales="eventStore.topSales"
      :top-revenue="eventStore.topRevenue"
      :loading-sales="eventStore.loadingTopSales"
      :loading-revenue="eventStore.loadingTopRevenue"
    />
  </div>
</template>