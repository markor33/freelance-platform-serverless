import { Certification } from "../freelancer.model";

export class EditCertificationCommand {
    certificationId: string;
    name: string;
    provider: string;
    description: string;
    start : Date;
    end: Date;

    constructor(certification: Certification) {
        this.certificationId = certification.id;
        this.name = certification.name;
        this.provider = certification.provider;
        this.description = certification.description;
        this.start = certification.attended.start;
        this.end = certification.attended.end;
    }
}