
export default function getMappedName(type: number | undefined, mappings: Record<number, string | undefined>): string {
    if (type === undefined) {
        return 'N/A';
    }
    
    return mappings[type] ?? `Unmapped type ${type}`;
}