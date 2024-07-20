import { Employment } from "../freelancer.model";

export class EditEmploymentCommand {
    employmentId: string;
    title: string;
    company: string;
    start: Date;
    end: Date;
    description: string;

    constructor(employment: Employment) {
        this.employmentId = employment.id;
        this.title = employment.title;
        this.company = employment.company;
        this.start = employment.period.start;
        this.end = employment.period.end;
        this.description = employment.description;
    }
}