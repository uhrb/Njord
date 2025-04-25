import type { PickingInfo } from "@deck.gl/core";
import { MapView } from "@deck.gl/core";
import { DeckLayer } from "./community";
import type { VesselState } from "../types/VesselState";
import { IconLayer } from "@deck.gl/layers";
import { FormatterHelper } from "./FormatterHelper";
import { LatLngBounds } from "leaflet";

import movingStatus from "../assets/moving-status.svg";
import pirateStatus from "../assets/pirate-status.svg";
import questionStatus from "../assets/question-status.svg";
import stoppedStatus from "../assets/stopped-status.svg";
import { VesselHelper } from "@/common/VesselHelper";
import { mapStateStore } from "@/stores/mapStateStore";

type ExtendedVesselState = VesselState & {
    size: number;
    angle: number;
    zoom: number;
    color: [number, number, number, number];
    icon: { url: string; width: number; height: number; anchorX: number; anchorY: number; mask: boolean };
}

export class VesselsLayer extends DeckLayer {
    constructor(shipTypeNameMappings: Record<number, string | undefined>, navigationStatusMappings: Record<number, string | undefined>) {
        super({
            views: [
                new MapView({
                    repeat: true,
                }),
            ],
            getTooltip: ({ object }: PickingInfo<ExtendedVesselState>) => {
                if (!object) {
                    return null;
                }
                return object && {
                    html: '<table class="map-tooltip">' +
                        `<tr><td>MMSI</td><td>${object.mmsi}</td></tr>` +
                        `<tr><td>Name</td><td>${object.name}</td></tr>` +
                        `<tr><td>Call Sign</td><td>${object.callSign}</td></tr>` +
                        `<tr><td>Type</td><td>${FormatterHelper.getMappedName(object.typeOfShipAndCargoType, shipTypeNameMappings, "Unknown")}</td></tr>` +
                        `<tr><td>Nav status</td><td>${FormatterHelper.getMappedName(object.navigationalStatus, navigationStatusMappings, "Unknown")}</td></tr>` +
                        `<tr><td>COG/SOG</td><td>${FormatterHelper.getDegreeString(object.courseOverGround, 360)} / ${object.speedOverGround?.toFixed(2) + ' kn'}</td></tr>` +
                        `<tr><td>Lat/Lon</td><td>${FormatterHelper.getDegreeString(object.latitude, 91, 5)}/${FormatterHelper.getDegreeString(object.longitude, 181, 5)}</td></tr>` +
                        `<tr><td>True Heading</td><td>${FormatterHelper.getDegreeString(object.trueHeading, 511)}</td></tr>` +
                        `<tr><td>Updated</td><td>${FormatterHelper.getLocalDate(object.updated, "Unknown")}</td></tr>` +
                        '</table>',
                    style: {
                        "backgroundColor": "#f3e3c3",
                        "border": "1px solid black"
                    }
                };
            },
            layers: [
            ],
        });
    }

    onAdd(): this {
        super.onAdd();
        this._map.on('zoom', () => this.updateLayerData(this._data));
        return this;
    }

    private _data: ExtendedVesselState[] = [];


    public onVesselClicked?: (pickingInfo: PickingInfo<VesselState>, event: any) => void;

    public updateLayerData(vessels: Iterable<VesselState>) {
        this._data = [];
        //TODO offload data calculation to background?
        let i = 0;
        const currentZoom = this._map.getZoom();
        for (const vessel of vessels) {
            if (i >= this._data.length) {
                this._data.push({
                    ...vessel,
                    size: this._getVesselSize(vessel),
                    angle: VesselHelper.getVesselAngle(vessel.trueHeading, vessel.courseOverGround),
                    zoom: currentZoom,
                    color: this._getVesselColor(vessel),
                    icon: this._getVesselIcon(vessel, this._map.getBounds(), currentZoom),
                });
            } else {
                Object.assign(this._data[i], {
                    ...vessel,
                    size: this._getVesselSize(vessel),
                    angle: VesselHelper.getVesselAngle(vessel.trueHeading, vessel.courseOverGround),
                    zoom: currentZoom,
                    color: this._getVesselColor(vessel),
                    icon: this._getVesselIcon(vessel, this._map.getBounds(), currentZoom)
                });
            }
            i++;

        };
        this.setProps({
            layers: [
                this._createIconLayer(),
            ]
        });
    }

    private _getVesselSize(d: VesselState): number {
        let height = (d.dimensions?.a ?? 0) + (d.dimensions?.b ?? 0);
        return height > 0 ? height : 15;
    }




    private _createIconLayer() {
        return new IconLayer<ExtendedVesselState>({
            id: `vessels-layer`,
            data: this._data,
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
        });
    }

    private _vesselClicked(pickingInfo: PickingInfo<ExtendedVesselState>, event: any) {
        this.onVesselClicked?.(pickingInfo, event);
    }

    private _getVesselIcon(vessel: VesselState, bounds: LatLngBounds, zoom: number) {

        let url: string = questionStatus;

        if (zoom >= 14 && bounds.contains({ lat: vessel.latitude!, lng: vessel.longitude! })) {
            if (vessel.dimensions != undefined) {
                const height = (vessel.dimensions.b ?? 0) + (vessel.dimensions.a ?? 0);
                const width = (vessel.dimensions.c ?? 0) + (vessel.dimensions.d ?? 0);


                if (height > 0 && width > 0) {
                    const svg = VesselHelper.getVesselIcon(height, width);
                    return {
                        url: `data:image/svg+xml;charset=utf-8,${encodeURIComponent(svg)}`,
                        width: width + 2,
                        height: height + 2,
                        anchorY: vessel.dimensions!.a!,
                        anchorX: vessel.dimensions!.c!,
                        mask: true,
                    };
                }
            }
        }


        switch (vessel.navigationalStatus) {
            case 0: // under way using engine
            case 8: // under way using sail
            case 3: // restricted maneuverability
            case 4: // constrained by draft
            case 7: // fishing
                url = movingStatus;
                break;
            case 1: // at anchor
            case 5: // moored
            case 6: // aground
                url = stoppedStatus;
                break;
            case 2: // not under command
                url = pirateStatus;
                break;
            default:
                if ((vessel.speedOverGround ?? 0 > 1) || (vessel.courseOverGround ?? 0 > 1)) {
                    url = movingStatus;
                }
                break;
        }

        return {
            url: url,
            width: 128,
            height: 128,
            anchorY: 64,
            anchorX: 64,
            mask: true,
        };
    }

    private _getVesselColor(vessel: VesselState): [number, number, number, number] {
        switch (vessel.typeOfShipAndCargoType) {
            case 20:// WingInGround
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
            case 29:
                return [245, 40, 145, 255];
            case 30: // Fishing
                return [247, 153, 124, 255];
            case 31: // Tug & special
            case 32:
            case 33:
            case 34:
            case 35:
            case 50:
            case 52:
            case 53:
            case 54:
            case 55:
            case 58:
            case 59:
                return [0, 139, 139, 255];
            case 36:
            case 37: // Pleasure Craft
            case 56:
            case 57:
                return [227, 58, 255, 255];
            case 40://HighSpeedCraftAllShipsOfThisType
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
            case 47:
            case 48:
            case 49:
                return [0, 255, 255, 255];
            case 51: // SAR
                return [247, 153, 124, 255];
            case 60: // Passenger
            case 61:
            case 62:
            case 63:
            case 64:
            case 65:
            case 66:
            case 67:
            case 68:
            case 69:
                return [0, 0, 255, 255];
            case 70: // Cargo
            case 71:
            case 72:
            case 73:
            case 74:
            case 75:
            case 76:
            case 77:
            case 78:
            case 79:
                return [20, 112, 20, 255];

            case 80: // Tanker
            case 81:
            case 82:
            case 83:
            case 84:
            case 85:
            case 86:
            case 87:
            case 88:
            case 89:
            case 90:
            case 91:
            case 92:
            case 93:
            case 94:
            case 95:
            case 96:
            case 97:
            case 98:
            case 99:
                return [255, 49, 37, 255];
            default:
                return [64, 64, 64, 255];
        }
    }


}