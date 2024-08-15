import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateJobDialogComponent } from './create-job-dialog/create-job-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatChipsModule} from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { ApplyDialogComponent } from './apply-dialog/apply-dialog.component';
import {MatDividerModule} from '@angular/material/divider';
import { JobSearchComponent } from './job-search/job-search.component';
import { JobsManagementComponent } from './jobs-management/jobs-management.component';
import {MatTableModule} from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatPaginatorModule} from '@angular/material/paginator';
import { JobInfoDialogComponent } from './jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { ProposalsManagementComponent } from './proposals-management/proposals-management.component';
import { ProposalInfoDialogComponent } from './proposals-management/dialogs/proposal-info-dialog/proposal-info-dialog.component';
import { SharedModule } from '../shared/shared.module';
import {MatTabsModule} from '@angular/material/tabs';
import { MatSortModule } from '@angular/material/sort';
import { StartContactComponent } from './proposals-management/dialogs/proposal-info-dialog/actions/start-contact/start-contact.component';
import { ClientAcceptComponent } from './proposals-management/dialogs/proposal-info-dialog/actions/client-accept/client-accept.component';
import { FreelancerAcceptComponent } from './proposals-management/dialogs/proposal-info-dialog/actions/freelancer-accept/freelancer-accept.component';
import { AuthGuard } from '../auth/helpers/auth.guard';
import { RoleGuard } from '../auth/helpers/role.guard';
import { MatMenuModule } from '@angular/material/menu';
import { EditJobDialogComponent } from './jobs-management/dialogs/edit-job-dialog/edit-job-dialog.component';

export const jobRoutes: Routes = [
  { path: 'job', component: JobSearchComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: ['Freelancer']} },
  { path: 'job-management', component: JobsManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: ['Employeer']} },
  { path: 'job/:id/proposal-management', component: ProposalsManagementComponent, canActivate: [AuthGuard, RoleGuard], data: { roles: ['Employeer']} }
]

@NgModule({
  declarations: [
    CreateJobDialogComponent,
    JobSearchComponent,
    ApplyDialogComponent,
    JobsManagementComponent,
    JobInfoDialogComponent,
    ProposalsManagementComponent,
    ProposalInfoDialogComponent,
    StartContactComponent,
    ClientAcceptComponent,
    FreelancerAcceptComponent,
    EditJobDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatExpansionModule,
    MatChipsModule,
    MatDividerModule,
    MatIconModule,
    HttpClientModule,
    MatDialogModule,
    MatCheckboxModule,
    MatMenuModule,
    MatTableModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    RouterModule,
    SharedModule,
    MatTabsModule,
    MatSortModule,
    MatButtonModule
  ],
  exports: [

  ]
})
export class JobModule { }
