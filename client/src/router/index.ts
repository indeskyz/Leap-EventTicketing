import { createRouter, createWebHistory } from 'vue-router';
import EventsView from '@/views/EventsView.vue';
import SalesView from '@/views/SalesView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'events',
      component: EventsView,
    },
    {
      path: '/sales',
      name: 'sales',
      component: SalesView,
    },
  ],
});

export default router;