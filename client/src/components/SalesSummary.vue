<script setup lang="ts">
import { ref, computed, h } from 'vue';
import  Button  from 'primevue/button';
import  Card  from 'primevue/card';
import DataTable from 'primevue/datatable';
import Column  from 'primevue/column';
import  Tooltip  from 'primevue/tooltip';
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

const activeView = ref<'sales' | 'revenue'>('sales');

const currentData = computed(() => {
  return activeView.value === 'sales' ? props.topSales : props.topRevenue;
});

const currentTitle = computed(() => {
  return activeView.value === 'sales' ? 'Top 5 Events by Tickets Sold' : 'Top 5 Events by Revenue';
});

const totalTicketsSold = computed(() => {
  return props.topSales.reduce((sum, sale) => sum + sale.ticketsSold, 0);
});

const totalRevenue = computed(() => {
  return props.topRevenue.reduce((sum, sale) => sum + sale.totalRevenue, 0);
});

const dismissed = ref(false);
const dismiss = () => {
  dismissed.value = true;
};
</script>

<template>
  <div v-if="!dismissed" class="space-y-6">
    <!-- Error Alert -->
    <Card
      v-if="error"
      class="p-error mb-4"
      title="Error"
      style="position: relative;"
    >
      <Button
        icon="pi pi-times"
        class="p-button-text p-button-sm"
        style="position: absolute; top: 0.5rem; right: 0.5rem;"
        @click="dismiss"
        aria-label="Dismiss error"
      />
      <div class="flex items-center gap-2">
        <XCircleIcon class="h-5 w-5 text-red-600" />
        <span class="text-red-700">{{ error.message }}</span>
      </div>
    </Card>

    <Card
      v-if="loading"
      class="mb-4"
      style="position: relative;"
    >
      <Button
        icon="pi pi-times"
        class="p-button-text p-button-sm"
        style="position: absolute; top: 0.5rem; right: 0.5rem;"
        @click="dismiss"
        aria-label="Dismiss loading state"
      />
      <div class="flex flex-col items-center justify-center py-12 gap-3">
        <LoadingSpinner class="h-12 w-12 text-blue-500" />
        <p class="text-lg font-medium text-gray-500">Loading sales data...</p>
      </div>
    </Card>

    <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
      <Card>
        <div class="flex items-center gap-4">
          <div class="bg-blue-500 rounded-md p-3 flex items-center justify-center">
            <TicketIcon class="h-6 w-6 text-white" />
          </div>
          <div>
            <h3 class="text-sm font-medium text-gray-500">Total Tickets Sold</h3>
            <p class="text-2xl font-semibold text-gray-900">{{ totalTicketsSold.toLocaleString() }}</p>
          </div>
        </div>
      </Card>

      <Card>
        <div class="flex items-center gap-4">
          <div class="bg-green-500 rounded-md p-3 flex items-center justify-center">
            <CurrencyDollarIcon class="h-6 w-6 text-white" />
          </div>
          <div>
            <h3 class="text-sm font-medium text-gray-500">Total Revenue</h3>
            <p class="text-2xl font-semibold text-gray-900">
              {{ totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
            </p>
          </div>
        </div>
      </Card>
    </div>

    <div class="flex justify-center mt-4">
      <Button
        label="By Sales"
        :class="{'p-button-primary': activeView === 'sales'}"
        class="mr-2"
        @click="activeView = 'sales'"
      />
      <Button
        label="By Revenue"
        :class="{'p-button-primary': activeView === 'revenue'}"
        @click="activeView = 'revenue'"
      />
    </div>

    <Card class="mt-6" style="position: relative;">
      <Button
        icon="pi pi-times"
        class="p-button-text p-button-sm"
        style="position: absolute; top: 0.5rem; right: 0.5rem;"
        @click="dismiss"
        aria-label="Dismiss summary"
      />
      <h3 class="text-lg font-medium mb-4">{{ currentTitle }}</h3>

      <DataTable
        :value="currentData"
        responsiveLayout="scroll"
        stripedRows
        size="small"
      >
        <Column
          field=""
          header="Rank"
          :body="(data, { rowIndex }) => {
            const colors = ['bg-yellow-100 text-yellow-800', 'bg-gray-100 text-gray-800', 'bg-orange-100 text-orange-800', 'bg-blue-50 text-blue-800'];
            const colorClass = rowIndex < colors.length ? colors[rowIndex] : colors[colors.length - 1];
            return h('span', { class: ['inline-flex items-center justify-center h-8 w-8 rounded-full text-sm font-medium', colorClass] }, rowIndex + 1);
          }"
          style="width: 4rem"
        />
        <Column field="eventName" header="Event Name" />
        <Column
          field="ticketsSold"
          header="Tickets Sold"
          :body="(data) => data.ticketsSold.toLocaleString()"
          style="text-align:right;"
        />
        <Column
          field="totalRevenue"
          header="Revenue"
          :body="(data) =>
            data.totalRevenue.toLocaleString('en-US', { style: 'currency', currency: 'USD' })"
          style="text-align:right;"
        />
      </DataTable>
    </Card>
  </div>
</template>
