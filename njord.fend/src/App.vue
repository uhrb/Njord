<template>
  <v-app>
    <v-app-bar>
      <template v-slot:append>
        <v-container>
          <v-row>
            <v-col>Vessels</v-col>
            <v-col>{{ objectsStore.vesselsCount }}</v-col>
          </v-row>
        </v-container>
      </template>
    </v-app-bar>
    <v-main>
      <SeaMap :vessels="objectsStore.vessels"></SeaMap>
    </v-main>
  </v-app>
</template>
<script setup lang="ts">
import SeaMap from "@/components/SeaMap.vue";

import { objectsStore } from "@/stores/objectsStore";

</script>