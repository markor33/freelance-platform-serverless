import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditCertificationCommand } from 'src/app/modules/freelancer/models/commands/edit-certification-command.model';
import { Certification } from 'src/app/modules/freelancer/models/freelancer.model';
import { FreelancerService } from 'src/app/modules/freelancer/services/freelancer.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { convertToUTCDate } from 'src/app/modules/shared/utils/date-helper.util';

@Component({
  selector: 'app-edit-certification-dialog',
  templateUrl: './edit-certification-dialog.component.html',
  styleUrls: ['./edit-certification-dialog.component.scss']
})
export class EditCertificationDialogComponent {

  editCertificationCommand: EditCertificationCommand;

  constructor(
    private dialogRef: MatDialogRef<EditCertificationDialogComponent>,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { certification: Certification }) {
      this.editCertificationCommand = new EditCertificationCommand(data.certification);
  }

  edit() {
    this.editCertificationCommand.start = convertToUTCDate(this.editCertificationCommand.start);
    this.editCertificationCommand.end = convertToUTCDate(this.editCertificationCommand.end);
    this.freelancerService.editCertification(this.editCertificationCommand).subscribe({
      complete: () => {
        this.snackBars.primary('Certification edit successful');
        this.dialogRef.close();
      },
      error: (err) => this.snackBars.error(err.error[0])
    });
  }

  static open(dialog: MatDialog, certification: Certification): MatDialogRef<EditCertificationDialogComponent> {
    return dialog.open(EditCertificationDialogComponent, {
      width: '40%',
      height: '72%',
      data: { certification: certification }
    });
  }

}
