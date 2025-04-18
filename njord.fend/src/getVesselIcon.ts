import { VesselState } from './VesselState';

export function getVesselIcon(vessel: VesselState) {

    let url = 'assets/ship-arrow.svg';
    if(vessel.navigationalStatus == 1 || vessel.navigationalStatus == 5) {
        url = 'assets/ship-stopped.svg';
    }

    return {
        url: url,
        width: 128,
        height: 128,
        anchorY: 64,
        anchorX: 64,
        mask: true,
    };
}
