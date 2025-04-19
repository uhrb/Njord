export function getDegreeString(degs: number | undefined, notAvail: number): string {
    if(degs === undefined || degs === null) {
        return 'Undefined';
    }
    if(degs === notAvail) {
        return 'Not available';
    }

    return `${degs}°`;
}