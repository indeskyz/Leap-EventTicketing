<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router';
import { Bars3Icon } from '@heroicons/vue/24/outline';
</script>

<template>
  <div class="min-h-full">
    <nav class="bg-white shadow-sm">
      <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div class="flex h-16 justify-between">
          <div class="flex">
            <div class="flex flex-shrink-0 items-center">
              <h1 class="text-xl font-bold text-gray-900">Event Management</h1>
            </div>
            <div class="sm:-my-px sm:ml-6 sm:flex sm:space-x-8">
              <RouterLink
                to="/"
                class="inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium"
                :class="{
                  'border-blue-500 text-gray-900': $route.name === 'events',
                  'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700': $route.name !== 'events',
                }"
              >
                Events
              </RouterLink>
              <RouterLink
                to="/sales"
                class="inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium"
                :class="{
                  'border-blue-500 text-gray-900': $route.name === 'sales',
                  'border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700': $route.name !== 'sales',
                }"
              >
                Sales Summary
              </RouterLink>
            </div>
          </div>
        </div>
      </div>
    </nav>

    <div class="py-6">
      <RouterView />
    </div>
  </div>
</template>
