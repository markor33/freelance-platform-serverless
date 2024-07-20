import { Injectable } from "@angular/core";
import { PaymentType } from "../../job/models/payment.model";
import { ExperienceLevel } from "../models/experience-level.model";
import { JobStatus } from "../../job/models/job.model";
import { ProposalStatus } from "../../job/models/proposal.model";
import { ContractStatus } from "../../contract/models/contract.model";
import { Availability } from "../../freelancer/models/freelancer.model";
import { LanguageProficiencyLevel } from "../models/language.model";

@Injectable({
    providedIn: 'root'
})
export class EnumConverter {

    availabilityToString(availability: Availability): string {
        switch(availability) {
            case Availability.FULL_TIME:
                return 'Full time';
            case Availability.PART_TIME:
                return 'Part time';
        }
    }

    languageProficiencyLevelToString(languageProficiencyLevel: LanguageProficiencyLevel): string {
        switch(languageProficiencyLevel) {
            case LanguageProficiencyLevel.BASIC:
                return 'Basic';
            case LanguageProficiencyLevel.CONVERSATIONAL:
                return 'Conversational';
            case LanguageProficiencyLevel.FLUENT:
                return 'Fluent';
            case LanguageProficiencyLevel.NATIVE:
                return 'Native';
        }
    }

    paymentTypeToString(paymentType: PaymentType): string {
        switch(paymentType) {
            case PaymentType.FIXED_RATE:
                return 'Fixed';
            case PaymentType.HOURLY_RATE:
                return 'Hourly';
        }
    }

    experienceLevelToString(experienceLevel: ExperienceLevel): string {
        switch(experienceLevel) {
            case ExperienceLevel.JUNIOR:
                return 'Junior';
            case ExperienceLevel.MEDIOR:
                return 'Medior';
            case ExperienceLevel.SENIOR:
                return 'Senior';
        }
    }

    jobStatusToString(jobStatus: JobStatus): string {
        switch(jobStatus) {
            case JobStatus.LISTED:
                return 'Listed';
            case JobStatus.IN_PROGRESS:
                return 'In progress';
            case JobStatus.DONE:
                return 'Done';
            case JobStatus.REMOVED:
                return 'Removed';
        }
    }

    proposalStatusToString(proposalStatus: ProposalStatus): string {
        switch(proposalStatus) {
            case ProposalStatus.SENT:
                return 'Sent';
            case ProposalStatus.INTERVIEW:
                return 'Interview';
            case ProposalStatus.CLIENT_APPROVED:
                return 'Approved';
            case ProposalStatus.FREELANCER_APPROVED:
                return 'Contract';
        }
    }

    contractStatusToString(contractStatus: ContractStatus): string {
        switch(contractStatus) {
            case ContractStatus.ACTIVE:
                return 'Active';
            case ContractStatus.FINISHED:
                return 'Finished';
            case ContractStatus.TERMINATED:
                return 'Terminated';
        }
    }
}