import type { VesselState } from "@/types/VesselState";
import {reactive} from "vue";
export const mapStateStore = reactive<{
    selectedVessel? : VesselState;
}>({})