import { Component, ViewChild } from '@angular/core';
import { JobService } from '../services/job.service';
import { Job, JobStatus } from '../models/job.model';
import { MatDialog } from '@angular/material/dialog';
import { CreateJobDialogComponent } from '../create-job-dialog/create-job-dialog.component';
import { JobInfoDialogComponent } from './dialogs/job-info-dialog/job-info-dialog.component';
import { SnackBarsService } from '../../shared/services/snack-bars.service';
import { Router } from '@angular/router';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ConfirmationDialogComponent } from '../../shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { EditJobDialogComponent } from './dialogs/edit-job-dialog/edit-job-dialog.component';

@Component({
  selector: 'app-jobs-management',
  templateUrl: './jobs-management.component.html',
  styleUrls: ['./jobs-management.component.scss']
})
export class JobsManagementComponent {

  public hoveredRow: any = null;

  public jobs: MatTableDataSource<any> = new MatTableDataSource();
  public displayedColumns: string[] = ['title', 'numOfProposals', 'currentlyInterviewing', 'status', 'numOfActiveContracts', 'numOfFinishedContracts', 'actions'];

  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private jobService: JobService,
    private dialog: MatDialog,
    private snackBarService: SnackBarsService,
    private router: Router,
    public enumConverter: EnumConverter) { }

  ngAfterViewInit() {
    this.jobs.sort = this.sort;
  }

  ngOnInit() {
    this.jobService.getByClient().subscribe({
      next: (jobs) => this.jobs.data = jobs
    });
  }

  openCreateJobDialog() {
    const dialog = CreateJobDialogComponent.open(this.dialog);
    dialog.afterClosed().subscribe(result => {
      if (!result)
        return;
      this.jobs.data = [...this.jobs.data, result];
    });
  }

  openJobInfoDialog(job: Job) {
    JobInfoDialogComponent.open(this.dialog, job.id);
  }

  editJobInfoDialog(job: Job) {
    EditJobDialogComponent.open(this.dialog, job);
  }

  openProposals(job: Job) {
    this.router.navigate(['/job', job.id, 'proposal-management']);
  }

  jobDone(job: Job) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to mark your job as done.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.jobService.done(job.id).subscribe({
        complete: () => {
          this.snackBarService.primary('Job is done');
          job.status = JobStatus.DONE;
        },
        error: (err) => this.snackBarService.error(err.error[0])
      });
    });
  }

  deleteJob(job: Job) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to delete your job.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.jobService.delete(job.id).subscribe({
        complete: () => this.jobDeletedSuccessfully(job),
        error: (err) => this.snackBarService.error(err.error[0])
      });
    });
  }

  jobDeletedSuccessfully(job: Job) {
    const index = this.jobs.data.indexOf(job);
    this.jobs.data.splice(index, 1);
    this.jobs.data = [...this.jobs.data];
    this.snackBarService.primary('Job deleted successfully');
  }

}
