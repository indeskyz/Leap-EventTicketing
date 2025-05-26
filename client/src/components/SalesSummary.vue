<script setup lang="ts">
import { ChevronUpIcon, ChevronDownIcon } from '@heroicons/vue/20/solid';
import type { SalesSummary } from '@/api/eventService';

const props = defineProps<{
  sales: SalesSummary[];
  loading: boolean;
  error: Error | null;
}>();

const sortBy = ref<'ticketsSold' | 'totalRevenue'>('ticketsSold');
const sortDirection = ref<'asc' | 'desc'>('desc');

const sortedSales = computed(() => {
  return [...props.sales].sort((a, b) => {
    const modifier = sortDirection.value === 'desc' ? -1 : 1;
    if (sortBy.value === 'ticketsSold') {
      return (a.ticketsSold - b.ticketsSold) * modifier;
    } else {
      return (a.totalRevenue - b.totalRevenue) * modifier;
    }
  });
});

const toggleSort = (field: 'ticketsSold' | 'totalRevenue') => {
  if (sortBy.value === field) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    sortBy.value = field;
    sortDirection.value = 'desc';
  }
};
</script>

<template>
  <div class="space-y-4">
    <!-- Error Message -->
    <div v-if="error" class="bg-red-50 text-red-700 p-4 rounded-lg">
      {{ error.message }}
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-8">
      <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
    </div>

    <!-- Table -->
    <div v-else class="overflow-hidden shadow ring-1 ring-black ring-opacity-5 rounded-lg">
      <table class="min-w-full divide-y divide-gray-300">
        <thead class="bg-gray-50">
          <tr>
            <th scope="col" class="px-6 py-3 text-left text-sm font-semibold text-gray-900">
              Event
            </th>
            <th
              scope="col"
              class="px-6 py-3 text-left text-sm font-semibold text-gray-900 cursor-pointer hover:bg-gray-100"
              @click="toggleSort('ticketsSold')"
            >
              <div class="flex items-center">
                Tickets Sold
                <span v-if="sortBy === 'ticketsSold'" class="ml-1">
                  <ChevronUpIcon
                    v-if="sortDirection === 'asc'"
                    class="h-4 w-4 text-gray-500"
                  />
                  <ChevronDownIcon
                    v-else
                    class="h-4 w-4 text-gray-500"
                  />
                </span>
              </div>
            </th>
            <th
              scope="col"
              class="px-6 py-3 text-left text-sm font-semibold text-gray-900 cursor-pointer hover:bg-gray-100"
              @click="toggleSort('totalRevenue')"
            >
              <div class="flex items-center">
                Total Revenue
                <span v-if="sortBy === 'totalRevenue'" class="ml-1">
                  <ChevronUpIcon
                    v-if="sortDirection === 'asc'"
                    class="h-4 w-4 text-gray-500"
                  />
                  <ChevronDownIcon
                    v-else
                    class="h-4 w-4 text-gray-500"
                  />
                </span>
              </div>
            </th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-200 bg-white">
          <tr v-for="sale in sortedSales" :key="sale.eventId" class="hover:bg-gray-50">
            <td class="whitespace-nowrap px-6 py-4 font-medium text-gray-900">
              {{ sale.eventName }}
            </td>
            <td class="whitespace-nowrap px-6 py-4 text-gray-500">
              {{ sale.ticketsSold.toLocaleString() }}
            </td>
            <td class="whitespace-nowrap px-6 py-4 text-gray-500">
              {{ sale.totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>