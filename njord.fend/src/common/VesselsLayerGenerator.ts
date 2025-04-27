import { NavigationHelper } from "@/common/NavigationHelper";
import { SvgHelper } from "@/common/SvgHelper";
import { VesselHelper } from "@/common/VesselHelper";
import type { IconLayerGenerator } from "@/types/IconLayerGenerator";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import type { VesselState } from "@/types/VesselState";
import { IconLayer, type Layer, type PickingInfo } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

export class VesselsLayerGenerator implements IconLayerGenerator<VesselState> {
    generateIconLayer(data: MaritimeObjectViewState<VesselState>[] | undefined, visible: boolean, onClick: (pickingInfo: PickingInfo<MaritimeObjectViewState<VesselState>>, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<VesselState>>({
            id: `vessels-layer`,
            data: data,
            getIcon: (d: MaritimeObjectViewState<VesselState>) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 10,
            getSize: (d: MaritimeObjectViewState<VesselState>) => d.size,
            getPosition: (d: MaritimeObjectViewState<VesselState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getAngle: (d: MaritimeObjectViewState<VesselState>) => d.angle,
            getColor: (d: MaritimeObjectViewState<VesselState>) => d.color,
            updateTriggers: {
                getSize: (d: MaritimeObjectViewState<VesselState>) => d.size,
                getIcon: (d: MaritimeObjectViewState<VesselState>) => d.icon,
                getPosition: (d: MaritimeObjectViewState<VesselState>) => [d.object.latitude, d.object.longitude],
                getAngle: (d: MaritimeObjectViewState<VesselState>) => d.angle,
                getColor: (d: MaritimeObjectViewState<VesselState>) => d.color,
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


    generateViewState(obj: VesselState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<VesselState> {
        return {
            object: obj,
            size: VesselHelper.getVesselSize(obj.dimensions),
            angle: NavigationHelper.getAngle(obj.trueHeading, obj.courseOverGround),
            zoom: zoom,
            color: VesselHelper.getVesselColor(obj.typeOfShipAndCargoType),
            icon: SvgHelper.getVesselIcon(obj, bounds, zoom),
        };
    }

}