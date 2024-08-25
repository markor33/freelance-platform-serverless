import { ExperienceLevel } from "src/app/modules/shared/models/experience-level.model";
import { LanguageProficiencyLevel } from "src/app/modules/shared/models/language.model";
import { ProfileSummary, HourlyRate, Availability } from "../freelancer.model";
import {Contact} from "../../../shared/models/contact.model";

export class ProfileSetupCommand {
    profileSummary: ProfileSummary = new ProfileSummary();
    hourlyRate: HourlyRate = new HourlyRate();
    isProfilePublic: boolean = false;
    availability: Availability = Availability.FULL_TIME;
    professionId: string = '';
    experienceLevel: ExperienceLevel = ExperienceLevel.JUNIOR;
    languageId : number = 0;
    languageProficiencyLevel: LanguageProficiencyLevel = LanguageProficiencyLevel.CONVERSATIONAL;
}
