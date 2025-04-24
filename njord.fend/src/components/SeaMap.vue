<template>
  <div id="map"></div>
  <v-card class="floating-card" v-if="mapStateStore.selectedVessel != undefined">
    <template v-slot:title>
      <a :href="`https://www.vesselfinder.com/vessels/details/${mapStateStore.selectedVessel.mmsi}`" target="_blank">{{ mapStateStore.selectedVessel.name }}</a> <span class="text-disabled"> {{
        mapStateStore.selectedVessel.callSign }}</span>
    </template>

    <template v-slot:subtitle>
      <div>
         {{ FormatterHelper.getMappedName(mapStateStore.selectedVessel.typeOfShipAndCargoType,
          mappingsStore.ShipTypeNameMappings, "Unknown") }}
      </div>
      <div>
        {{ FormatterHelper.getMappedName(mapStateStore.selectedVessel.navigationalStatus,
          mappingsStore.NavigationStatusMappings, "Unknown") }}
      </div>
    </template>

    <template v-slot:text>
      <v-table density="compact">
        <tbody>
          <tr>
            <td>MMSI / IMO</td>
            <td>{{ mapStateStore.selectedVessel.mmsi }} / {{ mapStateStore.selectedVessel.imoNumber }}</td>
          </tr>
          <tr>
            <td>Destination</td>
            <td>{{ mapStateStore.selectedVessel.destination }} </td>
          </tr>
          <tr>
            <td>ETA</td>
            <td>{{ FormatterHelper.getEtaDate(mapStateStore.selectedVessel.estimatedTimeOfArrival, "Unknown") }} </td>
          </tr>
          <tr>
            <td>Latitude / Longitude</td>
            <td>
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.latitude, 91, 4, "Unknown", "Not available") }} /
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.longitude, 181, 4, "Unknown", "Not available") }}
            </td>
          </tr>
          <tr>
            <td>Accuracy / RAIM / Type </td>
            <td>
              {{ FormatterHelper.getUndefTrueFalse(mapStateStore.selectedVessel.isPositionAccuracyHigh, "Unknown", "High", "Low") }} / 
              {{ FormatterHelper.getUndefTrueFalse(mapStateStore.selectedVessel.isRaimInUse, "Unknown", "Yes", "No") }} /
              {{ mapStateStore.selectedVessel.fixingDeviceType }}
            </td>
          </tr>
          <tr>
            <td>COG / SOG</td>
            <td>
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.courseOverGround, 360, 2, "Unknown", "Not available") }} /
              {{ `${mapStateStore.selectedVessel.speedOverGround} kn`  }}
            </td>
          </tr>
          <tr>
            <td>True heading</td>
            <td>{{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.trueHeading, 511, 2, "Unknown", "Not available") }}</td>
          </tr>
          <tr>
            <td>ROT / Maneouvr </td>
            <td>
              {{ mapStateStore.selectedVessel.rateOfTurn }} / {{ mapStateStore.selectedVessel.specialManoeuvreIndicator }}
            </td>
          </tr>
          <tr>
            <td>Length / Width / Draught </td>
            <td>
              {{ FormatterHelper.getLength(mapStateStore.selectedVessel.dimensions, "Unknown") }} / 
              {{ FormatterHelper.getWidth(mapStateStore.selectedVessel.dimensions, "Unknown") }} /
              {{ mapStateStore.selectedVessel.maximumPresentStaticDraught }}
            </td>
          </tr>
          <tr>
            <td>Updated</td>
            <td>
              {{ FormatterHelper.getLocalDate(mapStateStore.selectedVessel.updated, "Unknown") }}
            </td>
          </tr>
          <tr>
            NAVIGATION
          </tr>
        </tbody>
      </v-table>
    </template>
  </v-card>
</template>

<script lang="ts" setup>
import * as L from 'leaflet';
import type { PickingInfo } from '@deck.gl/core';
import type { VesselState } from '@/types/VesselState';
import 'leaflet/dist/leaflet.css';
import { VesselsLayer } from '@/common/VesselsLayer';
import { onMounted, defineProps, watch } from "vue";

import { FormatterHelper } from '@/common/FormatterHelper';

const mapInitialState: L.MapOptions = {
  center: [36.69, -4.41],
  zoom: 15
};

import { mappingsStore } from '@/stores/mappingsStore';
import { mapStateStore } from '@/stores/mapStateStore';

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

.floating-card {
  position: fixed !important;
  z-index: 999 !important;
  top: 10vh !important;
  right: 0;
  margin-right: 0.5vw !important;
  max-width: 35vw !important;
}
</style>