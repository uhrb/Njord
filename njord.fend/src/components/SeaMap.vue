<template>
  <div id="map"></div>
</template>

<script lang="ts" setup>
import * as L from 'leaflet';
import type { PickingInfo } from '@deck.gl/core';
import type { VesselState } from '@/types/VesselState';
import 'leaflet/dist/leaflet.css';
import { VesselsLayer } from '@/common/VesselsLayer';
import { onMounted, inject } from "vue";
import { useIntervalFn } from '@vueuse/core'
import type { DataStream } from '@/services/DataStream';

const dataStream: DataStream = inject("dataStream")!;

const mapInitialState: L.MapOptions = {
  center: [36.69, -4.41],
  zoom: 15
};

onMounted(() => {

  const map = L.map(document.getElementById('map')!, mapInitialState);

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 16,
    minZoom: 3,
  }).addTo(map);

  L.tileLayer('https://t1.openseamap.org/seamark/{z}/{x}/{y}.png', {
    maxZoom: 16,
    minZoom: 3,
  }).addTo(map);

  L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco2021_contour',
    transparent: true,
    format: 'image/png',
    opacity: 0.4,
    maxZoom: 16,
    minZoom: 3,
    attribution: '© OpenSeaMap contributors'
  }).addTo(map);

  L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco_2021_poly',
    transparent: true,
    format: 'image/png',
    opacity: 0.3,
    maxZoom: 16,
    minZoom: 3,
  }).addTo(map);



  var vesselsLayer = new VesselsLayer(dataStream.ShipTypeNameMappings, dataStream.NavigationStatusMappings);

  vesselsLayer.onVesselClicked = (info: PickingInfo<VesselState>, event: any) => {
    // https://www.vesselfinder.com/vessels/details/555666888
    // window.open(`https://www.vesselfinder.com/vessels/details/${info.object!.mmsi}`, '_blank');
  }

  map.addLayer(vesselsLayer);

  useIntervalFn(() => {
    vesselsLayer.updateLayerData(dataStream.Vessels.values());
  }, 1000);

});
</script>