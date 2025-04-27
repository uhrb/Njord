<template>
    <div>{{ props.object.name }}</div>
    <div class="text-disabled"> {{ props.object.nameExtension }}</div>
</template>
<script setup lang="ts">
import type { AtoNState } from '@/types/AtoNState';



const props = defineProps<{
    object: AtoNState;
}>();
</script>