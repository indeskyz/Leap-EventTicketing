<!-- components/common/AppPagination.vue -->
<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  currentPage: number;
  pageSize: number;
  totalItems: number;
  maxVisiblePages?: number;
}>();

const emit = defineEmits<{
  (e: 'page-change', page: number): void;
}>();

const totalPages = computed(() => Math.ceil(props.totalItems / props.pageSize));

const visiblePages = computed(() => {
  const maxVisible = props.maxVisiblePages || 5;
  const half = Math.floor(maxVisible / 2);
  let start = Math.max(1, props.currentPage - half);
  const end = Math.min(start + maxVisible - 1, totalPages.value);

  if (end - start + 1 < maxVisible) {
    start = Math.max(1, end - maxVisible + 1);
  }

  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
});

const handlePageChange = (page: number) => {
  if (page >= 1 && page <= totalPages.value) {
    emit('page-change', page);
  }
};
</script>

<template>
  <div class="flex items-center justify-between mt-4">
    <div class="text-sm text-gray-700">
      Showing <span class="font-medium">{{ (currentPage - 1) * pageSize + 1 }}</span> to
      <span class="font-medium">{{ Math.min(currentPage * pageSize, totalItems) }}</span> of
      <span class="font-medium">{{ totalItems }}</span> results
    </div>
    
    <div class="flex space-x-1">
      <button
        @click="handlePageChange(currentPage - 1)"
        :disabled="currentPage === 1"
        class="px-3 py-1 rounded-md border border-gray-300 bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Previous
      </button>
      
      <button
        v-for="page in visiblePages"
        :key="page"
        @click="handlePageChange(page)"
        :class="{
          'px-3 py-1 rounded-md border text-sm font-medium': true,
          'border-blue-500 bg-blue-50 text-blue-600': page === currentPage,
          'border-gray-300 bg-white text-gray-700 hover:bg-gray-50': page !== currentPage,
        }"
      >
        {{ page }}
      </button>
      
      <button
        @click="handlePageChange(currentPage + 1)"
        :disabled="currentPage === totalPages"
        class="px-3 py-1 rounded-md border border-gray-300 bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        Next
      </button>
    </div>
  </div>
</template>