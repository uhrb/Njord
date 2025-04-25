import type { Dimensions } from "@/types/Dimensions";
import { LatLngBounds } from "leaflet";

import movingStatus from "@/assets/moving-status.svg";
import nucStatus from "@/assets/nuc-status.svg";
import questionStatus from "@/assets/question-status.svg";
import stoppedStatus from "@/assets/stopped-status.svg";
import type { VesselState } from "@/types/VesselState";

class VesselHelperImplementation {

    public getVesselSvgPath(height: number, width: number, paddingWidth: number, paddingHeigth: number) {
        return `<path fill-rule='evenodd' d='` +
            `M ${paddingWidth + width / 2.0} ${paddingHeigth} L ${paddingWidth + width} ${paddingHeigth + height / 5.0} ` +
            `L ${width + paddingWidth} ${height + paddingHeigth} L ${paddingWidth} ${paddingHeigth + height} L ${paddingWidth} ${paddingHeigth + height / 5.0}` +
            `' stroke-width='1'>` +
            `</path>`
    }

   

    getVesselSize(d: Dimensions | undefined): number {
        let height = (d?.a ?? 0) + (d?.b ?? 0);
        return height > 0 ? height : 15;
    }

    getVesselIcon(vessel: VesselState, bounds: LatLngBounds, zoom: number) {

        let url: string = questionStatus;

        if (zoom >= 14 && bounds.contains({ lat: vessel.latitude!, lng: vessel.longitude! })) {
            if (vessel.dimensions != undefined) {
                const height = (vessel.dimensions.b ?? 0) + (vessel.dimensions.a ?? 0);
                const width = (vessel.dimensions.c ?? 0) + (vessel.dimensions.d ?? 0);


                if (height > 0 && width > 0) {
                    let rad = width;
                    if(height > width) {
                        rad = height;
                    }
                    const svg = `<svg width="${width + 2}" height="${height + 2}" xmlns="http://www.w3.org/2000/svg">
                            ${VesselHelper.getVesselSvgPath(height, width, 1, 1)}
                            </svg>`
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
                url = nucStatus;
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

    getVesselColor(type: number | undefined): [number, number, number, number] {
        switch (type) {
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

export const VesselHelper: VesselHelperImplementation = new VesselHelperImplementation();