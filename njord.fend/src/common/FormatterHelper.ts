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

    public getMappedName(type: number | undefined, mappings: Record<number, string | undefined>, unmatchedString : string, undefinedString?: string): string | undefined {
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

}

export const FormatterHelper = new FormatterHelperImplementation();