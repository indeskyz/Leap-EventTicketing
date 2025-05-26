<script setup lang="tsx">
import { CalendarIcon } from '@heroicons/vue/20/solid';
import type { Event } from '@/models/apiTypes';
import ErrorDisplay from '@/components/ui/ErrorDisplay.vue'
import type { DataTableSortEvent } from 'primevue/datatable';


const props = defineProps<{
  events: Event[];
  loading: boolean;
  error: Error | null;
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
  };
  sortField: 'name' | 'startDate' | null;
  sortDirection: 'asc' | 'desc' | null;
}>();

const emit = defineEmits<{
  (e: 'page-change', pageNumber: number): void;
  (e: 'page-size-change', size: number): void;
  (e: 'sort', field: 'name' | 'startDate', order: 1 | -1): void;
}>();

const onSort = (event: DataTableSortEvent) => {
  const field = event.sortField;
  const order = event.sortOrder === -1 ? -1 : 1;
  
  if (field === 'name' || field === 'startDate') {
    emit('sort', field, order);
  }
};

const onPage = (e: { first: number; rows: number }) => {
  const newPage = e.first / e.rows + 1;
  emit('page-change', newPage);
  emit('page-size-change', e.rows);
};

// Body templates
function nameBodyTemplate(data: Event) {
  return (
    <div>
      <span>
        <CalendarIcon />
      </span>
      <div>
        <div>{data.name}</div>
        <div>{data.description}</div>
      </div>
    </div>
  );
}

function startDateBodyTemplate(data: Event) {
  const date = new Date(data.startDate);
  return (
    <div>
      <div>{date.toLocaleDateString()}</div>
      <div>
        {date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
      </div>
    </div>
  );
}

function endDateBodyTemplate(data: Event) {
  const date = new Date(data.endDate);
  return (
    <div>
      <div>{date.toLocaleDateString()}</div>
      <div>
        {date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
      </div>
    </div>
  );
}

function locationBodyTemplate(data: Event) {
  return (
    <span>
      {data.location}
    </span>
  );
}
</script>

<template>
  <div>
    <h2>Upcoming Events</h2>
    
    <ErrorDisplay v-if="error" :error="error" />    

    <DataTable
      :value="events"
      :loading="loading"
      :sortField="sortField || undefined"
      :sortOrder="sortDirection === 'asc' ? 1 : -1"
      @sort="onSort"
      responsiveLayout="scroll"
      dataKey="id"
      :paginator="false"
    >
      <Column
        field="name"
        header="Event Name"
        sortable
        :body="nameBodyTemplate"
      />
      <Column
        field="startDate"
        header="Start Date"
        sortable
        :body="startDateBodyTemplate"
      />
      <Column field="endDate" header="End Date" :body="endDateBodyTemplate" />
      <Column field="location" header="Location" :body="locationBodyTemplate" />
    </DataTable>

    <Paginator
      :rows="pagination.pageSize"
      :totalRecords="pagination.totalCount"
      :first="(pagination.pageNumber - 1) * pagination.pageSize"
      :rowsPerPageOptions="[5, 10, 20, 50]"
      @page="onPage"
    />
  </div>
</template>
