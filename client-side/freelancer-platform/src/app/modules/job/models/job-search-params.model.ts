import { ExperienceLevel } from "../../shared/models/experience-level.model";
import { PaymentType } from "./payment.model";

export class JobSearchParams {
    fullTextSearch: string = '';
    professions: string[] = [];
    experienceLevels: ExperienceLevel[] = [];
    paymentTypes: PaymentType[] = [];
}
