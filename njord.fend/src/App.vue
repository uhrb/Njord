<template>
  <v-app>
    <v-app-bar>
      <template v-slot:append>
         <v-btn v-for="item in objectsStore.objectsAmount" @click="setLayerVisibility(item[0])">
            {{ item[0] }} {{ item[1] }}
         </v-btn>
      </template>
    </v-app-bar>
    <v-main>
      <SeaMap></SeaMap>
    </v-main>
  </v-app>
</template>
<script setup lang="ts">
import SeaMap from "@/components/SeaMap.vue";

import { objectsStore } from "@/stores/objectsStore";
import type { MaritimeObjectType } from "@/types/MaritimeObjectType";

function setLayerVisibility(type: MaritimeObjectType) {
  objectsStore.objectsVisibility.set(type, !objectsStore.objectsVisibility.get(type));
}

</script>