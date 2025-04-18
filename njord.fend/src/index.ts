import * as L from 'leaflet';
import { DeckLayer } from './community/index';
import { HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr"
import { MapView, PickingInfo } from '@deck.gl/core';
import { VesselState } from './VesselState';
import { createVesselsLayer } from './createVesselsLayer';
import { updateVesselsLayer } from './updateVesselsLayer';
import 'leaflet/dist/leaflet.css';

const vessels: Map<string, VesselState> = new Map<string, VesselState>();

const map = L.map(document.getElementById('map')!, {
    center: [51.47, 0.45],
    zoom: 4
});
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 17,
    minZoom: 3,
}).addTo(map);

L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco2021_contour',
    transparent: true,
    format: 'image/png',
    opacity: 0.4,
    maxZoom: 17,
    minZoom: 3,
    attribution: '© OpenSeaMap contributors'
}).addTo(map);

L.tileLayer.wms('https://geoserver.openseamap.org/geoserver/gwc/service/wms?', {
    layers: 'gebco2021:gebco_2021_poly',
    transparent: true,
    format: 'image/png',
    opacity: 0.3,
    maxZoom: 17,
    minZoom: 3,
}).addTo(map);


const deckLayer = new DeckLayer({
    views:[
        new MapView({
            repeat: true,
        }),
    ],
    getTooltip: ({ object }: PickingInfo<VesselState>) => {
        if (!object) {
            return null;
        }
        return object && {
            html: '<table>' +
                `<tr><td>MMSI</td><td>${object.mmsi}</td></tr>` +
                `<tr><td>Name</td><td>${object.name}</td></tr>` +
                `<tr><td>Call Sign</td><td>${object.callSign}</td></tr>` +
                `<tr><td>Type</td><td>${object.typeOfShipAndCargoType}</td></tr>` +
                `<tr><td>Nav status</td><td>${object.navigationalStatus}</td></tr>` +
                `<tr><td>COG</td><td>${object.courseOverGround}</td></tr>` +
                `<tr><td>True Heading</td><td>${object.trueHeading}</td></tr>` +
                `<tr><td>Updated</td><td>${new Date(Date.parse(object.updated!)).toLocaleTimeString()}</td></tr>` +
                '</table>'
        }
    },
    layers: [
        createVesselsLayer([], map.getZoom()),
    ]
});


map.addLayer(deckLayer);

map.on('zoom', () => {
    updateVesselsLayer(deckLayer, vessels, map.getZoom());
});

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
});

connection.start().then(() => {
    connection.invoke("CommandSendAllStatesByType", "Vessel");
});


setInterval(() => {
    updateVesselsLayer(deckLayer, vessels, map.getZoom());
}, 1000);