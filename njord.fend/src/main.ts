import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import { DataStream } from '@/services/dataStream';

const dataStream : DataStream = new DataStream();

const app = createApp(App);
dataStream.setup();
app.provide("dataStream", dataStream);
app.mount('#app')
