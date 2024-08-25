import { Component } from '@angular/core';
import { Registration, Role } from '../models/registration.model';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RegisterCompleteDialogComponent } from './register-complete-dialog/register-complete-dialog.component';
import { SnackBarsService } from '../../shared/services/snack-bars.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  hidePassword: boolean = true;
  registration: Registration = new Registration();
  roles = [Role.FREELANCER, Role.CLIENT];

  constructor(
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog,
    private snackBars: SnackBarsService) { }

  goToLogin() {
    this.authService.login();
  }

  register(): void {
    if (!this.validateForm())
      return;
    this.authService.register(this.registration).subscribe({
      complete: this.registrationComplete.bind(this),
      error: this.registrationError.bind(this)
    });
  }

  registrationComplete(): void {
    const dialog = this.dialog.open(RegisterCompleteDialogComponent, {
      height: '20%',
      width: '30%',
      panelClass: ['register-complete-dialog']
    });
    dialog.afterClosed().subscribe(() => this.authService.login());
  }

  registrationError(err: any): void {
    this.snackBars.error(err.error[0] as string);
  }

  validateForm(): boolean {
    if (this.registration.password === this.registration.confirmPassword)
      return true;
    this.snackBars.error('Passwords do not match');
    return false;
  }

}
