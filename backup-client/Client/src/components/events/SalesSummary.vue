<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { fetchTopEventsBySales } from '@/services/eventService';
import AppTable from '@/components/common/AppTable.vue';
import AppPagination from '@/components/common/AppPagination.vue';
import type { SalesSummary, Pagination } from '@/services/types';

const sales = ref<SalesSummary[]>([]);
const pagination = ref<Pagination>({
  page: 1,
  pageSize: 5,
  totalItems: 0,
});
const loading = ref(false);
const error = ref<Error | null>(null);

const headers = [
  { text: 'Event', value: 'eventName' },
  { text: 'Tickets Sold', value: 'ticketsSold' },
  { text: 'Total Revenue', value: 'totalRevenue' },
];

const loadSalesData = async () => {
  try {
    loading.value = true;
    error.value = null;
    const response = await fetchTopEventsBySales(pagination.value.page, pagination.value.pageSize);
    sales.value = response.data;
    pagination.value = response.pagination;
  } catch (err) {
    error.value = err as Error;
  } finally {
    loading.value = false;
  }
};

const handlePageChange = (page: number) => {
  pagination.value.page = page;
  loadSalesData();
};

const handlePageSizeChange = (size: number) => {
  pagination.value.pageSize = size;
  pagination.value.page = 1;
  loadSalesData();
};

onMounted(() => {
  loadSalesData();
});
</script>

<template>
  <div class="space-y-4">
    <div class="flex justify-between items-center">
      <div class="flex items-center space-x-2">
        <label for="pageSize" class="text-sm text-gray-700">Items per page:</label>
        <select
          id="pageSize"
          :value="pagination.pageSize"
          @change="handlePageSizeChange(Number($event.target.value))"
          class="border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:ring-blue-500 text-sm"
        >
          <option value="5">5</option>
          <option value="10">10</option>
          <option value="20">20</option>
        </select>
      </div>
    </div>

    <AppTable
      :items="sales"
      :headers="headers"
      :loading="loading"
      :error="error"
    >
      <template #row="{ item }: { item: SalesSummary }">
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm font-medium text-gray-900">{{ item.eventName }}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">{{ item.ticketsSold }}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">
            {{ item.totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
          </div>
        </td>
      </template>
    </AppTable>

    <AppPagination
      :current-page="pagination.page"
      :page-size="pagination.pageSize"
      :total-items="pagination.totalItems"
      @page-change="handlePageChange"
    />
  </div>
</template>