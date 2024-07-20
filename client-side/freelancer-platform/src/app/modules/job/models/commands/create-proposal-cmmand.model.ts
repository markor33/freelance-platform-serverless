import { Answer } from "../answer.model";
import { Payment } from "../payment.model";


export class CreateProposalCommand {
    freelancerId: string = '';
    jobId: string = '';
    text: string = '';
    payment: Payment = new Payment();
    answers: Answer[] = [];
}