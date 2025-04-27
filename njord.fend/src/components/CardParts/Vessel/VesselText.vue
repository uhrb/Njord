<template>
    <v-table density="compact">
        <tbody>
            <tr>
                <td>MMSI / IMO</td>
                <td><a :href="`https://www.vesselfinder.com/vessels/details/${props.object.mmsi}`" target="_blank">{{
                        props.object.mmsi }}</a> / {{ props.object.imoNumber
                    }}</td>
            </tr>
            <tr>
                <td>Destination</td>
                <td>{{ props.object.destination }} </td>
            </tr>
            <tr>
                <td>ETA</td>
                <td>{{ FormatterHelper.getEtaDate(props.object.estimatedTimeOfArrival, "Unknown") }} </td>
            </tr>
            <tr>
                <td>Latitude / Longitude</td>
                <td>
                    {{ FormatterHelper.getDegreeString(props.object.latitude, 91, 4, "Unknown",
                        "Not available") }} /
                    {{ FormatterHelper.getDegreeString(props.object.longitude, 181, 4, "Unknown",
                        "Not available") }}
                </td>
            </tr>
            <tr>
                <td>Accuracy / RAIM / Type </td>
                <td>
                    {{ FormatterHelper.getUndefTrueFalse(props.object.isPositionAccuracyHigh, "Unknown",
                        "High", "Low") }} /
                    {{ FormatterHelper.getUndefTrueFalse(props.object.isRaimInUse, "Unknown", "Yes", "No") }}
                    /
                    {{ FormatterHelper.getMappedName(props.object.fixingDeviceType,
                        mappingsStore.PositionFixingDeviceTypeMappings, "Unknown", "Not defined") }}
                </td>
            </tr>
            <tr>
                <td>COG / SOG</td>
                <td>
                    {{ FormatterHelper.getDegreeString(props.object.courseOverGround, 360, 2, "Unknown",
                        "Not available") }} /
                    {{ `${props.object.speedOverGround} kn` }}
                </td>
            </tr>
            <tr>
                <td>True heading</td>
                <td>{{ FormatterHelper.getDegreeString(props.object.trueHeading, 511, 2, "Unknown",
                    "Not available") }}</td>
            </tr>
            <tr>
                <td>ROT / Maneouvr </td>
                <td>
                    {{ props.object.rateOfTurn }} / {{
                        FormatterHelper.getMappedName(props.object.specialManoeuvreIndicator,
                            mappingsStore.SpecialManoeuvreIndicatorMappings, "Unknown", "Not defined") }}
                </td>
            </tr>
            <tr>
                <td>Length / Width / Draught </td>
                <td>
                    {{ FormatterHelper.getLength(props.object.dimensions, "Unknown") }} /
                    {{ FormatterHelper.getWidth(props.object.dimensions, "Unknown") }} /
                    {{ `${props.object.maximumPresentStaticDraught} m` }}
                </td>
            </tr>
            <tr>
                <td>Updated</td>
                <td>
                    {{ FormatterHelper.getLocalDate(props.object.updated, "Unknown") }}
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
<script setup lang="ts">
import type { VesselState } from '@/types/VesselState';
import { FormatterHelper } from '@/common/FormatterHelper';
import { mappingsStore } from '@/stores/mappingsStore';
import { computed } from 'vue';
import { NavigationHelper } from '@/common/NavigationHelper';
import { SvgHelper } from '@/common/SvgHelper';

const props = defineProps<{
    object: VesselState;
}>();


const computedVesselIcon = computed<string>(() => {
  let width = 50;
  let height = 150;
  let angle = 0;
  let cog = 0;
  let th = 0;
  if (props.object != undefined && props.object != undefined && props.object.dimensions != undefined) {
    width = (props.object.dimensions.c ?? 0) + (props.object.dimensions.d ?? 0);
    height = (props.object.dimensions.a ?? 0) + (props.object.dimensions.b ?? 0);
    angle = NavigationHelper.getAngle(props.object.trueHeading, props.object.courseOverGround);
    cog = props.object.courseOverGround ?? 0;
    th = props.object.trueHeading ?? 0;
    th = 360 - (th == 511 ? 0 : th);
    cog = 360 - (cog == 360 ? 0 : cog);
  }
  const coef = width / height;
  const vslWidth = 150 * coef;
  const paddingWidth = 100 - vslWidth / 2.0;
  const svg = `<svg width="200" height="200" xmlns="http://www.w3.org/2000/svg">` +
    `<g transform='rotate(${360 - angle} 100 100)' >${SvgHelper.getVesselSvgPath(150, vslWidth, paddingWidth, 25)}</g>` +
    `<circle cx='100' cy='100' r='80' stroke="grey" stroke-width="1" fill="none" />` +
    `<g transform='rotate(${360 - th} 100 100)'><line ${props.object?.trueHeading == 511 ? 'stroke-dasharray="4"' : ''} stroke="red" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line></g>` +
    `<g transform='rotate(${360 - cog} 100 100)'><line ${props.object?.courseOverGround == 360 ? 'stroke-dasharray="4"' : ''} stroke="blue" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line></g>` +
    `<text font-size="smaller" x="180" y="170" fill="red">th</text>` +
    `<text font-size="smaller" x="180" y="180" fill="blue">cog</text>` +
    `</svg>`;
  return `data:image/svg+xml;charset=utf-8,${encodeURIComponent(svg)}`;
})
</script>