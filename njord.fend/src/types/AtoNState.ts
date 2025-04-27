import type { AtoNStatus } from "@/types/AtoNStatus";
import type { AtoNType } from "@/types/AtoNType";
import type { Dimensions } from "@/types/Dimensions";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";

export interface AtoNState extends MaritimeObjectState {
    name?: string;
    atonType: AtoNType;
    nameExtension?: string;
    dimensions?: Dimensions;
    isVirtualDevice?: boolean;
    typeOfAidsToNavigation?: number;
    atonStatus?: AtoNStatus;
    isOffPosition?: boolean;
}
