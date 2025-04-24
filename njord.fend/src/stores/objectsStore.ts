import type { VesselState } from "@/types/VesselState";
import {reactive} from "vue";
export const objectsStore = reactive<{
    vessels: Iterable<VesselState>
    vesselsCount: number,
}>({
    vessels: new Map<string, VesselState>().values(),
    vesselsCount: 0
})