import { IconLayer } from '@deck.gl/layers';
import { getVesselColor } from './getVesselColor';
import { VesselState } from './VesselState';
import { zoomSizes } from './zoomSizes';
import { getVesselIcon } from './getVesselIcon';

export function createVesselsLayer(data: VesselState[], zoom: number) {
    return new IconLayer<VesselState>({
        id: `vessels-layer`,
        data: data,
        getIcon: (d: VesselState) => getVesselIcon(d),
        sizeUnits: 'meters',
        sizeScale: zoomSizes[zoom] ?? 1,
        sizeMinPixels: 10,
        sizeMaxPixels: 30,
        getSize: (d: VesselState) => {
            let height = (d.dimensions?.a ?? 0) + (d.dimensions?.b ?? 0);
            return height > 0 ? height : 15;
        },
        getPosition: (d: VesselState) => [d.longitude ?? 0, d.latitude ?? 0],
        getAngle: (d: VesselState) => {
            let angle = 0;
            if (
                (d.speedOverGround != undefined && d.speedOverGround > 1 && d.speedOverGround != 102.3) // speed is defined and available
                && (d.courseOverGround != undefined && d.courseOverGround != 360) // course is defined and available
            ) {
                // vessel is moving, using cog
                angle = d.courseOverGround;
            }
            else {
                // vessel is not moving, using true heading
                if (d.trueHeading != undefined && d.trueHeading != 360) {
                    angle = d.trueHeading;
                } 
            }
            return 360 - angle;
        },
        getColor: (d: VesselState) => getVesselColor(d),
        updateTriggers: {
            getSize: (d: VesselState) => d.dimensions,
            getIcon: (d: VesselState) => d.navigationalStatus,
            getPosition: (d: VesselState) => [d.latitude, d.longitude],
            getAngle: (d: VesselState) => [d.courseOverGround, d.trueHeading],
            getColor: (d: VesselState) => d.typeOfShipAndCargoType,
        },
        transitions: {
            getPosition: {
                duration: 5000,
            }
        },
        pickable: true,
    });
}
