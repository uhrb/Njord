<template>
    <div>
        {{ FormatterHelper.getMappedName(props.object.typeOfShipAndCargoType,
            mappingsStore.ShipTypeNameMappings, "Unknown") }}
    </div>
    <div>
        {{ FormatterHelper.getMappedName(props.object.navigationalStatus,
            mappingsStore.NavigationStatusMappings, "Unknown") }}
    </div>
</template>
<script setup lang="ts">
import type { VesselState } from '@/types/VesselState';
import { FormatterHelper } from '@/common/FormatterHelper';
import { mappingsStore } from '@/stores/mappingsStore';

const props =defineProps<{
    object: VesselState;
}>();
</script>