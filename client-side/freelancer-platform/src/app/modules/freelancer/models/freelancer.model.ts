import { Contact } from "../../shared/models/contact.model";
import { DateRange } from "../../shared/models/date-range.model";
import { ExperienceLevel } from "../../shared/models/experience-level.model";
import { LanguageKnowledge } from "../../shared/models/language.model";
import { Profession, Skill } from "../../shared/models/profession.mode";
import { EditCertificationCommand } from "./commands/edit-certification-command.model";
import { EditEducationCommand } from "./commands/edit-education-command.model";
import { EditEmploymentCommand } from "./commands/edit-employment-command.model";

export class Freelancer {
    id: string = '';
    firstName: string = '';
    lastName: string = '';
    contact: Contact = new Contact();
    profileSummary: ProfileSummary = new ProfileSummary();
    hourlyRate: HourlyRate = new HourlyRate();
    isPublic: boolean = false;
    joined: Date = new Date();
    availability: Availability = Availability.FULL_TIME;
    profilePictureUrl: string = '';
    languageKnowledges: LanguageKnowledge[] = new Array();
    profession: Profession = new Profession();
    skills: Skill[] = new Array();
    experienceLevel: ExperienceLevel = ExperienceLevel.JUNIOR;
    educations: Education[] = new Array();
    certifications: Certification[] = new Array();
    employments: Employment[] = new Array();
}

export class ProfileSummary {
    title: string = '';
    description: string = '';
}

export class HourlyRate {
    amount: number = 0.0;
    currency: string = 'EUR';
}

export enum Availability {
    FULL_TIME,
    PART_TIME
}

export class Education {
    id: string = '';
    schoolName: string = '';
    degree: string = '';
    attended: DateRange = new DateRange();

    update(data: EditEducationCommand) {
        this.schoolName = data.schoolName;
        this.degree = data.degree;
        this.attended.start = data.start;
        this.attended.end = data.end;
    }
}

export class Certification {
    id: string = '';
    name: string = '';
    provider: string = '';
    attended: DateRange = new DateRange();
    description: string = '';

    update(data: EditCertificationCommand) {
        this.name = data.name;
        this.provider = data.provider;
        this.attended.start = data.start;
        this.attended.end = data.end;
        this.description = data.description;
    }
}

export class Employment {
    id: string = '';
    title: string = '';
    company: string = '';
    period: DateRange = new DateRange();
    description: string = '';

    update(data: EditEmploymentCommand) {
        this.title = data.title;
        this.company = data.company;
        this.period.start = data.start;
        this.period.end = data.end;
        this.description = data.description;
    }
}
