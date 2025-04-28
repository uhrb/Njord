<template>
    {{ props.object.deviceType }}
</template>
<script setup lang="ts">
import type { MaritimeDeviceState } from '@/types/MaritimeDeviceState';


const props = defineProps<{
    object: MaritimeDeviceState;
}>();
</script>