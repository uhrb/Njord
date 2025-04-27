import movingStatus from "@/assets/moving-status.svg";
import nucStatus from "@/assets/nuc-status.svg";
import questionStatus from "@/assets/question-status.svg";
import stoppedStatus from "@/assets/stopped-status.svg";
import type { VesselState } from "@/types/VesselState";
import type { LatLngBounds } from "leaflet";


export interface CombineOptions {
    finalWidth: any;
    finalHeight: any;
    scale1?: number;
    scale2?: number;
    rotate1?: number;
    translate1X?: number;
    translate1Y?: number;
    rotate2?: number;
    translate2X?: number;
    translate2Y?: number;
}

class SvgHelperImplementation {

    public getVesselSvgPath(height: number, width: number, paddingWidth: number, paddingHeigth: number) {
        return `<path fill-rule='evenodd' d='` +
            `M ${paddingWidth + width / 2.0} ${paddingHeigth} L ${paddingWidth + width} ${paddingHeigth + height / 5.0} ` +
            `L ${width + paddingWidth} ${height + paddingHeigth} L ${paddingWidth} ${paddingHeigth + height} L ${paddingWidth} ${paddingHeigth + height / 5.0}` +
            `' stroke-width='1'>` +
            `</path>`
    }

    public getVesselIcon(vessel: VesselState, bounds: LatLngBounds, zoom: number) {

        let url: string = questionStatus;

        if (zoom >= 14 && bounds.contains({ lat: vessel.latitude!, lng: vessel.longitude! })) {
            if (vessel.dimensions != undefined) {
                const height = (vessel.dimensions.b ?? 0) + (vessel.dimensions.a ?? 0);
                const width = (vessel.dimensions.c ?? 0) + (vessel.dimensions.d ?? 0);


                if (height > 0 && width > 0) {
                    const svg = `<svg width="${width + 2}" height="${height + 2}" xmlns="http://www.w3.org/2000/svg">
                            ${this.getVesselSvgPath(height, width, 1, 1)}
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

    public extractSvgRaw(raw: string) : [ string, number, number ] {
        const parser = new DOMParser();
        const svgDoc = parser.parseFromString(raw, 'image/svg+xml');
        const svg = svgDoc.documentElement;
        let width = parseFloat(svg.getAttribute("width")!);
        let height = parseFloat(svg.getAttribute("height")!);
        const serializer = new XMLSerializer();
        const svgString = serializer.serializeToString(svg);
        return [svgString, width, height];
    }

    public combineAndTransformSVGs(firstRaw: string, secondRaw: string, options: CombineOptions) {
        const parser = new DOMParser();
        const svg1Doc = parser.parseFromString(firstRaw, 'image/svg+xml');
        const svg2Doc = parser.parseFromString(secondRaw, 'image/svg+xml');
        const svg1 = svg1Doc.documentElement;
        const svg2 = svg2Doc.documentElement;
        const combinedSVG = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        combinedSVG.setAttribute('xmlns', 'http://www.w3.org/2000/svg');
        combinedSVG.setAttribute('width', options.finalWidth.toString());
        combinedSVG.setAttribute('height', options.finalHeight.toString());

        // Wrap and transform each SVG
        const g1 = this._wrapInGroup(svg1, options.scale1, options.rotate1, options.translate1X, options.translate1Y);
        const g2 = this._wrapInGroup(svg2, options.scale2, options.rotate2, options.translate2X, options.translate2Y);

        // Append groups to the new SVG
        combinedSVG.appendChild(g1);
        combinedSVG.appendChild(g2);

        // Serialize the SVG to a string
        const serializer = new XMLSerializer();
        const svgString = serializer.serializeToString(combinedSVG);

        return svgString;
    }

    private _wrapInGroup(svgElement: HTMLElement, scale?: number, rotate?: number, translateX?: number, translateY?: number) {
        const g = document.createElementNS('http://www.w3.org/2000/svg', 'g');

        // Move contents of svgElement into <g>
        while (svgElement.childNodes.length > 0) {
            g.appendChild(svgElement.childNodes[0]);
        }

        let rotPointX = parseFloat(svgElement.getAttribute("width") ?? '0') / 2.0;
        let rotPointY = parseFloat(svgElement.getAttribute("height") ?? '0') / 2.0;

        if (scale != undefined) {
            rotPointX = rotPointX * scale;
            rotPointY = rotPointY * scale;
        }

        // Set transformation
        g.setAttribute('transform', `
            ${translateX != undefined && translateY != undefined ? `translate(${translateX}, ${translateY})` : ''}
            ${rotate != undefined ? `rotate(${rotate} ${rotPointX} ${rotPointY})` : ''}
            ${scale != undefined ? `scale(${scale})` : ''}
        `);
        return g;
    }


}

export const SvgHelper = new SvgHelperImplementation();