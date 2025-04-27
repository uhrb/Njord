import type { IconLayerGenerator } from "@/layer-generators/IconLayerGenerator";
import type { StationState } from "@/types/BaseStationState";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import { IconLayer, type Layer, type PickingInfo, type Viewport } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";
import svgStation from "@/assets/station.svg";

export class StationsLayerGenerator implements IconLayerGenerator<StationState> {
    generateIconLayer(data: MaritimeObjectViewState<StationState>[] | undefined, visible: boolean, onClick:(pickingInfo: PickingInfo<MaritimeObjectViewState<StationState>>, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<StationState>>({
            id: `base-stations-layer`,
            data: data,
            getIcon: (d: MaritimeObjectViewState<StationState>) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 15,
            sizeMaxPixels: 15,
            getSize: 5,
            getPosition: (d: MaritimeObjectViewState<StationState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getColor: [0,0,0,255],
            updateTriggers: {
                getPosition: (d: MaritimeObjectViewState<StationState>) => [d.object.latitude, d.object.longitude],
            },
            pickable: true,
            onClick: (a, b) => onClick(a,b),
            visible: visible
        });
    }

    generateViewState(obj: StationState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<StationState> {
        return {
            object: obj,
            size: 5,
            angle: 0,
            color: [0,0,0,255],
            icon: {
                url: svgStation,
                width: 64,
                height: 64,
                anchorY: 32,
                anchorX: 32,
                mask: true,
            },
            zoom: zoom
        };
    }

}