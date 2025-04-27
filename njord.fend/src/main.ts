import './assets/main.css'
import '@mdi/font/css/materialdesignicons.css'

import { createApp } from 'vue'
import App from './App.vue'
import { DataStream } from '@/services/DataStream';
import 'vuetify/styles'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

import { createVuetify } from 'vuetify'

const njordTheme = {
    "dark": false,
    "colors": {
        "background": "#f3e3c3",      // тёплая выцветшая бумага
        "surface": "#e8d3a4",         // светлая потёртая поверхность
        "primary": "#5e3b1d",         // тёмное дерево (основа)
        "secondary": "#d2aa6d",       // золото, латунь
        "accent": "#2f5742",          // морская зелень, как водоросли
        "info": "#2e4057",            // морская глубина, чернильный синий
        "success": "#4d724d",         // насыщенная зелень
        "warning": "#b97e2b",         // янтарный, как смола или лак
        "error": "#822e2e"            // винно-красный, как сургуч
    },
    "variables": {
        "borderRadiusRoot": "10px",
        "overlayOpacity": 0.12,
        "shadowKeyUmbraOpacity": 0.18,
        "shadowKeyPenumbraOpacity": 0.12,
        "shadowAmbientShadowOpacity": 0.08
    }
}



const vuetify = createVuetify({
    components,
    directives,
    theme: {
        defaultTheme: 'njordTheme',
        themes: {
            njordTheme,
        },
    },
    icons: {
        defaultSet: 'mdi', // This is already the default value - only for display purposes
    },
})

const myCustomLightTheme = {
    dark: false,
    colors: {
        background: '#FFFFFF',
        surface: '#FFFFFF',
        'surface-bright': '#FFFFFF',
        'surface-light': '#EEEEEE',
        'surface-variant': '#424242',
        'on-surface-variant': '#EEEEEE',
        primary: '#1867C0',
        'primary-darken-1': '#1F5592',
        secondary: '#48A9A6',
        'secondary-darken-1': '#018786',
        error: '#B00020',
        info: '#2196F3',
        success: '#4CAF50',
        warning: '#FB8C00',
    },
    variables: {
        'border-color': '#000000',
        'border-opacity': 0.12,
        'high-emphasis-opacity': 0.87,
        'medium-emphasis-opacity': 0.60,
        'disabled-opacity': 0.38,
        'idle-opacity': 0.04,
        'hover-opacity': 0.04,
        'focus-opacity': 0.12,
        'selected-opacity': 0.08,
        'activated-opacity': 0.12,
        'pressed-opacity': 0.12,
        'dragged-opacity': 0.08,
        'theme-kbd': '#212529',
        'theme-on-kbd': '#FFFFFF',
        'theme-code': '#F5F5F5',
        'theme-on-code': '#000000',
    }
}

const dataStream: DataStream = new DataStream();
dataStream.setup();

const app = createApp(App);
app.use(vuetify);
app.provide("dataStream", dataStream);
app.mount('#app')
