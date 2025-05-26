import 'primevue/resources/themes/lara-dark-purple/theme.css'; // 🌙 Dark theme
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import './assets/main.css';

import PrimeVue from 'primevue/config';

import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Card from 'primevue/card';
import Tag from 'primevue/tag';
import ProgressBar from 'primevue/progressbar';
import ProgressSpinner from 'primevue/progressspinner';
import Message from 'primevue/message';
import InputText from 'primevue/inputtext';
import Toolbar from 'primevue/toolbar';
import Paginator from 'primevue/paginator';
import ToastService from 'primevue/toastservice'

const app = createApp(App);

app.use(createPinia());
app.use(router);

app.use(ToastService);
app.use(PrimeVue, {
  ripple: true
});

app.component('Button', Button);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('Card', Card);
app.component('Tag', Tag);
app.component('ProgressBar', ProgressBar);
app.component('ProgressSpinner', ProgressSpinner);
app.component('Message', Message);
app.component('InputText', InputText);
app.component('Toolbar', Toolbar);
app.component('Paginator', Paginator);


app.mount('#app');
