import { Payment } from "../payment.model";

export class EditProposalPaymentCommand {
    jobId: string = '';
    proposalId: string = '';
    payment: Payment = new Payment();
}