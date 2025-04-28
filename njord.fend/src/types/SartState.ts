import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import type { SARTType } from "@/types/SartType";

export interface SARTState extends MaritimeObjectState {
    sartType: SARTType;
}