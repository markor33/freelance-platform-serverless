import { Component, ViewChild } from '@angular/core';
import { ContractService } from '../services/contract.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthService } from '../../auth/services/auth.service';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { JobInfoDialogComponent } from '../../job/jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { FeedbackDialogComponent } from '../../feedback/dialogs/feedback-dialog/feedback-dialog.component';

@Component({
  selector: 'app-my-contracts',
  templateUrl: './my-contracts.component.html',
  styleUrls: ['./my-contracts.component.scss']
})
export class MyContractsComponent {

  role: string = '';
  userId: string = '';

  contracts: MatTableDataSource<any> = new MatTableDataSource();

  @ViewChild(MatSort) sort!: MatSort;

  public displayedColumns: string[] = ['jobTitle', 'freelancer', 'status', 'started', 'finished', 'payment', 'feedback'];

  constructor(
    private contractService: ContractService,
    private authService: AuthService,
    private dialog: MatDialog,
    public enumConverter: EnumConverter
  ) {
    this.authService.userObserver.subscribe((user) => {
      this.userId = user?.userId as string;
      this.role = user?.role as string;
    });
  }

  ngAfterViewInit() {
    this.contracts.sort = this.sort;
  }

  ngOnInit() {
    if (this.role === 'Employeer')
      this.contractService.getByClient(this.userId).subscribe((contracts) => {
        this.contracts.data = contracts;
        console.log(contracts);
      });
    else if (this.role === 'Freelancer') {
      this.displayedColumns.splice(1, 1);
      this.contractService.getByFreelancer().subscribe((contracts) => this.contracts.data = contracts);
    }
  }

  openJobInfoDialog(jobId: string) {
    JobInfoDialogComponent.open(this.dialog, jobId);
  }

  openFeedbackDialog(jobId: string, jobTitle: string, contractId: string) {
    FeedbackDialogComponent.open(this.dialog, jobId, jobTitle, contractId);
  }

}
