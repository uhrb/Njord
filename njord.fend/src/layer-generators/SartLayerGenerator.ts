import { NavigationHelper } from "@/common/NavigationHelper";
import { SvgHelper } from "@/common/SvgHelper";
import type { IconLayerGenerator } from "@/layer-generators/IconLayerGenerator";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import type { SARTState } from "@/types/SartState";
import { IconLayer, type Layer, type PickingInfo } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

export class SartLayerGenerator implements IconLayerGenerator<SARTState> {
    generateIconLayer(data: MaritimeObjectViewState<SARTState>[] | undefined, visible: boolean, onClick: (pickingInfo: PickingInfo<MaritimeObjectViewState<SARTState>>, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<SARTState>>({
            id: `sart-layer`,
            data: data,
            getIcon: (d: MaritimeObjectViewState<SARTState>) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 10,
            sizeMaxPixels: 10,
            getPosition: (d: MaritimeObjectViewState<SARTState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getColor: (d: MaritimeObjectViewState<SARTState>) => d.color,
            updateTriggers: {
                getPosition: (d: MaritimeObjectViewState<SARTState>) => [d.object.latitude, d.object.longitude],
                getAngle: (d: MaritimeObjectViewState<SARTState>) => d.angle,
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
    generateViewState(obj: SARTState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<SARTState> {
        return {
            object: obj,
            size: 20,
            angle: NavigationHelper.getAngle(undefined, obj.courseOverGround),
            color: [242, 38, 19, 255],
            icon: SvgHelper.getSartIcon(obj.sartType),
            zoom: zoom
        };
    }

}