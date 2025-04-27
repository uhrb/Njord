<template>
    <div>
        {{ props.object.atonType }}
    </div>
    <div>
        {{ FormatterHelper.getMappedName(props.object.typeOfAidsToNavigation,
            mappingsStore.AidsToNavigationTypeMappings, "Unknown", "Undefined") }}
    </div>
</template>
<script setup lang="ts">
import type { AtoNState } from '@/types/AtoNState';
import { FormatterHelper } from '@/common/FormatterHelper';
import { mappingsStore } from '@/stores/mappingsStore';

const props = defineProps<{
    object: AtoNState;
}>();
</script>