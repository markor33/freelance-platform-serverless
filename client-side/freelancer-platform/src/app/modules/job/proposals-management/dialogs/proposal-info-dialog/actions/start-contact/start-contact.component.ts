import { Component, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ChatService } from 'src/app/modules/chat/services/chat.service';
import { Proposal } from 'src/app/modules/job/models/proposal.model';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { ProposalInfoDialogComponent } from '../../proposal-info-dialog.component';

@Component({
  selector: 'app-start-contact',
  templateUrl: './start-contact.component.html',
  styleUrls: ['./start-contact.component.scss']
})
export class StartContactComponent {

  @Input() jobId: string = '';
  @Input() proposal: Proposal = new Proposal();
  message: string = '';

  constructor(
    private chatService: ChatService,
    private snackBarService: SnackBarsService,
    private router: Router,
    private dialogRef: MatDialogRef<ProposalInfoDialogComponent>) {

  }

  sendMessage() {
    this.chatService.create(this.jobId, this.proposal, this.message).subscribe(() => {
      this.snackBarService.primary('Message sent to freelancer successfully');
      this.dialogRef.close();
      this.router.navigate(['/chat']);
    });
  }
}
