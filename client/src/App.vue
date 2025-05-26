<script setup lang="ts">
import { RouterLink, RouterView, useRoute } from 'vue-router';
import { ref } from 'vue';
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';

const route = useRoute();

const isActive = (name: string) => route.name === name;
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <Toolbar class="bg-white shadow-sm px-6">
      <template #start>
        <h1 class="text-xl font-bold text-gray-900 m-0">EventHub</h1>
      </template>

      <template #end>
        <RouterLink to="/" class="p-button p-component p-button-text"
          :class="isActive('events') ? 'p-button-primary' : ''">
          Events
        </RouterLink>

        <RouterLink to="/sales" class="p-button p-component p-button-text ml-3"
          :class="isActive('sales') ? 'p-button-primary' : ''">
          Sales Dashboard
        </RouterLink>

        <RouterLink to="/tickets" class="p-button p-component p-button-text ml-3"
          :class="isActive('tickets') ? 'p-button-primary' : ''">
          Ticket Lookup
        </RouterLink>
      </template>
    </Toolbar>

    <main class="flex-grow max-w-7xl mx-auto w-full px-4 sm:px-6 lg:px-8 py-8">
      <RouterView />
    </main>
  </div>
</template>


<style>
a.p-button {
  cursor: pointer;
  text-decoration: none;
}

a.p-button.p-button-primary {
  font-weight: 600;
}
</style>
