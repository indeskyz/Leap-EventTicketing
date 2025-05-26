import { createRouter, createWebHistory } from 'vue-router';
import EventsView from '@/views/EventsView.vue';
import SalesSummaryView from '@/views/SalesSummaryView.vue';
import TicketLookupView from '@/views/TicketLookupView.vue';


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
      component: SalesSummaryView,
    },
    {
      path: '/tickets',
      name: 'tickets',
      component: TicketLookupView,
    },
  ],
});

export default router;