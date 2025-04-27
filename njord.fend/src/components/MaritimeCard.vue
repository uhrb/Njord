<template>
    <v-card class="floating-card" v-if="props.object != undefined">
        <template v-slot:title>
            <VesselTitle :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel"></VesselTitle>
            <SarAircraftTitle :object="<SarAircraftState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftTitle>
            <v-btn variant="text" class="float-end" @click="emits('close')">X</v-btn>
        </template>

        <template v-slot:subtitle>
            <VesselSubtitle :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel"></VesselSubtitle>
            <SarAircraftSubtitle :object="<SarAircraftState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftSubtitle>
        </template>

        <template v-slot:text>
            <VesselText :object="props.object" v-if="props.object?.objectType == MaritimeObjectType.Vessel"></VesselText>
            <SarAircraftText :object="<SarAircraftState>props.object" v-if="props.object?.objectType == MaritimeObjectType.SarAircraft"></SarAircraftText>
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