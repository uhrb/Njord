import type { StationType } from "@/types/StationType";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";

export interface StationState extends MaritimeObjectState {
    baseStationType: StationType;
    name?: string;
    callSign: string;
}