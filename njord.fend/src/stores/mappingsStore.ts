import {reactive} from "vue";
export const mappingsStore = reactive<{
    ShipTypeNameMappings: Record<number, string | undefined>
    NavigationStatusMappings: Record<number, string | undefined>;
}>({
    ShipTypeNameMappings: {},
    NavigationStatusMappings: {}
})