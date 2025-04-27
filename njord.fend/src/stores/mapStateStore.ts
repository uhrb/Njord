import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import {reactive} from "vue";
export const mapStateStore = reactive<{
    selectedObject? : MaritimeObjectState;
}>({})