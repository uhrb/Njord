import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import type { Layer, PickingInfo } from "@deck.gl/core";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

export interface IconLayerGenerator<T extends MaritimeObjectState> {
    generateIconLayer(data: MaritimeObjectViewState<T>[] | undefined, visible: boolean, onClick: (pickingInfo: PickingInfo<MaritimeObjectViewState<T>>, event: MjolnirEvent) => void): Layer;
    generateViewState(obj: T, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<T>
}