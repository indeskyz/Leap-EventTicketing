<script setup lang="ts">
import { ref, watch } from 'vue';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Paginator from 'primevue/paginator';
import type { TicketSalesDto } from '@/models/apiTypes';

const props = defineProps<{
  eventId: string;
  tickets: TicketSalesDto[];
  totalRecords: number;
  loading: boolean;
  page: number;
  pageSize: number;
}>();

const emit = defineEmits<{
  (e: 'update:eventId', value: string): void;
  (e: 'search'): void;
  (e: 'page-change', page: number): void;
  (e: 'page-size-change', size: number): void;
}>();

const inputEventId = ref(props.eventId);

watch(() => props.eventId, (val) => {
  inputEventId.value = val;
});

const onSearch = () => {
  emit('update:eventId', inputEventId.value);
  emit('search');
};

const onPageChange = (e: any) => {
  emit('page-change', e.page + 1);
  emit('page-size-change', e.rows);
};
</script>

<template>
  <div>
    <div>
      <InputText
        v-model="inputEventId"
        placeholder="Enter Event ID"
        @keyup.enter="onSearch"
      />
      <Button label="Search" icon="pi pi-search" @click="onSearch" />
    </div>

    <DataTable
      :value="tickets"
      :loading="loading"
      dataKey="id"
      responsiveLayout="scroll"
    >
      <Column field="id" header="Ticket ID" sortable />
      <Column field="eventId" header="Event ID" sortable />
      <Column field="type" header="Ticket Type" sortable />
      <Column field="price" header="Price (USD)" sortable>
        <template #body="{ data }">
          {{ data.price.toFixed(2) }} USD
        </template>
      </Column>
      <Column field="quantityAvailable" header="Qty Available" sortable />
      <Column field="quantitySold" header="Qty Sold" sortable />
    </DataTable>

    <Paginator
      :rows="pageSize"
      :totalRecords="totalRecords"
      :first="(page - 1) * pageSize"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      @page="onPageChange"
    />
  </div>
</template>
