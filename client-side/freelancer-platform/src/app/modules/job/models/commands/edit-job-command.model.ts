import { ExperienceLevel } from "src/app/modules/shared/models/experience-level.model";
import { Payment } from "../payment.model";
import { Question } from "../question.model";
import { Job } from "../job.model";

export class EditJobCommand {
    jobId: string = '';
    title: string = '';
    description: string = '';
    professionId: string = '';
    skills: string[] = [];
    experienceLevel: ExperienceLevel = ExperienceLevel.JUNIOR;
    payment: Payment = new Payment();
    questions: Question[] = new Array<Question>();

    constructor(job: Job) {
        this.jobId = job.id;
        this.title = job.title;
        this.description = job.description;
        this.professionId = job.profession.id;
        this.skills = job.skills?.map((skill) => skill?.id);
        this.experienceLevel = job.experienceLevel;
        this.payment = job.payment;
        this.questions = job.questions;
    }
}