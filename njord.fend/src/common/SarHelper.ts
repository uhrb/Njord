import sarFixedIcon from "@/assets/aircraft.svg";
import sarHelicopter from "@/assets/helicopter.svg";
import { SarAircraftType } from "@/types/SarAircraftType";

class SarAircraftHelperImplementation {
    public getColor() : [number, number, number, number] {
        return [255, 49, 37, 255];
    }

    public getIcon(sarType: SarAircraftType) {
        let url = sarFixedIcon;
        switch(sarType) {
            case SarAircraftType.Helicopter:
                url = sarHelicopter;
                break;
            default:
                break;
        }
        return {
            url: url,
            width: 128,
            height: 128,
            anchorY: 64,
            anchorX: 64,
            mask: true,
        };;
    }
}

export const SarAircraftHelper: SarAircraftHelperImplementation = new SarAircraftHelperImplementation();