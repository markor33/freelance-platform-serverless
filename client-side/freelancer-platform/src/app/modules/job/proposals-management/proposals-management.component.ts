import { Component, ViewChild } from '@angular/core';
import { ProposalService } from '../services/proposal.service';
import { Proposal } from '../models/proposal.model';
import { Job } from '../models/job.model';
import { MatDialog } from '@angular/material/dialog';
import { JobInfoDialogComponent } from '../jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ProposalInfoDialogComponent } from './dialogs/proposal-info-dialog/proposal-info-dialog.component';
import { JobService } from '../services/job.service';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-proposals-management',
  templateUrl: './proposals-management.component.html',
  styleUrls: ['./proposals-management.component.scss']
})
export class ProposalsManagementComponent {

  jobId: string = '';
  job: Job = new Job();
  proposals: MatTableDataSource<any> = new MatTableDataSource();

  @ViewChild(MatSort) sort!: MatSort;

  public displayedColumns: string[] = ['freelancer', 'location', 'created', 'status', 'freelancerAverageRating'];

  constructor(
    private proposalService: ProposalService,
    private jobService: JobService,
    private dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    public enumConverter: EnumConverter) {
      this.jobId = this.route.snapshot.paramMap.get('id') as string;
      console.log(this.jobId)
  }

  ngAfterViewInit() {
    this.proposals.sort = this.sort;
  }

  ngOnInit() {
    this.jobService.get(this.jobId).subscribe((job) => this.job = job);
    this.proposalService.getByJobId(this.jobId).subscribe((proposals) => this.proposals.data = proposals);
  }

  openJobInfoDialog() {
    JobInfoDialogComponent.open(this.dialog, this.job.id);
  }

  openProposalInfoDialog(proposal: Proposal) {
    ProposalInfoDialogComponent.open(this.dialog, this.job.id, proposal.id);
  }

  openFreelancerProfile(freelancerId: string) {
    this.router.navigate([`freelancer/profile/${freelancerId}`]);
  }

  getRange(value: number): number[] {
    return Array(Math.floor(value)).fill(0).map((x, i) => i);
  }

}
