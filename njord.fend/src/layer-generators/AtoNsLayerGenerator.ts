import type { IconLayerGenerator } from "@/layer-generators/IconLayerGenerator";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import { IconLayer, type Layer, type PickingInfo, type Viewport } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

import type { AtoNState } from "@/types/AtoNState";
import { VesselHelper } from "@/common/VesselHelper";
import { SvgHelper } from "@/common/SvgHelper";

export class AtonSLayerGenerator implements IconLayerGenerator<AtoNState> {
    generateIconLayer(data: MaritimeObjectViewState<AtoNState>[] | undefined, visible: boolean, onClick: (pickingInfo: PickingInfo<MaritimeObjectViewState<AtoNState>>, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<AtoNState>>({
            id: `atons-layer`,
            data: data,
            sizeUnits: 'meters',
            sizeMinPixels: 10,
            getIcon: (d: MaritimeObjectViewState<AtoNState>) => d.icon,
            getSize: (d: MaritimeObjectViewState<AtoNState>) => d.size,
            getPosition: (d: MaritimeObjectViewState<AtoNState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getColor: (d: MaritimeObjectViewState<AtoNState>) => d.color,
            updateTriggers: {
                getSize: (d: MaritimeObjectViewState<AtoNState>) => d.size,
                getIcon: (d: MaritimeObjectViewState<AtoNState>) => d.icon,
                getPosition: (d: MaritimeObjectViewState<AtoNState>) => [d.object.latitude, d.object.longitude],
                getColor: (d: MaritimeObjectViewState<AtoNState>) => d.color,
            },
            transitions: {
                getPosition: {
                    duration: 1000,
                }
            },
            pickable: true,
            onClick: (a, b) => onClick(a, b),
            visible: visible
        });
    }

    generateViewState(obj: AtoNState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<AtoNState> {

        const size = VesselHelper.getVesselSize(obj.dimensions);

        return {
            object: obj,
            size: size,
            angle: 0,
            color: [0, 0, 0, 255],
            icon: SvgHelper.getAtoNIcon(obj, bounds, zoom),
            zoom: zoom
        };
    }

}