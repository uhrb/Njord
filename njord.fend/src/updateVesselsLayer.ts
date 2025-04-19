import { DeckLayer } from './community/index';
import { VesselState } from "./VesselState";
import { createVesselsLayer } from "./createVesselsLayer";

export function updateVesselsLayer(deck: DeckLayer, vessels: Map<string, VesselState>, zoom: number) {
    const newData: VesselState[] = [];
    // need data to be keep the same reference order for items. 
    vessels.forEach((vessel) => newData.push(vessel));
    deck.setProps({
        layers: [
            createVesselsLayer(newData, zoom),
        ]
    });
}