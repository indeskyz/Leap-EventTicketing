<script setup lang="ts">
import { XCircleIcon } from '@heroicons/vue/20/solid';

defineProps<{
  error: Error;
}>();

const emit = defineEmits<{
  (e: 'retry'): void;
}>();
</script>

<template>
  <div class="rounded-md bg-red-50 p-4">
    <div class="flex">
      <div class="flex-shrink-0">
        <XCircleIcon class="h-5 w-5 text-red-400" aria-hidden="true" />
      </div>
      <div class="ml-3">
        <h3 class="text-sm font-medium text-red-800">An error occurred</h3>
        <div class="mt-2 text-sm text-red-700">
          <p>{{ error.message }}</p>
        </div>
        <div class="mt-4">
          <button
            type="button"
            @click="emit('retry')"
            class="rounded-md bg-red-50 px-2 py-1.5 text-sm font-medium text-red-800 hover:bg-red-100 focus:outline-none focus:ring-2 focus:ring-red-600 focus:ring-offset-2 focus:ring-offset-red-50"
          >
            Retry
          </button>
        </div>
      </div>
    </div>
  </div>
</template>