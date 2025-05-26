

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'events',
      component: EventsView,
    },
    {
      path: '/sales-summary',
      name: 'sales-summary',
      component: SalesSummaryView,
    },
  ],
});

export default router;