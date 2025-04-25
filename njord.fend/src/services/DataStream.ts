import type { VesselState } from "@/types/VesselState";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

import { mappingsStore } from "@/stores/mappingsStore";
import { objectsStore } from "@/stores/objectsStore";
import { useIntervalFn } from "@vueuse/core";
import type { SarState } from "@/types/SarState";
import { SarType } from "@/types/SarType";

export class DataStream {

    _connection: HubConnection;
    _vessels: Map<string, VesselState> = new Map();
    _sar: Map<string, SarState> = new Map();

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
                    this._parseSar(SarType.FixedWing, objectId, objectState);
                    break;
                case "SearchAndRescueHelicopter":
                    this._parseSar(SarType.Helicopter, objectId, objectState);
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
            await await this._connection.invoke("CommandGetPositionFixingDeviceTypeMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(mappingsStore.PositionFixingDeviceTypeMappings, result);
            });
            await this._connection.invoke("CommandSendAllStatesByTypes", ["Vessel", "SearchAndRescueFixedWingAircraft", "SearchAndRescueHelicopter"]);
        });

        useIntervalFn(() => {
            objectsStore.vessels = this._vessels.values();
            objectsStore.vesselsCount = this._vessels.size;
            objectsStore.sars = this._sar.values();
            objectsStore.sarsCount = this._sar.size;
        }, 1000)
    }

    private _parseVesselUpdate(objectId: string, objectState: any) {
        var vsl: VesselState = {
            mmsi: objectId,
            ...objectState,
        }
        this._vessels.set(objectId, vsl);
    }

    private _parseSar(objectType: SarType, objectId: string, objectState: any) {
        var sar = {
            mmsi: objectId,
            type: objectType,
            ...objectState
        }
        this._sar.set(objectId, sar);
    }
}