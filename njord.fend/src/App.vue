<template>
  <v-app>
    <v-app-bar>
      <template v-slot:append>
        <v-btn @click="objectsStore.vesselsVisibility = !objectsStore.vesselsVisibility">
          Vessels&nbsp;{{ objectsStore.vesselsCount }}
        </v-btn>
        <v-btn @click="objectsStore.sarsVisibility = !objectsStore.sarsVisibility">
          SAR&nbsp;{{ objectsStore.sarsCount }}
        </v-btn>

      </template>
    </v-app-bar>
    <v-main>
      <SeaMap :vessels="objectsStore.vessels" :sar-fixed-wing="objectsStore.sars"
        :sars-visibility="objectsStore.sarsVisibility" :vessels-visibity="objectsStore.vesselsVisibility"></SeaMap>
    </v-main>
  </v-app>
</template>
<script setup lang="ts">
import SeaMap from "@/components/SeaMap.vue";

import { objectsStore } from "@/stores/objectsStore";

</script>