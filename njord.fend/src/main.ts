import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import { DataStream } from '@/services/DataStream';
import 'vuetify/styles'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

import { createVuetify } from 'vuetify'
const vuetify = createVuetify({
    components,
    directives,
})


const dataStream: DataStream = new DataStream();
dataStream.setup();

const app = createApp(App);
app.use(vuetify);
app.provide("dataStream", dataStream);
app.mount('#app')
