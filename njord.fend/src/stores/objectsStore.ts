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
        [MaritimeObjectType.SarAircraft, []],
        [MaritimeObjectType.Station, []],
        [MaritimeObjectType.AtoN, []],
        [MaritimeObjectType.Device, []],
        [MaritimeObjectType.SART, []]
    ]),
    objectsAmount: new Map<MaritimeObjectType, number>([
        [MaritimeObjectType.Vessel, 0],
        [MaritimeObjectType.SarAircraft, 0],
        [MaritimeObjectType.Station, 0],
        [MaritimeObjectType.AtoN, 0],
        [MaritimeObjectType.Device, 0],
        [MaritimeObjectType.SART, 0]
    ]),
    objectsVisibility: new Map<MaritimeObjectType, boolean>([
        [MaritimeObjectType.Vessel, true],
        [MaritimeObjectType.SarAircraft, true],
        [MaritimeObjectType.Station, true],
        [MaritimeObjectType.AtoN, true],
        [MaritimeObjectType.Device, true],
        [MaritimeObjectType.SART, true],
    ]),
})