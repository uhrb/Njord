class VesselHelperImplementation {

    public getVesselSvgPath(height: number, width: number, paddingWidth: number, paddingHeigth: number) {
        return `<path fill-rule='evenodd' d='` +
        `M ${paddingWidth + width / 2.0} ${paddingHeigth} L ${paddingWidth + width} ${paddingHeigth + height / 5.0} `+
        `L ${width + paddingWidth} ${height + paddingHeigth} L ${paddingWidth} ${paddingHeigth + height} L ${paddingWidth} ${paddingHeigth + height / 5.0}` +
        `' stroke-width='1'>` +
        `</path>` 
    }

    public getVesselIcon(height: number, width: number) {
        return `<svg width="${width + 2}" height="${height + 2}" xmlns="http://www.w3.org/2000/svg">${this.getVesselSvgPath(height, width, 1, 1)}`+
        `</svg>` 
    }

    getVesselAngle(trueHeading: number | undefined, courseOverGround: number | undefined) {
        let angle = 0;
        if (trueHeading != undefined && trueHeading != 511) {
            angle = trueHeading;
        }
        else {
            if (courseOverGround != undefined && courseOverGround != 360) {
                angle = courseOverGround;
            }
        }
        return 360 - angle;
    }
}

export const VesselHelper: VesselHelperImplementation = new VesselHelperImplementation();