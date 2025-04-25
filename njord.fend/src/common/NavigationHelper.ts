class NavigationHelperImplementation {
    getAngle(trueHeading: number | undefined, courseOverGround: number | undefined) {
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

export const NavigationHelper: NavigationHelperImplementation = new NavigationHelperImplementation();