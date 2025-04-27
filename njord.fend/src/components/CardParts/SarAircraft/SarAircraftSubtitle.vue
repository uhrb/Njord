<template>
    <div>
        Search And Rescue Aircraft
    </div>
    <div>
        {{  props.object.aircraftType == SarAircraftType.FixedWing ? 'Fixed Wing' : 'Helicopter' }}
    </div>
</template>
<script setup lang="ts">
import type { SarAircraftState } from '@/types/SarAircraftState';
import { SarAircraftType } from '@/types/SarAircraftType';

const props = defineProps<{
    object: SarAircraftState;
}>();
</script>