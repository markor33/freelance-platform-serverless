import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { EditEducationCommand } from 'src/app/modules/freelancer/models/commands/edit-education-command.model';
import { Education } from 'src/app/modules/freelancer/models/freelancer.model';
import { FreelancerService } from 'src/app/modules/freelancer/services/freelancer.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { convertToUTCDate } from 'src/app/modules/shared/utils/date-helper.util';

@Component({
  selector: 'app-edit-education-dialog',
  templateUrl: './edit-education-dialog.component.html',
  styleUrls: ['./edit-education-dialog.component.scss']
})
export class EditEducationDialogComponent {

  editEducationCommand: EditEducationCommand;

  constructor(
    private dialogRef: MatDialogRef<EditEducationDialogComponent>,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { education: Education }) {
      this.editEducationCommand = new EditEducationCommand(data.education);
  }

  edit() {
    this.editEducationCommand.start = convertToUTCDate(this.editEducationCommand.start);
    this.editEducationCommand.end = convertToUTCDate(this.editEducationCommand.end);
    this.freelancerService.editEducation(this.editEducationCommand).subscribe({
      complete: () => {
        this.snackBars.primary('Education edit successful');
        this.dialogRef.close();
      },
      error: (err) => console.log(err)
    });
  }

  // this.snackBars.error(err.error[0])

  static open(dialog: MatDialog, education: Education) {
    return dialog.open(EditEducationDialogComponent, {
      width: '40%',
      height: '50%',
      data: { education: education }
    });
  }
}
