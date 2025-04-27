<template>
    <div>
        {{  props.object.atonType }}
    </div>
</template>
<script setup lang="ts">
import type { AtoNState } from '@/types/AtoNState';

const props = defineProps<{
    object: AtoNState;
}>();
</script>