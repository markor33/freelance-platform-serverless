import { Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FreelancerService } from '../../../services/freelancer.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';

@Component({
  selector: 'app-set-profile-picture-dialog',
  templateUrl: './set-profile-picture-dialog.component.html',
  styleUrls: ['./set-profile-picture-dialog.component.scss']
})
export class SetProfilePictureDialogComponent {

  profilePicture!: File;

  constructor(
    private freelancerService: FreelancerService,
    private snackBarService: SnackBarsService,
    private matDialogRef: MatDialogRef<SetProfilePictureDialogComponent>) {

  }

  submit() {
    this.freelancerService.setProfilePicture(this.profilePicture).subscribe({
      complete: () => {
        this.snackBarService.primary('Success');
        this.matDialogRef.close();
      }
    });
  }

  onFileSelected(event: any) {
    this.profilePicture = event.target.files[0];
  }

  static open(dialog: MatDialog) {
    dialog.open(SetProfilePictureDialogComponent, {
      width: '40%'
    });
  }

}
