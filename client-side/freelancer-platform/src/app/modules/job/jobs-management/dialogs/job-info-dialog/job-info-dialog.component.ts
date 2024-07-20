import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Job } from '../../../models/job.model';
import { EnumConverter } from 'src/app/modules/shared/utils/enum-string-converter.util';
import { JobService } from '../../../services/job.service';

@Component({
  selector: 'app-job-info-dialog',
  templateUrl: './job-info-dialog.component.html',
  styleUrls: ['./job-info-dialog.component.scss']
})
export class JobInfoDialogComponent {

  job: Job = new Job();

  constructor(
    public enumConverter: EnumConverter,
    @Inject(MAT_DIALOG_DATA) public data: { jobId: string },
    private jobService: JobService) {
      this.jobService.get(data.jobId).subscribe((job) => this.job = job)
  }

  static open(dialog: MatDialog, jobId: string): MatDialogRef<JobInfoDialogComponent> {
    return dialog.open(JobInfoDialogComponent, {
      width: '50%',
      height: '80%',
      data: { jobId: jobId }
    });
  }

}
