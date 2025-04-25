import type { VesselState } from "@/types/VesselState";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

import { mappingsStore } from "@/stores/mappingsStore";
import { objectsStore } from "@/stores/objectsStore";
import { useIntervalFn } from "@vueuse/core";

export class DataStream {

    _connection: HubConnection;
    _vessels: Map<string, VesselState> = new Map();

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
                    // TODO offload to background?
                    this._parseVesselUpdate(objectId, objectState);
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
            await this._connection.invoke("CommandSendAllStatesByType", "Vessel");
        });

        useIntervalFn(() => {
            objectsStore.vessels = this._vessels.values();
            objectsStore.vesselsCount = this._vessels.size;
        }, 1000)
    }

    private _parseVesselUpdate(objectId: string, objectState: any) {
        var vsl: VesselState = {
            mmsi: objectId,
            ...objectState,
        }
        this._vessels.set(objectId, vsl);
    }
}