import { Address } from "./address.model";

export class Contact {
    phoneNumber: string = '';
    timeZoneId: string = 'Central Europe Standard Time';
    address: Address = new Address();
}