import { VesselState } from './VesselState';

export function getVesselIcon(vessel: VesselState) {
    let url = 'assets/question-status.svg';
    switch (vessel.navigationalStatus) {
        case 0: // under way using engine
        case 8: // under way using sail
        case 3: // restricted maneuverability
        case 4: // constrained by draft
        case 7: // fishing
            url = 'assets/moving-status.svg';
            break;
        case 1: // at anchor
        case 5: // moored
        case 6: // aground
            url = 'assets/stopped-status.svg';
            break;
        case 2: // not under command
            url = 'assets/pirate-status.svg';
            break;
        default:
            if((vessel.speedOverGround ?? 0 > 1) || (vessel.courseOverGround ?? 0 > 1)) {
                url = 'assets/moving-status.svg';
            }
            break;
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
