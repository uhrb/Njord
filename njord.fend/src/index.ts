import * as L from 'leaflet';
import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr"
import { MapView, PickingInfo } from '@deck.gl/core';
import { VesselState } from './VesselState';
import 'leaflet/dist/leaflet.css';
import './styles.css';
import { InfoControl } from './InfoControl';
import { MapStatistics } from './MapStatistics';
import { VesselsLayer } from './VesselsLayer';
import { FormatterHelper } from './FormatterHelper';

const vessels: Map<string, VesselState> = new Map<string, VesselState>();

let shipTypeNameMappings: Record<number, string | undefined> = {};
let navigationStatusMappings: Record<number, string | undefined> = {};

const map = L.map(document.getElementById('map')!, {
    center: [36.69, -4.41],
    zoom: 15
});

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 16,
    minZoom: 3,
}).addTo(map);

L.tileLayer('https://t1.openseamap.org/seamark/{z}/{x}/{y}.png', {
    maxZoom: 16,
    minZoom: 3,
}).addTo(map);

L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco2021_contour',
    transparent: true,
    format: 'image/png',
    opacity: 0.4,
    maxZoom: 16,
    minZoom: 3,
    attribution: '© OpenSeaMap contributors'
}).addTo(map);

L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco_2021_poly',
    transparent: true,
    format: 'image/png',
    opacity: 0.3,
    maxZoom: 16,
    minZoom: 3,
}).addTo(map);



var vesselsLayer = new VesselsLayer(shipTypeNameMappings, navigationStatusMappings);

vesselsLayer.onVesselClicked = (info: PickingInfo<VesselState>, event: any) => {
    // https://www.vesselfinder.com/vessels/details/555666888
    // window.open(`https://www.vesselfinder.com/vessels/details/${info.object!.mmsi}`, '_blank');
    vesselInfo.update(info.object);
}

var info = new InfoControl<MapStatistics>((stats?: MapStatistics) => {
    return `<table>` +
        `<tr><td>Vessels</td><td>${stats?.VesselCount}</td></tr>` +
        `</table>`;
}, { position: 'topright' });
var vesselInfo = new InfoControl<VesselState>((vsl?: VesselState) => {
    if (vsl == undefined) {
        return '';
    }
    return `<table>` +
        `<thead><tr><td colspan="2">Vessel</td></tr></thead>` +
        `<tbody>` +
        `<tr><td>MMSI</td><td><a href='https://www.vesselfinder.com/vessels/details/${vsl.mmsi}' target='_blank'>${vsl.mmsi}</a></td></tr>` +
        `<tr><td>Name</td><td>${vsl.name}</td></tr>` +
        `<tr><td>Call Sign</td><td>${vsl.callSign}</td></tr>` +
        `<tr><td>IMO Number</td><td>${vsl.imoNumber}</td></tr>` +
        `<tr><td>Vessel Type</td><td>${FormatterHelper.getMappedName(vsl.typeOfShipAndCargoType, shipTypeNameMappings, "Unknown")}</td></tr>` +
        `<tr><td>Draught</td><td>${vsl.maximumPresentStaticDraught}</td></tr>` +
        `<tr><td>Dimensions</td><td>A:${vsl.dimensions?.a ?? 'N/A'} B:${vsl.dimensions?.b ?? 'N/A'} C:${vsl.dimensions?.c ?? 'N/A'} D:${vsl.dimensions?.d ?? 'N/A'}</td></tr>` +
        `<tr><td>Updated</td><td>${new Date(Date.parse(vsl.updated!)).toLocaleTimeString()}</td></tr>` +
        `</tbody>` +
        `<thead><tr><td colspan="2">Position</td></tr></thead>` +
        `<tbody>` +
        `<tr><td>Fixing device</td><td>${vsl.fixingDeviceType}</td></tr>` +
        `<tr><td>Position Accuracy</td><td>${FormatterHelper.getUndefTrueFalse(vsl.isPositionAccuracyHigh, 'Unknown', 'High', 'Low')}</td></tr>` +
        `<tr><td>RAIM</td><td>${FormatterHelper.getUndefTrueFalse(vsl.isRaimInUse, 'Unknown', 'Yes', 'No')}</td></tr>` +
        `<tr><td>Longitude</td><td>${FormatterHelper.getDegreeString(vsl.longitude, 181, 5)}</td></tr>` +
        `<tr><td>Latitude</td><td>${FormatterHelper.getDegreeString(vsl.latitude, 91, 5)}</td></tr>` +
        `</tbody>` +
        `<thead><tr><td colspan="2">Navigation</td></tr></thead>` +
        `<tbody>` +
        `<tr><td>Status</td><td>${FormatterHelper.getMappedName(vsl.navigationalStatus, navigationStatusMappings, "Unknown")}</td></tr>` +
        `<tr><td>Special manoeuvre</td><td>Not implemented</td></tr>` +
        `<tr><td>COG</td><td>${FormatterHelper.getDegreeString(vsl.courseOverGround, 360, 2)}</td></tr>` +
        `<tr><td>True Heading</td><td>${FormatterHelper.getDegreeString(vsl.trueHeading, 511, 2)}</td></tr>` +
        `<tr><td>SOG</td><td>${vsl.speedOverGround?.toFixed(2) + ' kn'}</td></tr>` +
        `<tr><td>Rate of turn</td><td>${vsl.rateOfTurn}</td></tr>` +
        `</tbody>` +
        `<thead><tr><td colspan="2">Voyage</td></tr></thead>` +
        `<tbody>` +
        `<tr><td>Destination</td><td>${vsl.destination}</td></tr>` +
        `<tr><td>ETA</td><td>Not implemented</td></tr>` +
        `</tbody>` +
        `</table>`;
}, { position: 'bottomright' });



map.addControl(info);
map.addControl(vesselInfo);
map.addLayer(vesselsLayer);


function parseVesselUpdate(objectId: string, objectState: any) {
    var vsl: VesselState = {
        mmsi: objectId,
        ...objectState,
    }
    vessels.set(objectId, vsl);
}

const connection = new HubConnectionBuilder()
    .withUrl('https://localhost:7077/map-updates', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

connection.on("Update", (objectType, objectId, objectState) => {
    switch (objectType) {
        case "Vessel":
            parseVesselUpdate(objectId, objectState);
            break;
        default:
            break;
    }
    info.update({ VesselCount: vessels.size });
});


connection.start().then(async () => {
    await connection.invoke("CommandGetShipTypeMappings").then((result: Record<number, string | undefined>) => {
        Object.assign(shipTypeNameMappings, result);
    });
    await connection.invoke("CommandGetNavigationStatusMappings").then((result: Record<number, string | undefined>) => {
        Object.assign(navigationStatusMappings, result);
    });
    await connection.invoke("CommandSendAllStatesByType", "Vessel");
});


setInterval(() => {
    const newData = Array.from(vessels.values());
    vesselsLayer.updateLayerData(newData);
    //updateVesselsLayer(deckLayer, vessels, map.getZoom(), vesselClicked);
}, 1000);