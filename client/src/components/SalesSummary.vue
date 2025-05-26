<script setup lang="ts">
import { ref, computed } from 'vue';
import { ChevronUpIcon, ChevronDownIcon } from '@heroicons/vue/20/solid';
import { XCircleIcon, TicketIcon, CurrencyDollarIcon } from '@heroicons/vue/24/outline';

interface SalesSummary {
  eventId: string;
  eventName: string;
  ticketsSold: number;
  totalRevenue: number;
}

const props = defineProps<{
  topSales: SalesSummary[];
  topRevenue: SalesSummary[];
  loading: boolean;
  error: Error | null;
}>();

// Local state for which view to show
const activeView = ref<'sales' | 'revenue'>('sales');

// Computed properties for current data
const currentData = computed(() => {
  return activeView.value === 'sales' ? props.topSales : props.topRevenue;
});

const currentTitle = computed(() => {
  return activeView.value === 'sales' ? 'Top 5 Events by Tickets Sold' : 'Top 5 Events by Revenue';
});

// Summary calculations
const totalTicketsSold = computed(() => {
  return props.topSales.reduce((sum, sale) => sum + sale.ticketsSold, 0);
});

const totalRevenue = computed(() => {
  return props.topRevenue.reduce((sum, sale) => sum + sale.totalRevenue, 0);
});
</script>

<template>
  <div class="space-y-6">
    <!-- Error Display -->
    <div v-if="error" class="rounded-lg bg-red-50 p-4 shadow-sm">
      <div class="flex items-center">
        <XCircleIcon class="h-5 w-5 text-red-500 mr-2" />
        <p class="text-red-700">{{ error.message }}</p>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex flex-col items-center justify-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      <p class="mt-3 text-lg font-medium text-gray-500">Loading sales data...</p>
    </div>

    <!-- Content -->
    <div v-else class="space-y-6">
      <!-- Summary Cards -->
      <div class="grid grid-cols-1 gap-6 sm:grid-cols-2">
        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-blue-500 rounded-md p-3">
                <TicketIcon class="h-6 w-6 text-white" />
              </div>
              <div class="ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-sm font-medium text-gray-500 truncate">Total Tickets Sold (Top 5)</dt>
                  <dd class="flex items-baseline">
                    <div class="text-2xl font-semibold text-gray-900">
                      {{ totalTicketsSold.toLocaleString() }}
                    </div>
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>

        <div class="bg-white overflow-hidden shadow rounded-lg">
          <div class="px-4 py-5 sm:p-6">
            <div class="flex items-center">
              <div class="flex-shrink-0 bg-green-500 rounded-md p-3">
                <CurrencyDollarIcon class="h-6 w-6 text-white" />
              </div>
              <div class="ml-5 w-0 flex-1">
                <dl>
                  <dt class="text-sm font-medium text-gray-500 truncate">Total Revenue (Top 5)</dt>
                  <dd class="flex items-baseline">
                    <div class="text-2xl font-semibold text-gray-900">
                      {{ totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
                    </div>
                  </dd>
                </dl>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- View Toggle -->
      <div class="flex items-center justify-center">
        <div class="flex rounded-lg border border-gray-200 bg-gray-50 p-1">
          <button
            @click="activeView = 'sales'"
            :class="{
              'bg-white shadow-sm text-blue-600 border-blue-200': activeView === 'sales',
              'text-gray-600 hover:text-gray-800 hover:bg-gray-100': activeView !== 'sales'
            }"
            class="px-4 py-2 text-sm font-medium rounded-md border transition-all duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1"
          >
            Top by Sales
          </button>
          <button
            @click="activeView = 'revenue'"
            :class="{
              'bg-white shadow-sm text-blue-600 border-blue-200': activeView === 'revenue',
              'text-gray-600 hover:text-gray-800 hover:bg-gray-100': activeView !== 'revenue'
            }"
            class="px-4 py-2 text-sm font-medium rounded-md border transition-all duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1"
          >
            Top by Revenue
          </button>
        </div>
      </div>

      <!-- Table -->
      <div class="bg-white shadow rounded-lg overflow-hidden">
        <div class="px-6 py-4 border-b border-gray-200">
          <h3 class="text-lg font-medium text-gray-900">{{ currentTitle }}</h3>
        </div>
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Rank
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Event Name
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Tickets Sold
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Revenue
                </th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr 
                v-for="(sale, index) in currentData" 
                :key="sale.eventId" 
                class="hover:bg-gray-50 transition-colors duration-150"
                :class="{ 'bg-yellow-50': index === 0 }"
              >
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="flex items-center">
                    <span 
                      class="inline-flex items-center justify-center h-8 w-8 rounded-full text-sm font-medium"
                      :class="{
                        'bg-yellow-500 text-white': index === 0,
                        'bg-gray-200 text-gray-700': index === 1,
                        'bg-orange-200 text-orange-800': index === 2,
                        'bg-blue-100 text-blue-800': index > 2
                      }"
                    >
                      {{ index + 1 }}
                    </span>
                  </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm font-medium text-gray-900">{{ sale.eventName }}</div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm text-gray-900">{{ sale.ticketsSold.toLocaleString() }}</div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm font-semibold text-green-600">
                    {{ sale.totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Empty State -->
      <div v-if="currentData.length === 0" class="text-center py-12">
        <p class="text-gray-500">No sales data available</p>
      </div>
    </div>
  </div>
</template>