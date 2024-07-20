export function convertToUTCDate(date: Date) {
    const timezoneOffset = date.getTimezoneOffset();
    const utcDate = new Date(date.getTime() - timezoneOffset * 60 * 1000);
    return utcDate;
}