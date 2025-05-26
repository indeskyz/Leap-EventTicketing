<script setup lang="ts">
import { useToast } from 'primevue/usetoast'
import { watch } from 'vue'

const props = defineProps<{
  error: Error
}>()

const toast = useToast()

watch(
  () => props.error,
  (err) => {
    if (err) {
      toast.add({
        severity: 'error',
        summary: 'An error occurred',
        detail: err.message,
        life: 5000,
        closable: true
      })
    }
  },
  { immediate: true }
)
</script>

<template>
  <div />
</template>
