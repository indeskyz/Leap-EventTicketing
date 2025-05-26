<script setup lang="ts">
import { ref } from 'vue';
import Card from 'primevue/card';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Panel from 'primevue/panel';
import Button from 'primevue/button';

const props = defineProps({
  topSales: {
    type: Array as () => EventSalesSummary[],
    required: true,
    default: () => []
  },
  topRevenue: {
    type: Array as () => EventSalesSummary[],
    required: true,
    default: () => []
  },
  loadingSales: {
    type: Boolean,
    default: false
  },
  loadingRevenue: {
    type: Boolean,
    default: false
  }
});

const activeTab = ref(0);
const showDebug = ref(false);

const formatCurrency = (amount: number) => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  })
};
</script>

<template>
  <Card>
    <template #title>
      <h3>Top 5 Events Summary</h3>
      <Button
        :label="showDebug ? 'Hide Debug Data' : 'Show Debug Data'"
        icon="pi pi-info-circle"
        @click="showDebug = !showDebug"
      />
    </template>

    <template #content>
      <TabView v-model:activeIndex="activeTab">
        <!-- Ticket Sales Tab -->
        <TabPanel header="Ticket Sales">
          <DataTable
            :value="topSales"
            :loading="props.loadingSales"
            dataKey="eventId"
            responsiveLayout="scroll"
          >
            <Column field="eventName" header="Event" sortable />
            <Column field="ticketsSold" header="Tickets Sold" sortable />
          </DataTable>
        </TabPanel>

        <!-- Revenue Tab -->
        <TabPanel header="Revenue">
          <DataTable
            :value="topRevenue"
            :loading="props.loadingRevenue"
            dataKey="eventId"
            responsiveLayout="scroll"
          >
            <Column field="eventName" header="Event" sortable />
            <Column
              field="totalRevenue"
              header="Total Revenue"
              sortable
              :body="row => formatCurrency(row.totalRevenue)"
            />
          </DataTable>
        </TabPanel>
      </TabView>

      <Panel v-if="showDebug" header="Debug Data">
        <pre>Sales: {{ JSON.stringify(topSales, null, 2) }}</pre>
        <pre>Revenue: {{ JSON.stringify(topRevenue, null, 2) }}</pre>
      </Panel>
    </template>
  </Card>
</template>
