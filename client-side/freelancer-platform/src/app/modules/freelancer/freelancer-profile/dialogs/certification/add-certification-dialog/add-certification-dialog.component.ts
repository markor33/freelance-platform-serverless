import { Component } from '@angular/core';
import { AddCertificationCommand } from '../../../../models/commands/add-certification-command.model';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FreelancerService } from '../../../../services/freelancer.service';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { convertToUTCDate } from 'src/app/modules/shared/utils/date-helper.util';

@Component({
  selector: 'app-add-certification-dialog',
  templateUrl: './add-certification-dialog.component.html',
  styleUrls: ['./add-certification-dialog.component.scss']
})
export class AddCertificationDialogComponent {

  addCertificationCommand: AddCertificationCommand = new AddCertificationCommand();
  attended = {
    start: new Date(),
    end: new Date()
  }

  constructor(
    private dialogRef: MatDialogRef<AddCertificationDialogComponent>,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService) {}

  add() {
    this.addCertificationCommand.start = convertToUTCDate(this.attended.start);
    this.addCertificationCommand.end = convertToUTCDate(this.attended.end);
    this.freelancerService.addCertification(this.addCertificationCommand).subscribe({
      complete: this.certificationSuccessfullyAdded.bind(this)
    });
  }

  certificationSuccessfullyAdded() {
    this.snackBars.primary('Certification successfully added');
    this.dialogRef.close()
  }

  static open(dialog: MatDialog): MatDialogRef<AddCertificationDialogComponent> {
    return dialog.open(AddCertificationDialogComponent, {
      width: '40%',
      height: '72%'
    });
  }

}
