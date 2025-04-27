<template>
    <div>
        {{  props.object.baseStationType }}
    </div>
</template>
<script setup lang="ts">
import type { StationState } from '@/types/BaseStationState';

const props = defineProps<{
    object: StationState;
}>();
</script>