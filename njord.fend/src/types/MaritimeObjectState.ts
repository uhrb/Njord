import type { MaritimeObjectType } from "@/types/MaritimeObjectType";

export interface MaritimeObjectState {
    objectType: MaritimeObjectType;
    mmsi: string;
    longitude?: number;
    latitude?: number;
    isPositionAccuracyHigh?: boolean;
    isRaimInUse?: boolean;
    fixingDeviceType?: number;
    courseOverGround?: number;
    speedOverGround?: number;
    updated: string;
}