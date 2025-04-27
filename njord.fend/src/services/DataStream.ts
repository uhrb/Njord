import type { VesselState } from "@/types/VesselState";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

import { mappingsStore } from "@/stores/mappingsStore";
import { objectsStore } from "@/stores/objectsStore";
import { useIntervalFn } from "@vueuse/core";
import type { SarAircraftState } from "@/types/SarAircraftState";
import { SarAircraftType } from "@/types/SarAircraftType";
import { MaritimeObjectType } from "@/types/MaritimeObjectType";
import type { MaritimeObjectState } from "@/types/MaritimeObjectState";

export class DataStream {

    _connection: HubConnection;
    _dataMap : Map<MaritimeObjectType, Map<string, MaritimeObjectState>> = new Map([
        [MaritimeObjectType.Vessel, new Map()],
        [MaritimeObjectType.SarAircraft, new Map()]
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
                default:
                    break;
            }
        });
        this._connection.start().then(async () => {
            await this._connection.invoke("CommandGetShipTypeMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.ShipTypeNameMappings, result);
            });
            await this._connection.invoke("CommandGetNavigationStatusMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.NavigationStatusMappings, result);
            });
            await this._connection.invoke("CommandGetSpecialManoeuvreIndicatorMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.SpecialManoeuvreIndicatorMappings, result);
            });
            await this._connection.invoke("CommandGetPositionFixingDeviceTypeMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.PositionFixingDeviceTypeMappings, result);
            });
            await this._connection.invoke("CommandSendAllStatesByTypes", ["Vessel", "SearchAndRescueFixedWingAircraft", "SearchAndRescueHelicopter"]);
        });

        useIntervalFn(() => {
            for(const kv of this._dataMap) {
                const key = kv[0];
                const val = kv[1];
                objectsStore.objects.set(key, val.values());
                objectsStore.objectsAmount.set(key, val.size);
            }
        }, 1000)
    }

    private _parseVesselUpdate(objectId: string, objectState: any) {
        var vsl: VesselState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.Vessel,
            ...objectState,
        }
        this._dataMap.get(MaritimeObjectType.Vessel)?.set(objectId, vsl);
    }

    private _parseSar(objectType: SarAircraftType, objectId: string, objectState: any) {
        var sar : SarAircraftState = {
            mmsi: objectId,
            objectType: MaritimeObjectType.SarAircraft,
            aircraftType: objectType,
            ...objectState
        }
        this._dataMap.get(MaritimeObjectType.SarAircraft)?.set(objectId, sar);
    }
}