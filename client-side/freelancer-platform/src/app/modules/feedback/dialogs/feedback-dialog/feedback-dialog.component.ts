import { Component, Inject } from '@angular/core';
import { Feedback, FinishedContract } from '../../models/feedback.model';
import { FeedbackService } from '../../services/feedback.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { JobInfoDialogComponent } from 'src/app/modules/job/jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { AuthService } from 'src/app/modules/auth/services/auth.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';

@Component({
  selector: 'app-feedback-dialog',
  templateUrl: './feedback-dialog.component.html',
  styleUrls: ['./feedback-dialog.component.scss']
})
export class FeedbackDialogComponent {

  role: string = '';
  jobId: string = '';
  jobTitle: string = '';
  contractId: string = '';
  finishedContract: FinishedContract = new FinishedContract();
  feedback: Feedback = new Feedback();

  constructor(
    private dialog: MatDialog,
    private feedbackService: FeedbackService,
    private authService: AuthService,
    private snackBarService: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { jobId: string, jobTitle: string, contractId: string }) {
      this.role = this.authService.getUserRole();
      this.jobId = data.jobId;
      this.jobTitle = data.jobTitle;
      this.contractId = data.contractId;
  }

  ngOnInit() {
    this.feedbackService.getByContract(this.contractId).subscribe((finishedContract) => this.finishedContract = finishedContract);
  }

  submitFeedback() {
    this.feedbackService.create(this.contractId, this.feedback).subscribe({
      complete: () => {
        this.snackBarService.primary('Feedback submitted successfully');
        this.ngOnInit();
      },
      error: (err) => this.snackBarService.error(err.error[0])
    });
  }

  openJobInfoDialog() {
    JobInfoDialogComponent.open(this.dialog, this.jobId);
  }

  getRange(value: number): number[] {
    return Array(value).fill(0).map((x, i) => i);
  }

  static open(dialog: MatDialog, jobId: string, jobTitle: string, contractId: string): MatDialogRef<FeedbackDialogComponent> {
    return dialog.open(FeedbackDialogComponent, {
      width: '50%',
      height: '70%',
      data: {
        jobId: jobId,
        jobTitle: jobTitle,
        contractId: contractId
      }
    });
  }

}
