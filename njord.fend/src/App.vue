<template>
  <SeaMap :vessels="vessels" :ship-type-name-mappings="shipTypeNameMappings" :navigation-status-mappings="navigationStatusMappings"></SeaMap>
</template>
<script setup lang="ts">
import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";
import SeaMap from "@/components/SeaMap.vue"
import type { VesselState } from "@/types/VesselState";

import { ref } from "vue";

const vessels = ref<Map<string,VesselState>>(new Map<string, VesselState>());
const shipTypeNameMappings = ref<Record<number, string | undefined>>({});
const navigationStatusMappings = ref<Record<number, string | undefined>>({});

function parseVesselUpdate(objectId: string, objectState: any) {
  var vsl: VesselState = {
    mmsi: objectId,
    ...objectState,
  }
  vessels.value.set(objectId, vsl);
}






</script>