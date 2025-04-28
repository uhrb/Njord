import { NavigationHelper } from "@/common/NavigationHelper";
import { SvgHelper } from "@/common/SvgHelper";
import type { IconLayerGenerator } from "@/layer-generators/IconLayerGenerator";
import type { MaritimeDeviceState } from "@/types/MaritimeDeviceState";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import { IconLayer, type Layer, type PickingInfo } from "deck.gl";
import type { LatLngBounds } from "leaflet";
import type { MjolnirEvent } from "mjolnir.js";

export class MaritimeDevicesLayerGenerator implements IconLayerGenerator<MaritimeDeviceState> {
    generateIconLayer(data: MaritimeObjectViewState<MaritimeDeviceState>[] | undefined, visible: boolean, onClick: (pickingInfo: PickingInfo<MaritimeObjectViewState<MaritimeDeviceState>>, event: MjolnirEvent) => void): Layer {
        return new IconLayer<MaritimeObjectViewState<MaritimeDeviceState>>({
            id: `device-layer`,
            data: data,
            getIcon: (d: MaritimeObjectViewState<MaritimeDeviceState>) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 10,
            sizeMaxPixels: 10,
            getPosition: (d: MaritimeObjectViewState<MaritimeDeviceState>) => [d.object.longitude ?? 0, d.object.latitude ?? 0],
            getColor: (d: MaritimeObjectViewState<MaritimeDeviceState>) => d.color,
            updateTriggers: {
                getPosition: (d: MaritimeObjectViewState<MaritimeDeviceState>) => [d.object.latitude, d.object.longitude],
                getAngle: (d: MaritimeObjectViewState<MaritimeDeviceState>) => d.angle,
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
    generateViewState(obj: MaritimeDeviceState, zoom: number, bounds: LatLngBounds): MaritimeObjectViewState<MaritimeDeviceState> {
        return {
            object: obj,
            size: 20,
            angle: NavigationHelper.getAngle(undefined, obj.courseOverGround),
            color: [207, 0, 15, 255],
            icon: SvgHelper.getDeviceIcon(obj.deviceType),
            zoom: zoom
        };
    }

}