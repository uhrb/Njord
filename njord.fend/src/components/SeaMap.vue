<template>
  <div id="map"></div>
  <MaritimeCard :object="mapStateStore.selectedObject" @close="unselectObject"></MaritimeCard>
</template>

<script lang="ts" setup>
import * as L from 'leaflet';
import type { PickingInfo } from '@deck.gl/core';
import 'leaflet/dist/leaflet.css';
import { MaritimeLayer } from '@/common/MaritimeLayer';
import { onMounted, watch } from "vue";



const mapInitialState: L.MapOptions = {
  center: [36.69, -4.41],
  zoom: 15
};

import { mappingsStore } from '@/stores/mappingsStore';
import { mapStateStore } from '@/stores/mapStateStore';
import { MaritimeObjectType } from '@/types/MaritimeObjectType';
import type { IconLayerGenerator } from '@/layer-generators/IconLayerGenerator';
import type { MaritimeObjectState } from '@/types/MaritimeObjectState';
import { VesselsLayerGenerator } from '@/layer-generators/VesselsLayerGenerator';
import { SarAircraftsLayerGenerator } from '@/layer-generators/SarAircraftsLayerGenerator';
import { objectsStore } from '@/stores/objectsStore';

import MaritimeCard from '@/components/MaritimeCard.vue';
import { StationsLayerGenerator } from '@/layer-generators/StationsLayerGenerator';
import { AtonSLayerGenerator } from '@/layer-generators/AtoNsLayerGenerator';


const generators = new Map<MaritimeObjectType, IconLayerGenerator<MaritimeObjectState>>([
  [MaritimeObjectType.Vessel, new VesselsLayerGenerator()],
  [MaritimeObjectType.SarAircraft, new SarAircraftsLayerGenerator()],
  [MaritimeObjectType.Station, new StationsLayerGenerator()],
  [MaritimeObjectType.AtoN, new AtonSLayerGenerator()]
]);

const mariTimeLayer = new MaritimeLayer(generators, mappingsStore.ShipTypeNameMappings, mappingsStore.NavigationStatusMappings);

mariTimeLayer.onObjectClicked = (info: PickingInfo<MaritimeObjectState>, event: any) => {
  if(info.picked == false || info.object == undefined || info.object == null) {
    return;
  }

  mapStateStore.selectedObject = info.object;
}

const layers = [
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 17,
    minZoom: 3,
  }),
  L.tileLayer('https://t1.openseamap.org/seamark/{z}/{x}/{y}.png', {
    maxZoom: 17,
    minZoom: 3,
  }),
  L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco2021_contour',
    transparent: true,
    format: 'image/png',
    opacity: 0.4,
    maxZoom: 17,
    minZoom: 3,
    attribution: '© OpenSeaMap contributors'
  }),
  L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco_2021_poly',
    transparent: true,
    format: 'image/png',
    opacity: 0.3,
    maxZoom: 17,
    minZoom: 3,
  }),
  mariTimeLayer
]

function unselectObject() {
  mapStateStore.selectedObject = undefined;
}

watch([objectsStore.objects, objectsStore.objectsVisibility], () => {
  for (const kv of objectsStore.objects) {
    const key = kv[0];
    const val = kv[1];
    mariTimeLayer.updateData(key, val);
    const visibility = objectsStore.objectsVisibility.get(key) ?? true;
    mariTimeLayer.setLayerVisibility(key, visibility);
  }
  mariTimeLayer.render();
});

onMounted(() => {
  const map = L.map(document.getElementById('map')!, mapInitialState);
  for (const layer of layers) {
    layer.addTo(map)
  }
  map.addLayer(mariTimeLayer);
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