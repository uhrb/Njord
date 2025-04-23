import { type ETA } from './ETA';
import { type Dimensions } from './Dimensions';

export type VesselState = {
    mmsi?: string;
    longitude?: number;
    latitude?: number;
    isPositionAccuracyHigh?: boolean;
    isRaimInUse?: boolean;
    fixingDeviceType?: number;
    courseOverGround?: number;
    speedOverGround?: number;
    rateOfTurn?: number;
    specialManoeuvreIndicator?: number;
    trueHeading?: number;
    navigationalStatus?: number;
    name?: string;
    callSign?: string;
    updated?: string;
    destination?: string;
    imoNumber?: string;
    maximumPresentStaticDraught?: number;
    typeOfShipAndCargoType?: number;
    dimensions?: Dimensions;
    estimatedTimeOfArrival?: ETA;
};
