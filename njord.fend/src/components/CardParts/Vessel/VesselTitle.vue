<template>
    {{ props.object.name }} <span class="text-disabled"> {{ props.object.callSign }}</span>
</template>
<script setup lang="ts">
import type { VesselState } from '@/types/VesselState';

const props = defineProps<{
    object: VesselState;
}>();
</script>