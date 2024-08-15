import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss']
})
export class ConfirmationDialogComponent {

  text:string = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { text: string },
    private dialogRef: MatDialogRef<ConfirmationDialogComponent>) {
    this.text = data.text;
  }

  submit(isConfirmed: boolean) {
    this.dialogRef.close(isConfirmed);
  }

  static open(dialog: MatDialog, text: string): MatDialogRef<ConfirmationDialogComponent> {
    return dialog.open(ConfirmationDialogComponent, {
      width: "30%",
      height: "20%",
      data: { text: text}
    });
  }

}
