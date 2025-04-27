<template>
    <v-table density="compact">
        <tbody>
            <tr>
                <td>MMSI</td>
                <td>
                    <a :href="`https://www.vesselfinder.com/vessels/details/${props.object.mmsi}`" target="_blank">{{
                        props.object.mmsi }}</a>
                </td>
            </tr>
            <tr>
                <td>Altitude / Barometric</td>
                <td>
                    {{ `${props.object.altitude} m` }} /
                    {{ FormatterHelper.getUndefTrueFalse(props.object.isAltitudeSensorTypeBarometric, "?", "Yes", "No")
                    }}
                </td>
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
                <td>Updated</td>
                <td>
                    {{ FormatterHelper.getLocalDate(props.object.updated, "Unknown") }}
                </td>
            </tr>
            <tr>
                <td colspan="2" class="nav-circle">
                    <img :src="computedSarIcon" />
                </td>
            </tr>
        </tbody>
    </v-table>
</template>
<script setup lang="ts">
import { FormatterHelper } from '@/common/FormatterHelper';
import { mappingsStore } from '@/stores/mappingsStore';
import { computed } from 'vue';
import { NavigationHelper } from '@/common/NavigationHelper';
import type { SarAircraftState } from '@/types/SarAircraftState';
import svgHelicopter from "@/assets/helicopter.svg?raw";
import svgAircraft from "@/assets/aircraft.svg?raw";
import { SarAircraftType } from '@/types/SarAircraftType';
import { SvgHelper, type CombineOptions } from '@/common/SvgHelper';

const props = defineProps<{
    object: SarAircraftState;
}>();


const computedSarIcon = computed<string>(() => {
    let angle = NavigationHelper.getAngle(undefined, props.object.courseOverGround);
    let cog = props.object.courseOverGround ?? 0;
    cog = 360 - (cog == 360 ? 0 : cog);

    let aircraft = svgAircraft;
    switch (props.object.aircraftType) {
        case SarAircraftType.FixedWing:
            aircraft = svgAircraft;
            break;
        case SarAircraftType.Helicopter:
            aircraft = svgHelicopter;
            break;
    }

    const [craft, width, height] = SvgHelper.extractSvgRaw(aircraft);
    const svg = `<svg width="200" height="200" xmlns="http://www.w3.org/2000/svg">` +
        `<g transform='rotate(${360 - angle} 100 100)'>` +
        `<g transform='translate(${(200 - width) / 2.0}, ${(200 - height) /2.0})'>${craft}</g>` +
        `</g>` +
        `<circle cx='100' cy='100' r='80' stroke="grey" stroke-width="1" fill="none" />` +
        `<line stroke-dasharray="4" stroke="red" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line>` +
        `<g transform='rotate(${360 - cog} 100 100)'><line ${props.object?.courseOverGround == 360 ? 'stroke-dasharray="4"' : ''} stroke="blue" stroke-width='1' x1='100' y1='100' x2='100' y2='0'></line></g>` +
        `<text font-size="smaller" x="180" y="170" fill="red">th</text>` +
        `<text font-size="smaller" x="180" y="180" fill="blue">cog</text>` +
        `</svg>`;


    return `data:image/svg+xml;charset=utf-8,${encodeURIComponent(svg)}`;
})
</script>