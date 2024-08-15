import { Component } from '@angular/core';
import {Role} from "../../shared/models/role.model";
import {AuthService} from "../services/auth.service";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-choose-role-dialog',
  templateUrl: './choose-role-dialog.component.html',
  styleUrls: ['./choose-role-dialog.component.scss']
})
export class ChooseRoleDialogComponent {

  role: Role = Role.Freelancer;
  isCompleted: boolean = false;

  constructor(
    private authService: AuthService,
    private dialogRef: MatDialogRef<ChooseRoleDialogComponent>,
    private dialog: MatDialog
  ) {
    this.dialogRef.afterClosed().subscribe(() => {
      if (!this.isCompleted)
        ChooseRoleDialogComponent.open(this.dialog);
    });
  }

  submit() {
    this.authService.chooseRole(this.role).subscribe(() => {
      this.isCompleted = true;
      this.dialogRef.close();
    });
  }

  static open(dialog: MatDialog): MatDialogRef<ChooseRoleDialogComponent> {
    return dialog.open(ChooseRoleDialogComponent, {
      width: '40%'
    });
  }

  protected readonly Role = Role;
}
