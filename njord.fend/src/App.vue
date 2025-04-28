<template>
  <v-app>
    <v-app-bar title="Njord">
      <v-switch color="primary" v-for="item in objectsStore.objectsAmount" :label="`${item[0]} ${item[1]}`" class="mr-4 mt-4"
        :model-value="objectsStore.objectsVisibility.get(item[0])" @update:model-value="setLayerVisibility(item[0])">

      </v-switch>
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