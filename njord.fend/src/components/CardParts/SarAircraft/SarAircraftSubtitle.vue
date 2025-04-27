<template>
    <div>
        Search And Rescue Aircraft
    </div>
    <div>
        {{  props.object.aircraftType }}
    </div>
</template>
<script setup lang="ts">
import type { SarAircraftState } from '@/types/SarAircraftState';

const props = defineProps<{
    object: SarAircraftState;
}>();
</script>