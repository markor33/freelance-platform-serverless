import { Contact } from "../../shared/models/contact.model";

export class Registration {
    username: string = '';
    email: string = '';
    password: string = '';
    confirmPassword: string = '';
    role: Role = Role.FREELANCER;
    firstName: string = '';
    lastName: string = '';
    contact: Contact = new Contact();
}

export enum Role {
    FREELANCER = 0, 
    CLIENT = 1
}