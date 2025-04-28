import type { VesselState } from "@/types/VesselState";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

import { mappingsStore } from "@/stores/mappingsStore";
import { objectsStore } from "@/stores/objectsStore";
import { useIntervalFn } from "@vueuse/core";
import type { SarAircraftState } from "@/types/SarAircraftState";
import { SarAircraftType } from "@/types/SarAircraftType";
import { MaritimeObjectType } from "@/types/MaritimeObjectType";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";
import { StationType } from "@/types/StationType";
import type { StationState } from "@/types/BaseStationState";
import { AtoNType } from "@/types/AtoNType";
import type { AtoNState } from "@/types/AtoNState";
import { MaritimeDeviceType } from "@/types/MaritimeDeviceType";
import { SARTType } from "@/types/SartType";
import type { SARTState } from "@/types/SartState";

export class DataStream {

    _connection: HubConnection;
    _dataMap: Map<MaritimeObjectType, Map<string, MaritimeObjectState>> = new Map([
        [MaritimeObjectType.Vessel, new Map()],
        [MaritimeObjectType.SarAircraft, new Map()],
        [MaritimeObjectType.Station, new Map()],
        [MaritimeObjectType.AtoN, new Map()],
        [MaritimeObjectType.Device, new Map()],
        [MaritimeObjectType.SART, new Map()]
    ]);

    constructor() {
        this._connection = new HubConnectionBuilder()
            .withUrl('https://localhost:7077/map-updates', {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();
    }

    public setup() {
        this._connection.on("Update", (objectType, objectId, objectState) => {
            switch (objectType) {
                case "Vessel":
                    this._parseVesselUpdate(objectId, objectState);
                    break;
                case "SearchAndRescueFixedWingAircraft":
                    this._parseSar(SarAircraftType.FixedWing, objectId, objectState);
                    break;
                case "SearchAndRescueHelicopter":
                    this._parseSar(SarAircraftType.Helicopter, objectId, objectState);
                    break;
                case "BaseStation":
                    this._parseStation(StationType.BaseStation, objectId, objectState);
                    break;
                case "CoastStation":
                    this._parseStation(StationType.CoastStation, objectId, objectState);
                    break;
                case "PilotStation":
                    this._parseStation(StationType.PilotStation, objectId, objectState);
                    break;
                case "PortStation":
                    this._parseStation(StationType.PortStation, objectId, objectState);
                    break;
                case "RepeaterStation":
                    this._parseStation(StationType.RepeaterStation, objectId, objectState);
                    break;
                case "MobileAidsToNavigation":
                    this._parseAtoN(AtoNType.Mobile, objectId, objectState);
                    break;
                case "PhysicalAidsToNavigation":
                    this._parseAtoN(AtoNType.Physical, objectId, objectState);
                    break;
                case "VirtualAidsToNavigation":
                    this._parseAtoN(AtoNType.Virtual, objectId, objectState);
                case "EmergencyPositionIdentificationSystem":
                    this._parseMaritimeDevice(MaritimeDeviceType.EmergencyPositionIdentificationSystem, objectId, objectState);
                    break;
                case "HandheldVHF":
                    this._parseMaritimeDevice(MaritimeDeviceType.HandheldVHF, objectId, objectState);
                    break;
                case "ManOverBoard":
                    this._parseSart(SARTType.ManOverBoard, objectId, objectState);
                    break;
                case "SearchAndRescueTransmitter":
                    this._parseSart(SARTType.SearchAndRescueTransmitter, objectId, objectState);
                    break;
                default:
                    break;
            }
        });
        this._connection.start().then(async () => {
            await this._connection.invoke("GetEnumNamesMappings", "TypeOfShipAndCargoType").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.ShipTypeNameMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "NavigationalStatus").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.NavigationStatusMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "SpecialManoeuvreIndicator").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.SpecialManoeuvreIndicatorMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "PositionFixingDeviceType").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.PositionFixingDeviceTypeMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "AidsToNavigationType").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.AidsToNavigationTypeMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "AidsToNavigationLightsStatus").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.AidsToNavigationLightsStatusMappings, result);
            });
            await this._connection.invoke("GetEnumNamesMappings", "AidsToNavigationRACONStatus").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.AidsToNavigationRACONStatusMappings, result);
            });
            await this._connection.invoke("CommandSendAllStatesByTypes", [
                "Vessel",
                "SearchAndRescueFixedWingAircraft",
                "SearchAndRescueHelicopter",
                "BaseStation",
                "CoastStation",
                "PilotStation",
                "PortStation",
                "RepeaterStation",
                "MobileAidsToNavigation",
                "PhysicalAidsToNavigation",
                "VirtualAidsToNavigation",
                "EmergencyPositionIdentificationSystem",
                "HandheldVHF",
                "ManOverBoard",
                "SearchAndRescueTransmitter"
            ]);
        });

        useIntervalFn(() => {
            for (const kv of this._dataMap) {
                const key = kv[0];
                const val = kv[1];
                objectsStore.objects.set(key, val.values());
                objectsStore.objectsAmount.set(key, val.size);
            }
        }, 1000)
    }
    
    private _parseSart(sartType: SARTType, objectId: string, objectState: any) {
        let sart: SARTState = {
            sartType: sartType,
            mmsi: objectId,
            objectType: MaritimeObjectType.SART,
            ...objectState
        }

        this._dataMap.get(MaritimeObjectType.SART)?.set(objectId, sart);
    }

    private _parseMaritimeDevice(deviceType: MaritimeDeviceType, objectId: string, objectState: any) {
        let dev: MaritimeObjectState = {
            mmsi: objectId,
            deviceType: deviceType,
            objectType: MaritimeObjectType.Device,
            ...objectState
        };

        this._dataMap.get(MaritimeObjectType.Device)?.set(objectId, dev);
    }

    private _parseVesselUpdate(objectId: string, objectState: any) {
        let vsl: VesselState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.Vessel,
            ...objectState,
        }
        this._dataMap.get(MaritimeObjectType.Vessel)?.set(objectId, vsl);
    }

    private _parseSar(objectType: SarAircraftType, objectId: string, objectState: any) {
        let sar: SarAircraftState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.SarAircraft,
            aircraftType: objectType,
            ...objectState
        }
        this._dataMap.get(MaritimeObjectType.SarAircraft)?.set(objectId, sar);
    }

    private _parseStation(baseStationType: StationType, objectId: string, objectState: any) {
        let station: StationState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.Station,
            baseStationType: baseStationType,
            ...objectState
        }
        this._dataMap.get(MaritimeObjectType.Station)?.set(objectId, station);
    }

    private _parseAtoN(atonType: AtoNType, objectId: string, objectState: any) {
        let aton: AtoNState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.AtoN,
            atonType: atonType,
            ...objectState
        }
        this._dataMap.get(MaritimeObjectType.AtoN)?.set(objectId, aton);
    }
}