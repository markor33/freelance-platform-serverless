import { Component, Inject } from '@angular/core';
import { ProfileSummary } from '../../../models/freelancer.model';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { FreelancerService } from '../../../services/freelancer.service';
import { EditCertificationDialogComponent } from '../certification/edit-certification-dialog/edit-certification-dialog.component';

@Component({
  selector: 'app-edit-profile-summary-dialog',
  templateUrl: './edit-profile-summary-dialog.component.html',
  styleUrls: ['./edit-profile-summary-dialog.component.scss']
})
export class EditProfileSummaryDialogComponent {

  profileSummary: ProfileSummary;

  constructor(private dialogRef: MatDialogRef<EditCertificationDialogComponent>,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { profileSummary: ProfileSummary }) {
      this.profileSummary = data.profileSummary;
  }

  edit() {
    this.freelancerService.editProfileSummary(this.profileSummary).subscribe({
      complete: () => {
        this.snackBars.primary('Profile summary edit successful');
        this.dialogRef.close();
      },
      error: (err) => this.snackBars.error(err.error[0])
    });
  }

  static open(dialog: MatDialog, profileSummary: ProfileSummary): MatDialogRef<EditProfileSummaryDialogComponent> {
    return dialog.open(EditProfileSummaryDialogComponent, {
      width: '40%',
      height: '60%',
      data: { profileSummary: {...profileSummary} }
    });
  }

}
