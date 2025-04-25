import type { SarState } from "@/types/SarState";
import type { VesselState } from "@/types/VesselState";
import {reactive} from "vue";
export const objectsStore = reactive<{
    vessels: Iterable<VesselState>
    vesselsCount: number,
    sars: Iterable<SarState>,
    sarsCount: number,
    sarsVisibility: boolean,
    vesselsVisibility: boolean,
}>({
    vessels: new Map<string, VesselState>().values(),
    vesselsCount: 0,
    sars: new Map<string, SarState>().values(),
    sarsCount : 0,
    sarsVisibility: true,
    vesselsVisibility: true
})