import { ExperienceLevel } from "../../shared/models/experience-level.model";
import { Payment } from "./payment.model";

export class SearchJob {
    id: string = '';
    title: string = '';
    description: string = '';
    created: Date = new Date();
    credits: number = 0;
    payment: Payment = new Payment();
    experienceLevel: ExperienceLevel = ExperienceLevel.JUNIOR;
    numOfProposals: number = 0;
    numOfCurrInterviews: number = 0;
    clientAverageRating: number = 0;
}
