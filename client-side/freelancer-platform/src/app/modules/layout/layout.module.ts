import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { MatDialogModule } from '@angular/material/dialog';
import {MatToolbarModule} from '@angular/material/toolbar';
import { FreelancerModule, freelancerRoutes } from '../freelancer/freelancer.module';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, Routes } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import { JobModule, jobRoutes } from '../job/job.module';
import { MatBadgeModule } from '@angular/material/badge';
import { NotificationModule } from '../notification/notification.module';
import { chatRoutes } from '../chat/chat.module';
import { contractRoutes } from '../contract/contract.module';
import { AuthGuard } from '../auth/helpers/auth.guard';
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

const routes: Routes = [
  { path: '', component: HomeComponent, children: [
    ...freelancerRoutes,
    ...jobRoutes,
    ...chatRoutes,
    ...contractRoutes
  ],
    canActivate: [AuthGuard]}
];

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    FreelancerModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    JobModule,
    MatMenuModule,
    MatBadgeModule,
    NotificationModule,
    RouterModule.forRoot(routes)
  ],
  exports: [HomeComponent]
})
export class LayoutModule { }
