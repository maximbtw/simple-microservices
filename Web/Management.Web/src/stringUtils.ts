export class StringUtils {
    static isNullOrEmpty(str: string | null | undefined): boolean {
        return !str || str.trim().length === 0;
    }

    static capitalize(str: string): string {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }
}