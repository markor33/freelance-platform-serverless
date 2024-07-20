import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContractService } from 'src/app/modules/contract/services/contract.service';
import { Proposal, ProposalStatus } from 'src/app/modules/job/models/proposal.model';
import { ConfirmationDialogComponent } from 'src/app/modules/shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';

@Component({
  selector: 'app-freelancer-accept',
  templateUrl: './freelancer-accept.component.html',
  styleUrls: ['./freelancer-accept.component.scss']
})
export class FreelancerAcceptComponent {

  @Input() jobId: string = '';
  @Input() proposal: Proposal = new Proposal();

  constructor(
    private contractService: ContractService,
    private dialog: MatDialog,
    private snackBarService: SnackBarsService) { }

  accept() {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to accept the job and make a contract with client.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.contractService.create(this.jobId, this.proposal.id).subscribe(() => {
        this.snackBarService.primary('Job accepted successfully');
        this.proposal.status = ProposalStatus.FREELANCER_APPROVED;
      });
    })
  }

}
