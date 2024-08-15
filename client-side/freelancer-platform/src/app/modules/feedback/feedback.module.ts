import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FeedbackDialogComponent } from './dialogs/feedback-dialog/feedback-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {MatSliderModule} from '@angular/material/slider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [
    FeedbackDialogComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    MatDialogModule,
    MatIconModule,
    FormsModule,
    MatButtonModule,
    MatSliderModule,
    MatInputModule
  ]
})
export class FeedbackModule { }
