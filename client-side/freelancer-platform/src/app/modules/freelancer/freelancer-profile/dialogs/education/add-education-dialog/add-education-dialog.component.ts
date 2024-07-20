import { Component } from '@angular/core';
import { FreelancerService } from '../../../../services/freelancer.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { convertToUTCDate } from 'src/app/modules/shared/utils/date-helper.util';
import { AddEducationCommand } from '../../../../models/commands/add-education-command.model';

@Component({
  selector: 'app-add-education-dialog',
  templateUrl: './add-education-dialog.component.html',
  styleUrls: ['./add-education-dialog.component.scss']
})
export class AddEducationDialogComponent {

  addEducationCommand: AddEducationCommand = new AddEducationCommand();
  attended = {
    start: new Date(),
    end: new Date()
  }

  constructor(
    private dialogRef: MatDialogRef<AddEducationDialogComponent>,
    private freelancerService: FreelancerService, 
    private snackBars: SnackBarsService) { }

  add() {
    this.addEducationCommand.start = convertToUTCDate(this.attended.start);
    this.addEducationCommand.end = convertToUTCDate(this.attended.end);
    this.freelancerService.addEducation(this.addEducationCommand).subscribe({
      complete: this.educationSuccessfullyAdded.bind(this),
      error: (err) => console.log(err)
    });
  }

  educationSuccessfullyAdded(): void {
    this.snackBars.primary('Education successfully added');
    this.dialogRef.close()
  }

  static open(dialog: MatDialog): MatDialogRef<AddEducationDialogComponent> {
    return dialog.open(AddEducationDialogComponent, {
      width: '40%',
      height: '50%'
    });
  }

}