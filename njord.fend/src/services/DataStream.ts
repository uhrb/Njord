import type { VesselState } from "@/types/VesselState";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";


export class DataStream {

    _connection: HubConnection;
    public Vessels: Map<string, VesselState> = new Map();
    public ShipTypeNameMappings: Record<number, string | undefined> = {};
    public NavigationStatusMappings: Record<number, string | undefined> = {};


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
                default:
                    break;
            }
        });
        this._connection.start().then(async () => {
            await this._connection.invoke("CommandGetShipTypeMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(this.ShipTypeNameMappings, result);
            });
            await this._connection.invoke("CommandGetNavigationStatusMappings").then((result: Record<number, string | undefined>) => {
                Object.assign(this.NavigationStatusMappings, result);
            });
            await this._connection.invoke("CommandSendAllStatesByType", "Vessel");
        });
    }

    private _parseVesselUpdate(objectId: string, objectState: any) {
        var vsl: VesselState = {
            mmsi: objectId,
            ...objectState,
        }
        this.Vessels.set(objectId, vsl);
    }
}