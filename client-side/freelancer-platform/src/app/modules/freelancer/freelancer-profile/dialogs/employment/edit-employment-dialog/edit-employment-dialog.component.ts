import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Employment } from 'src/app/modules/freelancer/models/freelancer.model';
import { FreelancerService } from 'src/app/modules/freelancer/services/freelancer.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { EditCertificationDialogComponent } from '../../certification/edit-certification-dialog/edit-certification-dialog.component';
import { EditEmploymentCommand } from 'src/app/modules/freelancer/models/commands/edit-employment-command.model';
import { convertToUTCDate } from 'src/app/modules/shared/utils/date-helper.util';

@Component({
  selector: 'app-edit-employment-dialog',
  templateUrl: './edit-employment-dialog.component.html',
  styleUrls: ['./edit-employment-dialog.component.scss']
})
export class EditEmploymentDialogComponent {

  editEmploymentCommand: EditEmploymentCommand;

  constructor(private dialogRef: MatDialogRef<EditCertificationDialogComponent>,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { employment: Employment }) {
      this.editEmploymentCommand = new EditEmploymentCommand(data.employment);
  }

  edit() {
    this.editEmploymentCommand.start = convertToUTCDate(this.editEmploymentCommand.start);
    this.editEmploymentCommand.end = convertToUTCDate(this.editEmploymentCommand.end);
    this.freelancerService.editEmployment(this.editEmploymentCommand).subscribe({
      complete: () => {
        this.snackBars.primary('Employment edit successful');
        this.dialogRef.close();
      },
      error: (err) => this.snackBars.error(err.error[0])
    });
  }

  static open(dialog: MatDialog, employment: Employment): MatDialogRef<EditEmploymentDialogComponent> {
    return dialog.open(EditEmploymentDialogComponent, {
      width: '40%',
      height: '71%',
      data: { employment: employment }
    });
  }
}
