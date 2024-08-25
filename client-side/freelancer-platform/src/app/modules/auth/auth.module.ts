import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {MatCardModule} from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {MatDividerModule} from '@angular/material/divider';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import {MatDialogModule} from '@angular/material/dialog';
import { SharedModule } from '../shared/shared.module';
import { CallbackComponent } from './callback/callback.component';
import {RegisterComponent} from "./register/register.component";

const routes: Routes = [
  { path: 'auth-callback', component: CallbackComponent },
  { path: 'auth/register', component: RegisterComponent }
];

@NgModule({
  declarations: [
    CallbackComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    HttpClientModule,
    MatCardModule,
    MatInputModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    MatDividerModule,
    MatRadioModule,
    MatCheckboxModule,
    MatSnackBarModule,
    RouterModule.forRoot(routes),
    ReactiveFormsModule
  ]
})
export class AuthModule { }
