import type { SarType } from "@/types/SarType";

export type SarState = {
    type: SarType;
    altitude?: number;
    isAltitudeSensorTypeBarometric?: boolean;
    longitude?: number;
    latitude?: number;
    isPositionAccuracyHigh?: boolean;
    isRaimInUse?: boolean;
    fixingDeviceType?: number;
    courseOverGround?: number;
    speedOverGround?: number;
    updated: string;
}