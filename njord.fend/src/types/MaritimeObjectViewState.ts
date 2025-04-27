import type { MaritimeObjectState } from "@/types/MaritimeObjectState";

export interface MaritimeObjectViewState<T extends MaritimeObjectState> 
{
    object: T;
    size: number;
    angle: number;
    zoom: number;
    color: [number, number, number, number];
    icon: { url: string; width: number; height: number; anchorX: number; anchorY: number; mask: boolean };
}