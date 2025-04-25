import type { PickingInfo } from "@deck.gl/core";
import { MapView } from "@deck.gl/core";
import { DeckLayer } from "./community";
import type { VesselState } from "../types/VesselState";
import { IconLayer } from "@deck.gl/layers";

import { VesselHelper } from "@/common/VesselHelper";
import type { SarState } from "@/types/SarState";
import { NavigationHelper } from "@/common/NavigationHelper";
import { SarHelper } from "@/common/SarHelper";




type ExtendedVesselState = VesselState & {
    size: number;
    angle: number;
    zoom: number;
    color: [number, number, number, number];
    icon: { url: string; width: number; height: number; anchorX: number; anchorY: number; mask: boolean };
}

type ExtendedSarState = SarState & {
    size: number;
    angle: number;
    color: [number, number, number, number];
    icon: { url: string; width: number; height: number; anchorX: number; anchorY: number; mask: boolean };
}

export class MaritimeLayer extends DeckLayer {
    constructor(shipTypeNameMappings: Record<number, string | undefined>, navigationStatusMappings: Record<number, string | undefined>) {
        super({
            views: [
                new MapView({
                    repeat: true,
                }),
            ],
            /*
            getTooltip: (picking: PickingInfo<ExtendedVesselState>) => {
                if (picking.picked == false || picking.object == null) {
                    return null;
                }
                return {
                    html: '<table class="map-tooltip">' +
                        `<tr><td>MMSI</td><td>${picking.object.mmsi}</td></tr>` +
                        `<tr><td>Name</td><td>${picking.object.name}</td></tr>` +
                        `<tr><td>Call Sign</td><td>${picking.object.callSign}</td></tr>` +
                        `<tr><td>Type</td><td>${FormatterHelper.getMappedName(picking.object.typeOfShipAndCargoType, shipTypeNameMappings, "Unknown")}</td></tr>` +
                        `<tr><td>Nav status</td><td>${FormatterHelper.getMappedName(picking.object.navigationalStatus, navigationStatusMappings, "Unknown")}</td></tr>` +
                        `<tr><td>COG/SOG</td><td>${FormatterHelper.getDegreeString(picking.object.courseOverGround, 360)} / ${picking.object.speedOverGround?.toFixed(2) + ' kn'}</td></tr>` +
                        `<tr><td>Lat/Lon</td><td>${FormatterHelper.getDegreeString(picking.object.latitude, 91, 5)}/${FormatterHelper.getDegreeString(picking.object.longitude, 181, 5)}</td></tr>` +
                        `<tr><td>True Heading</td><td>${FormatterHelper.getDegreeString(picking.object.trueHeading, 511)}</td></tr>` +
                        `<tr><td>Updated</td><td>${FormatterHelper.getLocalDate(picking.object.updated, "Unknown")}</td></tr>` +
                        '</table>',
                    style: {
                        "backgroundColor": "#f3e3c3",
                        "border": "1px solid black"
                    }
                };
            },
            */
            layers: [
            ],
            pickingRadius: 10,
        });
    }

    onAdd(): this {
        super.onAdd();
        this._map.on('zoom', () => this.render());
        return this;
    }

    private _vessels: ExtendedVesselState[] = [];
    private _sars: ExtendedSarState[] = [];
    private _sarsVisibility: boolean = true;
    private _vesselsVisibility: boolean = true;

    public onVesselClicked?: (pickingInfo: PickingInfo<VesselState>, event: any) => void;
    public onSarClicked?: (pickingInfo: PickingInfo<SarState>, event: any) => void;

    public updateSarData(sars: Iterable<SarState>) {
        let i = 0;
        this._sars = [];
        for (const sar of sars) {
            const obj = {
                ...sar,
                size: 20,
                angle: NavigationHelper.getAngle(undefined, sar.courseOverGround),
                color: SarHelper.getColor(),
                icon: SarHelper.getIcon(sar.type),
            };
            if (i >= this._sars.length) {
                this._sars.push(obj);

            } else {
                this._sars[i] = obj;
            }
            i++;
        };
    }

    public updateVesselsData(vessels: Iterable<VesselState>) {
        let i = 0;
        const currentZoom = this._map.getZoom();
        const bounds = this._map.getBounds();
        this._vessels = [];
        for (const vessel of vessels) {
            const obj = {
                ...vessel,
                size: VesselHelper.getVesselSize(vessel.dimensions),
                angle: NavigationHelper.getAngle(vessel.trueHeading, vessel.courseOverGround),
                zoom: currentZoom,
                color: VesselHelper.getVesselColor(vessel.typeOfShipAndCargoType),
                icon: VesselHelper.getVesselIcon(vessel, bounds, currentZoom),
            };
            if (i >= this._vessels.length) {
                this._vessels.push(obj);

            } else {
                this._vessels[i] = obj;
            }
            i++;

        };
    }

    public render() {
        this.setProps({
            layers: [
                this._createVesselsLayer(),
                this._createSarLayer()
            ]
        });
    }

    public setSarsVisibility(value: boolean) {
        this._sarsVisibility = value;
    }

    public setVesselsVisibility(value: boolean) {
        this._vesselsVisibility = value;
    }

    private _createSarLayer() {
        return new IconLayer<ExtendedSarState>({
            id: `sar-layer`,
            data: this._sars,
            getIcon: (d: ExtendedSarState) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 20,
            getSize: (d: ExtendedSarState) => d.size,
            getPosition: (d: ExtendedSarState) => [d.longitude ?? 0, d.latitude ?? 0],
            getAngle: (d: ExtendedSarState) => d.angle,
            getColor: (d: ExtendedSarState) => d.color,
            updateTriggers: {
                getSize: (d: ExtendedSarState) => d.size,
                getPosition: (d: ExtendedSarState) => [d.latitude, d.longitude],
                getAngle: (d: ExtendedSarState) => d.angle,
            },
            transitions: {
                getPosition: {
                    duration: 1000,
                }
            },
            pickable: true,
            onClick: (a, b) => this._sarClicked(a, b),
            visible: this._sarsVisibility
        });
    }

    private _sarClicked(pickingInfo: PickingInfo<ExtendedSarState>, event: any) {
        this.onSarClicked?.(pickingInfo, event);
    }

    private _createVesselsLayer() {
        return new IconLayer<ExtendedVesselState>({
            id: `vessels-layer`,
            data: this._vessels,
            getIcon: (d: ExtendedVesselState) => d.icon,
            sizeUnits: 'meters',
            sizeMinPixels: 10,
            getSize: (d: ExtendedVesselState) => d.size,
            getPosition: (d: ExtendedVesselState) => [d.longitude ?? 0, d.latitude ?? 0],
            getAngle: (d: ExtendedVesselState) => d.angle,
            getColor: (d: ExtendedVesselState) => d.color,
            updateTriggers: {
                getSize: (d: ExtendedVesselState) => d.size,
                getIcon: (d: ExtendedVesselState) => d.icon,
                getPosition: (d: ExtendedVesselState) => [d.latitude, d.longitude],
                getAngle: (d: ExtendedVesselState) => d.angle,
                getColor: (d: ExtendedVesselState) => d.color,
            },
            transitions: {
                getPosition: {
                    duration: 1000,
                }
            },
            pickable: true,
            onClick: (a, b) => this._vesselClicked(a, b),
            visible: this._vesselsVisibility
        });
    }

    private _vesselClicked(pickingInfo: PickingInfo<ExtendedVesselState>, event: any) {
        this.onVesselClicked?.(pickingInfo, event);
    }
}