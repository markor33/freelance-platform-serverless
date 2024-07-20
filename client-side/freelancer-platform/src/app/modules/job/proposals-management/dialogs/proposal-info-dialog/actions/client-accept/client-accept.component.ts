import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EditProposalPaymentCommand } from 'src/app/modules/job/models/commands/edit-proposal-payment-command-model';
import { Payment } from 'src/app/modules/job/models/payment.model';
import { Proposal, ProposalStatus } from 'src/app/modules/job/models/proposal.model';
import { ProposalService } from 'src/app/modules/job/services/proposal.service';
import { ConfirmationDialogComponent } from 'src/app/modules/shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';

@Component({
  selector: 'app-client-accept',
  templateUrl: './client-accept.component.html',
  styleUrls: ['./client-accept.component.scss']
})
export class ClientAcceptComponent {

  paymentPanelOpenState: boolean = false;
  @Input() jobId: string = '';
  @Input() proposal: Proposal = new Proposal();

  payment: Payment = new Payment();
  editProposalPaymentCommand = new EditProposalPaymentCommand();

  constructor(
    private proposalService: ProposalService,
    private dialog: MatDialog,
    private snackBarService: SnackBarsService) { }

  ngOnInit() {
    this.payment = {...this.proposal.payment};
    this.editProposalPaymentCommand.jobId = this.jobId;
    this.editProposalPaymentCommand.proposalId = this.proposal.id;
    this.editProposalPaymentCommand.payment = this.payment;
  }

  editPayment() {
    this.proposalService.editPayment(this.editProposalPaymentCommand).subscribe(() => {
      this.snackBarService.primary('Proposals payment successfully changed');
      this.proposal.payment = this.payment;
    });
  }

  accept() {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to approve the proposal.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.proposalService.clientApprove(this.jobId, this.proposal.id).subscribe(() => {
        this.snackBarService.primary('Proposal approved successfully');
        this.proposal.status = ProposalStatus.CLIENT_APPROVED;
      });
  })
  }
  
}
