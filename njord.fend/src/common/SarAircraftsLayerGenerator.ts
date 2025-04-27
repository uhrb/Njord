import { NavigationHelper } from "@/common/NavigationHelper";
import { SarAircraftHelper } from "@/common/SarHelper";
import type { IconLayerGenerator } from "@/types/IconLayerGenerator";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import type { SarAircraftState } from "@/types/SarAircraftState";
import { IconLayer, type Layer, type Viewport } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

export class SarAircraftsLayerGenerator implements IconLayerGenerator<SarAircraftState> {
    generateIconLayer(data: MaritimeObjectViewState<SarAircraftState>[] | undefined, visible: boolean, onClick: (pickingInfo: { color: Uint8Array | null; layer: Layer | null; sourceLayer?: Layer | null; viewport?: Viewport; index: number; picked: boolean; object?: MaritimeObjectViewState<SarAircraftState> | undefined; x: number; y: number; pixel?: [number, number]; coordinate?: number[]; devicePixel?: [number, number]; pixelRatio: number; }, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<SarAircraftState>>({
            id: `sar-layer`,
            data: data,
            getIcon: (d: MaritimeObjectViewState<SarAircraftState>) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 20,
            getSize: (d: MaritimeObjectViewState<SarAircraftState>) => d.size,
            getPosition: (d: MaritimeObjectViewState<SarAircraftState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getAngle: (d: MaritimeObjectViewState<SarAircraftState>) => d.angle,
            getColor: (d: MaritimeObjectViewState<SarAircraftState>) => d.color,
            updateTriggers: {
                getSize: (d: MaritimeObjectViewState<SarAircraftState>) => d.size,
                getPosition: (d: MaritimeObjectViewState<SarAircraftState>) => [d.object.latitude, d.object.longitude],
                getAngle: (d: MaritimeObjectViewState<SarAircraftState>) => d.angle,
            },
            transitions: {
                getPosition: {
                    duration: 1000,
                }
            },
            pickable: true,
            onClick: (a, b) => onClick(a,b),
            visible: visible
        });
    }
    generateViewState(obj: SarAircraftState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<SarAircraftState> {
        return {
            object: obj,
            size: 20,
            angle: NavigationHelper.getAngle(undefined, obj.courseOverGround),
            color: SarAircraftHelper.getColor(),
            icon: SarAircraftHelper.getIcon(obj.aircraftType),
            zoom: zoom
        };
    }

}