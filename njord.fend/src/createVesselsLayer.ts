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
            let cog = d.courseOverGround ?? 0;
            cog = cog == 360 ? 0 : cog;
            let th = d.trueHeading ?? 0;
            th = th == 511 ? 0 : th;
            if (cog != 0) {
                angle = cog;
            } else if (th != 0) {
                angle = th;
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
