import type { Layer, PickingInfo, Viewport } from "@deck.gl/core";
import { MapView } from "@deck.gl/core";
import { DeckLayer } from "./community";
import type { MaritimeObjectViewState } from "@/types/MaritimeObjectViewState";
import type { MaritimeObjectType } from "@/types/MaritimeObjectType";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import type { IconLayerGenerator } from "@/types/IconLayerGenerator";
import type { MjolnirEvent } from "mjolnir.js";


export class MaritimeLayer extends DeckLayer {

    public onObjectClicked?:(pickingInfo: PickingInfo<MaritimeObjectState>, event: MjolnirEvent) => void;

    private _dataMap: Map<MaritimeObjectType, MaritimeObjectViewState<MaritimeObjectState>[]> = new Map<MaritimeObjectType, MaritimeObjectViewState<MaritimeObjectState>[]>();
    private _layerGenerators: Map<MaritimeObjectType, IconLayerGenerator<MaritimeObjectState>> = new Map<MaritimeObjectType, IconLayerGenerator<MaritimeObjectState>>();
    private _visibility: Map<MaritimeObjectType, boolean> = new Map<MaritimeObjectType, boolean>();


    constructor(
        layerGenerators:  Map<MaritimeObjectType, IconLayerGenerator<MaritimeObjectState>>,
        shipTypeNameMappings: Record<number, string | undefined>,
        navigationStatusMappings: Record<number, string | undefined>
    ) {
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

        this._layerGenerators = layerGenerators;
    }

    onAdd(): this {
        super.onAdd();
        this._map.on('zoom', () => this.render());
        return this;
    }

    public updateData(objectType: MaritimeObjectType, data: Iterable<MaritimeObjectState>) {
        const generator = this._layerGenerators.get(objectType);
        if (generator == undefined) {
            return;
        }
        this._dataMap.set(objectType, []);
        const arr = this._dataMap.get(objectType);
        if (arr == undefined) {
            return;
        }
        const zoom = this._map.getZoom();
        const bounds = this._map.getBounds();
        for (const maritimeObject of data) {
            arr.push(generator.generateViewState(maritimeObject, zoom, bounds));
        }
    }

    public render() {
        const layers = [];
        for (const pair of this._layerGenerators) {
            const type = pair[0];
            const generator = pair[1];
            const data = this._dataMap.get(type);
            const visibility = this._visibility.get(type) ?? true;
            layers.push(generator.generateIconLayer(data, visibility, (a, b) => this._handleLayerClick(a, b)))
        }
        this.setProps({
            layers: layers
        });
    }
    private _handleLayerClick(pickingInfo: PickingInfo<MaritimeObjectViewState<MaritimeObjectState>>, event: MjolnirEvent) {
        if(this.onObjectClicked != undefined) {
            this.onObjectClicked({
                ...pickingInfo,
                object: pickingInfo.object?.object
            }, event);
        }
    }

    public setLayerVisibility(objectType: MaritimeObjectType, value: boolean) {
        this._visibility.set(objectType, value);
    }
}