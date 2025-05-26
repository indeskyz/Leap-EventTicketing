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
    pageNumber: number;
    pageSize: number;
    totalCount: number;
  };
  sortField: 'name' | 'startDate';
  sortDirection: 'asc' | 'desc';
}>();

const emit = defineEmits<{
  (e: 'page-change', pageNumber: number): void;
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
  <div class="space-y-6">
    <!-- Status Indicators -->
    <div v-if="error" class="rounded-lg bg-danger-50 p-4 shadow-sm">
      <div class="flex items-center">
        <XCircleIcon class="h-5 w-5 text-danger-500 mr-2" />
        <p class="text-danger-700">{{ error.message }}</p>
      </div>
    </div>

    <div v-if="loading" class="flex flex-col items-center justify-center py-12">
      <LoadingSpinner class="h-12 w-12 text-primary-500" />
      <p class="mt-3 text-lg font-medium text-secondary-500">Loading events...</p>
    </div>

    <!-- Table Container -->
    <div v-else class="rounded-xl border border-gray-200 bg-white shadow-sm">
      <!-- Table -->
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th
                v-for="header in headers"
                :key="header.key"
                scope="col"
                class="px-6 py-3 text-left text-sm font-semibold text-secondary-600 uppercase tracking-wider"
                :class="{ 'cursor-pointer hover:bg-gray-100': header.sortable }"
                @click="header.sortable ? handleSort(header.key as 'name' | 'startDate') : null"
              >
                <div class="flex items-center">
                  {{ header.label }}
                  <span v-if="header.sortable && sortField === header.key" class="ml-2">
                    <ChevronUpIcon
                      v-if="sortDirection === 'asc'"
                      class="h-4 w-4 text-primary-500"
                    />
                    <ChevronDownIcon
                      v-else
                      class="h-4 w-4 text-primary-500"
                    />
                  </span>
                </div>
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200 bg-white">
            <tr 
              v-for="event in events" 
              :key="event.id" 
              class="hover:bg-gray-50 transition-colors duration-150"
            >
              <td class="whitespace-nowrap px-6 py-4">
                <div class="flex items-center">
                  <div class="flex-shrink-0 h-10 w-10 rounded-full bg-primary-100 flex items-center justify-center mr-4">
                    <CalendarIcon class="h-5 w-5 text-primary-600" />
                  </div>
                  <div>
                    <p class="text-sm font-medium text-gray-900">{{ event.name }}</p>
                    <p class="text-sm text-gray-500 line-clamp-1">{{ event.description }}</p>
                  </div>
                </div>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <div class="text-sm text-gray-900">
                  {{ new Date(event.startDate).toLocaleDateString() }}
                </div>
                <div class="text-xs text-gray-400">
                  {{ new Date(event.startDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}
                </div>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <div class="text-sm text-gray-900">
                  {{ new Date(event.endDate).toLocaleDateString() }}
                </div>
                <div class="text-xs text-gray-400">
                  {{ new Date(event.endDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) }}
                </div>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-primary-100 text-primary-800">
                  {{ event.location }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <div class="bg-gray-50 px-6 py-3 flex items-center justify-between border-t border-gray-200 rounded-b-xl">
        <div class="flex-1 flex justify-between sm:hidden">
          <button
            :disabled="pagination.pageNumber <= 1"
            @click="emit('page-change', pagination.pageNumber - 1)"
            class="relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50"
          >
            Previous
          </button>
          <button
            :disabled="pagination.pageNumber >= totalPages"
            @click="emit('page-change', pagination.pageNumber + 1)"
            class="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 disabled:opacity-50"
          >
            Next
          </button>
        </div>
        <div class="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
          <div>
            <p class="text-sm text-gray-700">
              Showing <span class="font-medium">{{ (pagination.pageNumber - 1) * pagination.pageSize + 1 }}</span> to
              <span class="font-medium">{{ Math.min(pagination.pageNumber * pagination.pageSize, pagination.totalCount) }}</span> of
              <span class="font-medium">{{ pagination.totalCount }}</span> results
            </p>
          </div>
          <div class="flex items-center space-x-4">
            <div class="flex items-center">
              <label for="page-size" class="mr-2 text-sm text-gray-700">Rows:</label>
              <select
                id="page-size"
                v-model="pagination.pageSize"
                @change="emit('page-size-change', Number($event.target.value))"
                class="block w-full rounded-md border-gray-300 shadow-sm focus:border-primary-500 focus:ring-primary-500 text-sm"
              >
                <option v-for="size in [5, 10, 20, 50]" :key="size" :value="size">
                  {{ size }}
                </option>
              </select>
            </div>
            <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">
              <button
                :disabled="pagination.pageNumber <= 1"
                @click="emit('page-change', pagination.pageNumber - 1)"
                class="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 disabled:opacity-50"
              >
                <span class="sr-only">Previous</span>
                <ChevronLeftIcon class="h-5 w-5" aria-hidden="true" />
              </button>
              <button
                v-for="page in Math.min(5, totalPages)"
                :key="page"
                @click="emit('page-change', page)"
                :class="{
                  'z-10 bg-primary-50 border-primary-500 text-primary-600': pagination.pageNumber === page,
                  'bg-white border-gray-300 text-gray-500 hover:bg-gray-50': pagination.pageNumber !== page,
                  'relative inline-flex items-center px-4 py-2 border text-sm font-medium': true
                }"
              >
                {{ page }}
              </button>
              <button
                :disabled="pagination.pageNumber >= totalPages"
                @click="emit('page-change', pagination.pageNumber + 1)"
                class="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 disabled:opacity-50"
              >
                <span class="sr-only">Next</span>
                <ChevronRightIcon class="h-5 w-5" aria-hidden="true" />
              </button>
            </nav>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>