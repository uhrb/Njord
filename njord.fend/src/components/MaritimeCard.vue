<template>
    <v-card class="floating-card" v-if="props.object != undefined">
        <template v-slot:title>
            <VesselTitle :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel">
            </VesselTitle>
            <SarAircraftTitle :object="<SarAircraftState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftTitle>
            <StationTitle :object="<StationState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.Station"></StationTitle>
            <AtonTitle :object="<AtoNState>props.object" v-if="props.object?.objectType == MaritimeObjectType.AtoN">
            </AtonTitle>
            <SartTitle :object="<SARTState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SART">
            </SartTitle>
            <DeviceTitle :object="<MaritimeDeviceState>props.object" v-if="props.object?.objectType == MaritimeObjectType.Device">
            </DeviceTitle>
            <v-fab class="ma-1" variant="text" icon="mdi-close" location="top end" size="small" absolute @click="emits('close')"></v-fab>
        </template>

        <template v-slot:subtitle>
            <VesselSubtitle :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel">
            </VesselSubtitle>
            <SarAircraftSubtitle :object="<SarAircraftState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftSubtitle>
            <StationSubtitle :object="<StationState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.Station"></StationSubtitle>
            <AtoNSubtitle :object="<AtoNState>props.object" v-if="props.object?.objectType == MaritimeObjectType.AtoN">
            </AtoNSubtitle>
            <SartSubtitle :object="<SARTState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SART">
            </SartSubtitle>
            <DeviceSubtitle :object="<MaritimeDeviceState>props.object" v-if="props.object?.objectType == MaritimeObjectType.Device">
            </DeviceSubtitle>
        </template>

        <template v-slot:text>
            <VesselText :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel">
            </VesselText>
            <SarAircraftText :object="<SarAircraftState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftText>
            <StationText :object="<StationState>props.object"
                v-if="props.object?.objectType == MaritimeObjectType.Station"></StationText>
            <AtoNText :object="<AtoNState>props.object" v-if="props.object?.objectType == MaritimeObjectType.AtoN">
            </AtoNText>
            <SartText :object="<SARTState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SART">
            </SartText>
            <DeviceText :object="<MaritimeDeviceState>props.object" v-if="props.object?.objectType == MaritimeObjectType.Device">
            </DeviceText>
        </template>
    </v-card>
</template>

<script lang="ts" setup>
import type { MaritimeObjectState } from '@/types/MaritimeObjectState';
import VesselTitle from '@/components/CardParts/Vessel/VesselTitle.vue';
import VesselSubtitle from '@/components/CardParts/Vessel/VesselSubtitle.vue';
import VesselText from '@/components/CardParts/Vessel/VesselText.vue';
import SarAircraftTitle from '@/components/CardParts/SarAircraft/SarAircraftTitle.vue';
import SarAircraftSubtitle from '@/components/CardParts/SarAircraft/SarAircraftSubtitle.vue';
import SarAircraftText from '@/components/CardParts/SarAircraft/SarAircraftText.vue';
import { MaritimeObjectType } from '@/types/MaritimeObjectType';
import type { SarAircraftState } from '@/types/SarAircraftState';
import StationTitle from '@/components/CardParts/Station/StationTitle.vue';
import type { StationState } from '@/types/BaseStationState';
import StationSubtitle from '@/components/CardParts/Station/StationSubtitle.vue';
import StationText from '@/components/CardParts/Station/StationText.vue';
import AtonTitle from '@/components/CardParts/AtoN/AtoNTitle.vue';
import type { AtoNState } from '@/types/AtoNState';
import AtoNSubtitle from '@/components/CardParts/AtoN/AtoNSubtitle.vue';
import AtoNText from '@/components/CardParts/AtoN/AtoNText.vue';
import SartTitle from '@/components/CardParts/SART/SartTitle.vue';
import type { SARTState } from '@/types/SartState';
import SartSubtitle from '@/components/CardParts/SART/SartSubtitle.vue';
import SartText from '@/components/CardParts/SART/SartText.vue';
import DeviceTitle from '@/components/CardParts/Device/DeviceTitle.vue';
import type { MaritimeDeviceState } from '@/types/MaritimeDeviceState';
import DeviceSubtitle from '@/components/CardParts/Device/DeviceSubtitle.vue';
import DeviceText from '@/components/CardParts/Device/DeviceText.vue';

const props = defineProps<{
    object: MaritimeObjectState | undefined
}>();

const emits = defineEmits<{
    (e: 'close'): void
}>();


</script>

<style lang="css">
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