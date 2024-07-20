import { ExperienceLevel } from "../../shared/models/experience-level.model";
import { Profession, Skill } from "../../shared/models/profession.mode";
import { Payment } from "./payment.model";
import { Question } from "./question.model";

export class Job {
    id: string = '';
    title: string = '';
    description: string = '';
    experienceLevel: ExperienceLevel = ExperienceLevel.JUNIOR;
    payment: Payment = new Payment();
    credits: number = 0;
    questions: Question[] = [];
    profession: Profession = new Profession();
    skills: Skill[] = [];
    numOfProposals: number = 0;
    currentlyInterviewing: number = 0;
    numOfActiveContracts: number = 0;
    NumOfFinishedContracts: number = 0;
    status: JobStatus = JobStatus.LISTED;
}

export enum JobStatus {
    LISTED,
    IN_PROGRESS,
    DONE,
    REMOVED
}