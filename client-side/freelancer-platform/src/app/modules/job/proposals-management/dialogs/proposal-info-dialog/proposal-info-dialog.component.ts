import { Component, Inject } from '@angular/core';
import { Proposal } from '../../../models/proposal.model';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EnumConverter } from 'src/app/modules/shared/utils/enum-string-converter.util';
import { ProposalService } from '../../../services/proposal.service';
import { AuthService } from 'src/app/modules/auth/services/auth.service';

@Component({
  selector: 'app-proposal-info-dialog',
  templateUrl: './proposal-info-dialog.component.html',
  styleUrls: ['./proposal-info-dialog.component.scss']
})
export class ProposalInfoDialogComponent {

  jobId: string = '';
  proposalId: string = '';
  proposal: Proposal = new Proposal();
  message: string = '';
  role: string = '';

  paymentPanelOpenState = false;

  constructor(
    public enumConverter: EnumConverter,
    private authService: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: {jobId: string, proposalId: string},
    private proposalService: ProposalService) {
      this.jobId = data.jobId;
      this.proposalId = data.proposalId;
      this.role = this.authService.getUserRole();
  }

  ngOnInit() {
    this.proposalService.get(this.proposalId, this.jobId).subscribe((proposal) => this.proposal = proposal);
  }

  static open(dialog: MatDialog, jobId: string, proposalId: string): MatDialogRef<ProposalInfoDialogComponent> {
    return dialog.open(ProposalInfoDialogComponent, {
      width: '50%',
      height: '100%',
      data: { jobId: jobId, proposalId: proposalId }
    })
  }

}
