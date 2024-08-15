import { Education } from "../freelancer.model";

export class EditEducationCommand {
    educationId: string;
    schoolName: string;
    degree: string;
    start : Date;
    end: Date;

    constructor(education: Education) {
        this.educationId = education.id;
        this.schoolName = education.schoolName;
        this.degree = education.degree;
        this.start = education.attended.start;
        this.end = education.attended.end;
    }
}