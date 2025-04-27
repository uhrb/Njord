<template>
    {{ props.object.name }} <span class="text-disabled"> {{ props.object.callSign }}</span>
</template>
<script setup lang="ts">
import type { SarAircraftState } from '@/types/SarAircraftState';


const props = defineProps<{
    object: SarAircraftState;
}>();
</script>