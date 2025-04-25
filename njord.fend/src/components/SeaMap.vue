<template>
  <div id="map"></div>
  <v-card class="floating-card" v-if="mapStateStore.selectedVessel != undefined">
    <template v-slot:title>
      {{ mapStateStore.selectedVessel.name }} <span class="text-disabled"> {{
        mapStateStore.selectedVessel.callSign }}</span>
      <v-btn variant="text" class="float-end" @click="mapStateStore.selectedVessel = undefined">X</v-btn>
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
            <td><a :href="`https://www.vesselfinder.com/vessels/details/${mapStateStore.selectedVessel.mmsi}`"
                target="_blank">{{ mapStateStore.selectedVessel.mmsi }}</a> / {{ mapStateStore.selectedVessel.imoNumber
              }}</td>
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
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.latitude, 91, 4, "Unknown",
                "Not available") }} /
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.longitude, 181, 4, "Unknown",
                "Not available") }}
            </td>
          </tr>
          <tr>
            <td>Accuracy / RAIM / Type </td>
            <td>
              {{ FormatterHelper.getUndefTrueFalse(mapStateStore.selectedVessel.isPositionAccuracyHigh, "Unknown",
                "High", "Low") }} /
              {{ FormatterHelper.getUndefTrueFalse(mapStateStore.selectedVessel.isRaimInUse, "Unknown", "Yes", "No") }}
              /
              {{ FormatterHelper.getMappedName(mapStateStore.selectedVessel.fixingDeviceType,
                mappingsStore.PositionFixingDeviceTypeMappings, "Unknown", "Not defined") }}
            </td>
          </tr>
          <tr>
            <td>COG / SOG</td>
            <td>
              {{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.courseOverGround, 360, 2, "Unknown",
                "Not available") }} /
              {{ `${mapStateStore.selectedVessel.speedOverGround} kn` }}
            </td>
          </tr>
          <tr>
            <td>True heading</td>
            <td>{{ FormatterHelper.getDegreeString(mapStateStore.selectedVessel.trueHeading, 511, 2, "Unknown",
              "Not available") }}</td>
          </tr>
          <tr>
            <td>ROT / Maneouvr </td>
            <td>
              {{ mapStateStore.selectedVessel.rateOfTurn }} / {{
                FormatterHelper.getMappedName(mapStateStore.selectedVessel.specialManoeuvreIndicator,
                  mappingsStore.SpecialManoeuvreIndicatorMappings, "Unknown", "Not defined") }}
            </td>
          </tr>
          <tr>
            <td>Length / Width / Draught </td>
            <td>
              {{ FormatterHelper.getLength(mapStateStore.selectedVessel.dimensions, "Unknown") }} /
              {{ FormatterHelper.getWidth(mapStateStore.selectedVessel.dimensions, "Unknown") }} /
              {{ `${mapStateStore.selectedVessel.maximumPresentStaticDraught} m` }}
            </td>
          </tr>
          <tr>
            <td>Updated</td>
            <td>
              {{ FormatterHelper.getLocalDate(mapStateStore.selectedVessel.updated, "Unknown") }}
            </td>
          </tr>
          <tr>
            <td colspan="2" class="nav-circle">
              <img :src="computedVesselIcon"></img>
            </td>
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
import { computed, onMounted, watch } from "vue";

import { FormatterHelper } from '@/common/FormatterHelper';

const mapInitialState: L.MapOptions = {
  center: [36.69, -4.41],
  zoom: 15
};

import { mappingsStore } from '@/stores/mappingsStore';
import { mapStateStore } from '@/stores/mapStateStore';
import { VesselHelper } from '@/common/VesselHelper';

const props = defineProps<{
  vessels: Iterable<VesselState>
}>();

const computedVesselIcon = computed<string>(() => {
  let width = 50;
  let height = 150;
  let angle = 0;
  let cog = 0;
  let th = 0;
  if (mapStateStore.selectedVessel != undefined && mapStateStore.selectedVessel.dimensions != undefined) {
    width = (mapStateStore.selectedVessel.dimensions.c ?? 0) + (mapStateStore.selectedVessel.dimensions.d ?? 0);
    height = (mapStateStore.selectedVessel.dimensions.a ?? 0) + (mapStateStore.selectedVessel.dimensions.b ?? 0);
    angle = VesselHelper.getVesselAngle(mapStateStore.selectedVessel.trueHeading, mapStateStore.selectedVessel.courseOverGround);
    cog = mapStateStore.selectedVessel.courseOverGround ?? 0;
    th = mapStateStore.selectedVessel.trueHeading ?? 0;
    th = 360 - (th == 511 ? 0 : th);
    cog = 360 - (cog == 360 ? 0 : cog);
  }
  const coef = width / height;
  const vslWidth = 150 * coef;
  const paddingWidth = 100 - vslWidth / 2.0;
  const svg = `<svg width="200" height="200" xmlns="http://www.w3.org/2000/svg">` +
    `<g transform='rotate(${360 - angle} 100 100)' >${VesselHelper.getVesselSvgPath(150, vslWidth, paddingWidth, 25)}</g>` +
    `<circle cx='100' cy='100' r='80' stroke="grey" stroke-width="1" fill="none" />` +
    `<g transform='rotate(${360 - th} 100 100)'><line ${mapStateStore.selectedVessel?.trueHeading == 511 ? 'stroke-dasharray="4"' : ''} stroke="red" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line></g>` +
    `<g transform='rotate(${360 - cog} 100 100)'><line ${mapStateStore.selectedVessel?.courseOverGround == 360 ? 'stroke-dasharray="4"' : ''} stroke="blue" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line></g>` +
    `<text font-size="smaller" x="180" y="170" fill="red">th</text>` +
    `<text font-size="smaller" x="180" y="180" fill="blue">cog</text>` +
    `</svg>`;
  return `data:image/svg+xml;charset=utf-8,${encodeURIComponent(svg)}`;
})



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

  const vesselsLayer = new VesselsLayer(mappingsStore.ShipTypeNameMappings, mappingsStore.NavigationStatusMappings);


  vesselsLayer.onVesselClicked = (info: PickingInfo<VesselState>, event: any) => {
    mapStateStore.selectedVessel = info.object;
  }

  console.log(vesselsLayer.onVesselClicked);

  map.addLayer(vesselsLayer);

  watch(props, () => {
    vesselsLayer.updateLayerData(props.vessels);
  });

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
  max-width: 45vw !important;
}

.nav-circle {
  text-align: center;
  padding: 20px !important;
  height: auto;
}
</style>