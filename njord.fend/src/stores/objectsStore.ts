import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import { MaritimeObjectType } from "@/types/MaritimeObjectType";
import {reactive} from "vue";
export const objectsStore = reactive<{
    objects: Map<MaritimeObjectType, Iterable<MaritimeObjectState>>,
    objectsAmount: Map<MaritimeObjectType, number>,
    objectsVisibility: Map<MaritimeObjectType, boolean>
}>({
    objects: new Map<MaritimeObjectType, MaritimeObjectState[]>([
        [MaritimeObjectType.Vessel, []],
        [MaritimeObjectType.SarAircraft, []]
    ]),
    objectsAmount: new Map<MaritimeObjectType, number>([
        [MaritimeObjectType.Vessel, 0],
        [MaritimeObjectType.SarAircraft, 0]
    ]),
    objectsVisibility: new Map<MaritimeObjectType, boolean>([
        [MaritimeObjectType.Vessel, true],
        [MaritimeObjectType.SarAircraft, true]
    ]),
})