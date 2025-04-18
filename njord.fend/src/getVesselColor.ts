import { VesselState } from './VesselState';

export function getVesselColor(vessel: VesselState): [number, number, number, number] {
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
            return [39, 245, 245, 255];
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
