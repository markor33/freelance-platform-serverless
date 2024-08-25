import { Component, Inject } from '@angular/core';
import {MatDialog, MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-register-complete-dialog',
  templateUrl: './register-complete-dialog.component.html',
  styleUrls: ['./register-complete-dialog.component.scss']
})
export class RegisterCompleteDialogComponent {

  constructor(
    private dialogRef: MatDialogRef<RegisterCompleteDialogComponent>,
  ) { }

  close(): void {
    this.dialogRef.close();
  }

}
