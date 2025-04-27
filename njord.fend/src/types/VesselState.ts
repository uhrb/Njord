import type { ETA } from '@/types/ETA';
import type { Dimensions } from '@/types/Dimensions';
import type { MaritimeObjectState } from '@/types/MaritimeObjectState';

export interface VesselState extends MaritimeObjectState {
    rateOfTurn?: number;
    specialManoeuvreIndicator?: number;
    trueHeading?: number;
    navigationalStatus?: number;
    name?: string;
    callSign?: string;
    destination?: string;
    imoNumber?: string;
    maximumPresentStaticDraught?: number;
    typeOfShipAndCargoType?: number;
    dimensions?: Dimensions;
    estimatedTimeOfArrival?: ETA;
};
