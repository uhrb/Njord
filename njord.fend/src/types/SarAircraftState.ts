import type { SarAircraftType } from "@/types/SarAircraftType";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";

export interface SarAircraftState extends MaritimeObjectState {
    aircraftType: SarAircraftType;
    altitude?: number;
    isAltitudeSensorTypeBarometric?: boolean;
    name?: string;
    callSign?: string;
}