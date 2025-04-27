<template>
    {{ props.object.name }} <span class="text-disabled"> {{ props.object.callSign }}</span>
</template>
<script setup lang="ts">
import type { StationState } from '@/types/BaseStationState';



const props = defineProps<{
    object: StationState;
}>();
</script>