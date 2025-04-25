import {reactive} from "vue";
export const mappingsStore = reactive<{
    ShipTypeNameMappings: Record<number, string | undefined>;
    NavigationStatusMappings: Record<number, string | undefined>;
    SpecialManoeuvreIndicatorMappings:Record<number, string | undefined>;
    PositionFixingDeviceTypeMappings:Record<number, string | undefined>;
}>({
    ShipTypeNameMappings: {},
    NavigationStatusMappings: {},
    SpecialManoeuvreIndicatorMappings : {},
    PositionFixingDeviceTypeMappings: {}
})