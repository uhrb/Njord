import type { Dimensions } from "@/types/Dimensions";
import type { ETA } from "@/types/ETA";

class FormatterHelperImplementation {
    // Converts a number to a string with a degree symbol (°) and optional decimal places.
    // If the number is undefined or null, it returns 'Undefined'.
    public getDegreeString(degs: number | undefined, notAvail: number, toFixed?: number, undefinedString?: string, notAvailString?: string): string | undefined {
        if (degs === undefined || degs === null) {
            return undefinedString;
        }
        if (degs === notAvail) {
            return notAvailString;
        }

        return `${toFixed == undefined ? degs : degs.toFixed(toFixed)}°`;
    }

    public getMappedName(type: number | undefined, mappings: Record<number, string | undefined>, unmatchedString: string, undefinedString?: string): string | undefined {
        if (type === undefined) {
            return undefinedString;
        }

        return mappings[type] ?? `${unmatchedString} ${type}`;
    }

    public getUndefTrueFalse(value: boolean | undefined, ifUndef: string, ifTrue: string, ifFalse: string,): string {
        if (value === undefined) {
            return ifUndef;
        }
        return value ? ifTrue : ifFalse;
    }

    public getLocalDate(date: string | undefined, undefinedString: string) {
        if (date == undefined) {
            return undefinedString
        }
        return new Date(Date.parse(date)).toLocaleTimeString();
    }

    public getEtaDate(eta: ETA | undefined, undefinedString: string) {
        if (eta == undefined) {
            return undefinedString;
        }

        var date = new Date(Date.now());
        const month = (eta.month ?? 0) == 0 ? date.getMonth() : eta.month! -1;
        const day = (eta.day ?? 0) == 0 ? date.getDate() : eta.day!;
        const hour = (eta.hour ?? 0) == 0 ? date.getHours() : eta.hour!;
        const minutes = (eta.minute ?? 0) == 0 ? date.getMinutes() : eta.minute!;
        date.setMonth(month);
        date.setDate(day);
        date.setHours(hour);
        date.setMinutes(minutes);
        return date.toLocaleString();
    }

    public getLength(dim: Dimensions | undefined, undefinedString: string) {
        if(dim == undefined || dim?.a == undefined || dim?.b == undefined) {
            return undefinedString;
        }

        return dim.a + dim.b;
    }

    public getWidth(dim: Dimensions | undefined, undefinedString: string) {
        if(dim == undefined || dim?.c == undefined || dim?.d == undefined) {
            return undefinedString;
        }

        return dim.d + dim.c;
    }


}

export const FormatterHelper = new FormatterHelperImplementation();