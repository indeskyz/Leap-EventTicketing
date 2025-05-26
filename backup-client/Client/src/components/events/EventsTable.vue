<script setup lang="ts">
import { inject } from 'vue';
import AppTable from '@/components/common/AppTable.vue';
import AppPagination from '@/components/common/AppPagination.vue';
import type { Event, EventSortField, SortDirection, Pagination } from '@/services/types';

const events = inject<Ref<Event[]>>('events')!;
const loading = inject<Ref<boolean>>('loading')!;
const error = inject<Ref<Error | null>>('error')!;
const pagination = inject<Ref<Pagination>>('pagination')!;
const handlePageChange = inject<(page: number) => void>('handlePageChange')!;
const handlePageSizeChange = inject<(size: number) => void>('handlePageSizeChange')!;

const props = defineProps<{
  sortField: EventSortField;
  sortDirection: SortDirection;
}>();

const emit = defineEmits<{
  (e: 'update:sortField', value: EventSortField): void;
  (e: 'update:sortDirection', value: SortDirection): void;
}>();

const headers = [
  { text: 'Event Name', value: 'name', sortable: true },
  { text: 'Start Date', value: 'startDate', sortable: true },
  { text: 'End Date', value: 'endDate' },
  { text: 'Location', value: 'location' },
];

const handleSort = (field: EventSortField) => {
  if (props.sortField === field) {
    emit('update:sortDirection', props.sortDirection === 'asc' ? 'desc' : 'asc');
  } else {
    emit('update:sortField', field);
    emit('update:sortDirection', 'asc');
  }
};
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
          <option value="50">50</option>
        </select>
      </div>
    </div>

    <AppTable
      :items="events"
      :headers="headers"
      :loading="loading"
      :error="error"
      :sort-field="sortField"
      :sort-direction="sortDirection"
      @sort="handleSort"
    >
      <template #row="{ item }: { item: Event }">
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm font-medium text-gray-900">{{ item.name }}</div>
          <div class="text-sm text-gray-500 line-clamp-2">{{ item.description }}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">
            {{ new Date(item.startDate).toLocaleDateString() }}
          </div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">
            {{ new Date(item.endDate).toLocaleDateString() }}
          </div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">{{ item.location }}</div>
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