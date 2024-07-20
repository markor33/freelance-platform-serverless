import { Payment } from "../../job/models/payment.model";

export class Contract {
    id: string = '';
    jobId: string = '';
    jobTitle: string = '';
    clientId: string = '';
    freelancerId: string = '';
    freelancerName: string = '';
    payment: Payment = new Payment();
    started: Date = new Date();
    finished: Date = new Date();
    status: ContractStatus = ContractStatus.ACTIVE;
}

export enum ContractStatus {
    ACTIVE,
    FINISHED,
    TERMINATED
}