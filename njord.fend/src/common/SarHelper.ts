import sarFixedIcon from "@/assets/aircraft.svg";
import sarHelicopter from "@/assets/helicopter.svg";
import { SarType } from "@/types/SarType";

class SarHelperImplementation {
    public getColor() : [number, number, number, number] {
        return [255, 49, 37, 255];
    }

    public getIcon(sarType: SarType) {
        let url = sarFixedIcon;
        switch(sarType) {
            case SarType.Helicopter:
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

export const SarHelper: SarHelperImplementation = new SarHelperImplementation();