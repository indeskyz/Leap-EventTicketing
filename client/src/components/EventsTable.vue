<script setup lang="ts">
import { computed } from 'vue';
import { ChevronUpIcon, ChevronDownIcon } from '@heroicons/vue/20/solid';
import type { Event } from '@/api/eventService';
import ChevronLeftIcon from '@heroicons/vue/20/solid/ChevronLeftIcon';
import ChevronRightIcon from '@heroicons/vue/20/solid/ChevronRightIcon';

const props = defineProps<{
  events: Event[];
  loading: boolean;
  error: Error | null;
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
  };
  sortField: 'name' | 'startDate';
  sortDirection: 'asc' | 'desc';
}>();

const emit = defineEmits<{
  (e: 'page-change', page: number): void;
  (e: 'page-size-change', size: number): void;
  (e: 'sort', field: 'name' | 'startDate'): void;
}>();

const headers = [
  { key: 'name', label: 'Event Name', sortable: true },
  { key: 'startDate', label: 'Start Date', sortable: true },
  { key: 'endDate', label: 'End Date' },
  { key: 'location', label: 'Location' },
];

const totalPages = computed(() => 
  Math.ceil(props.pagination.totalCount / props.pagination.pageSize)
);

const handleSort = (field: 'name' | 'startDate') => {
  emit('sort', field);
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
            <th
              v-for="header in headers"
              :key="header.key"
              scope="col"
              class="px-6 py-3 text-left text-sm font-semibold text-gray-900"
              :class="{ 'cursor-pointer hover:bg-gray-100': header.sortable }"
              @click="header.sortable ? handleSort(header.key as 'name' | 'startDate') : null"
            >
              <div class="flex items-center">
                {{ header.label }}
                <span v-if="header.sortable && sortField === header.key" class="ml-1">
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
          <tr v-for="event in events" :key="event.id" class="hover:bg-gray-50">
            <td class="whitespace-nowrap px-6 py-4">
              <div class="font-medium text-gray-900">{{ event.name }}</div>
              <div class="text-gray-500 line-clamp-2">{{ event.description }}</div>
            </td>
            <td class="whitespace-nowrap px-6 py-4 text-gray-500">
              {{ new Date(event.startDate).toLocaleDateString() }}
              <div class="text-sm text-gray-400">
                {{ new Date(event.startDate).toLocaleTimeString() }}
              </div>
            </td>
            <td class="whitespace-nowrap px-6 py-4 text-gray-500">
              {{ new Date(event.endDate).toLocaleDateString() }}
              <div class="text-sm text-gray-400">
                {{ new Date(event.endDate).toLocaleTimeString() }}
              </div>
            </td>
            <td class="whitespace-nowrap px-6 py-4 text-gray-500">
              {{ event.location }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <div class="flex items-center justify-between border-t border-gray-200 px-4 py-3 sm:px-6">
      <div class="flex flex-1 justify-between sm:hidden">
        <button
          :disabled="pagination.page <= 1"
          @click="emit('page-change', pagination.page - 1)"
          class="relative inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50"
        >
          Previous
        </button>
        <button
          :disabled="pagination.page >= totalPages"
          @click="emit('page-change', pagination.page + 1)"
          class="relative ml-3 inline-flex items-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50"
        >
          Next
        </button>
      </div>
      <div class="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
        <div>
          <p class="text-sm text-gray-700">
            Showing <span class="font-medium">{{ (pagination.page - 1) * pagination.pageSize + 1 }}</span> to
            <span class="font-medium">{{ Math.min(pagination.page * pagination.pageSize, pagination.totalCount) }}</span> of
            <span class="font-medium">{{ pagination.totalCount }}</span> results
          </p>
        </div>
        <div class="flex items-center space-x-2">
          <select
            v-model="pagination.pageSize"
            @change="emit('page-size-change', Number($event.target.value))"
            class="rounded-md border-gray-300 py-1 pl-2 pr-8 text-sm focus:border-blue-500 focus:outline-none focus:ring-blue-500"
          >
            <option value="5">5 per page</option>
            <option value="10">10 per page</option>
            <option value="20">20 per page</option>
            <option value="50">50 per page</option>
          </select>
          <nav class="flex gap-1" aria-label="Pagination">
            <button
              :disabled="pagination.page <= 1"
              @click="emit('page-change', pagination.page - 1)"
              class="relative inline-flex items-center rounded-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0 disabled:opacity-50"
            >
              <span class="sr-only">Previous</span>
              <ChevronLeftIcon class="h-5 w-5" aria-hidden="true" />
            </button>
            <button
              v-for="page in Math.min(5, totalPages)"
              :key="page"
              @click="emit('page-change', page)"
              :class="{
                'relative z-10 inline-flex items-center px-4 py-2 text-sm font-semibold focus:z-20': true,
                'bg-blue-600 text-white focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600': pagination.page === page,
                'text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50': pagination.page !== page,
              }"
            >
              {{ page }}
            </button>
            <button
              :disabled="pagination.page >= totalPages"
              @click="emit('page-change', pagination.page + 1)"
              class="relative inline-flex items-center rounded-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 focus:z-20 focus:outline-offset-0 disabled:opacity-50"
            >
              <span class="sr-only">Next</span>
              <ChevronRightIcon class="h-5 w-5" aria-hidden="true" />
            </button>
          </nav>
        </div>
      </div>
    </div>
  </div>
</template>