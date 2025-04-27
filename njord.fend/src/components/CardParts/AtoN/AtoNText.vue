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
                <td>Length / Width  </td>
                <td>
                    {{ FormatterHelper.getLength(props.object.dimensions, "Unknown") }} /
                    {{ FormatterHelper.getWidth(props.object.dimensions, "Unknown") }} 
                </td>
            </tr>
            <tr>
                <td>Updated</td>
                <td>
                    {{ FormatterHelper.getLocalDate(props.object.updated, "Unknown") }}
                </td>
            </tr>
        </tbody>
    </v-table>
</template>
<script setup lang="ts">
import { FormatterHelper } from '@/common/FormatterHelper';
import { mappingsStore } from '@/stores/mappingsStore';
import type { AtoNState } from '@/types/AtoNState';

const props = defineProps<{
    object: AtoNState;
}>();
</script>