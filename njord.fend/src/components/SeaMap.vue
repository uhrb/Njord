<template>
  <div id="map"></div>
</template>

<script lang="ts" setup>
import * as L from 'leaflet';
import type { PickingInfo } from '@deck.gl/core';
import type { VesselState } from '@/types/VesselState';
import 'leaflet/dist/leaflet.css';
import { VesselsLayer } from '@/common/VesselsLayer';
import { onMounted, defineProps, watch } from "vue";

const mapInitialState: L.MapOptions = {
  center: [36.69, -4.41],
  zoom: 15
};

import { mappingsStore } from '@/stores/mappingsStore';
import { mapStateStore } from '@/stores/mapStateStore';
import { useTimeoutFn } from '@vueuse/core';

const props = defineProps<{
  vessels: Iterable<VesselState>
}>();

onMounted(() => {

  const map = L.map(document.getElementById('map')!, mapInitialState);

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 17,
    minZoom: 3,
  }).addTo(map);

  L.tileLayer('https://t1.openseamap.org/seamark/{z}/{x}/{y}.png', {
    maxZoom: 17,
    minZoom: 3,
  }).addTo(map);
 
  L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco2021_contour',
    transparent: true,
    format: 'image/png',
    opacity: 0.4,
    maxZoom: 17,
    minZoom: 3,
    attribution: '© OpenSeaMap contributors'
  }).addTo(map);

    L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco_2021_poly',
    transparent: true,
    format: 'image/png',
    opacity: 0.3,
    maxZoom: 17,
    minZoom: 3,
  }).addTo(map); 

  var vesselsLayer = new VesselsLayer(mappingsStore.ShipTypeNameMappings, mappingsStore.NavigationStatusMappings);

  vesselsLayer.onVesselClicked = (info: PickingInfo<VesselState>, event: any) => {
    mapStateStore.selectedVessel = info.object;
  }

  console.log(vesselsLayer.onVesselClicked);

  map.addLayer(vesselsLayer);

  watch(props, () => {
    vesselsLayer.updateLayerData(props.vessels);
  })

});
</script>
<style lang="css">
#map {
  padding: 0px;
  margin: 0px;
  height: 100%;
  width: 100%;
}
</style>