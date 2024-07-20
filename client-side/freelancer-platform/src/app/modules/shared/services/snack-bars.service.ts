import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackBarsService {

  constructor(private snackBar: MatSnackBar) { }

  primary(message: string): MatSnackBarRef<TextOnlySnackBar> {
    const snackBar = this.snackBar.open(message, 'Ok', {
      duration: 3000
    });
    return snackBar;
  }

  error(message: string): MatSnackBarRef<TextOnlySnackBar> {
    const snackbar = this.snackBar.open(message, 'Ok', {
      duration: 3000,
      panelClass: ['error-snackbar']
    });
    return snackbar;
  }

}
