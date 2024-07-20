import { Component, ViewChild } from '@angular/core';
import { ContractService } from '../services/contract.service';
import { ActivatedRoute } from '@angular/router';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { JobInfoDialogComponent } from '../../job/jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { Contract, ContractStatus } from '../models/contract.model';
import { SnackBarsService } from '../../shared/services/snack-bars.service';
import { ConfirmationDialogComponent } from '../../shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { FeedbackDialogComponent } from '../../feedback/dialogs/feedback-dialog/feedback-dialog.component';
import { Contact } from '../../shared/models/contact.model';

@Component({
  selector: 'app-contract-management',
  templateUrl: './contract-management.component.html',
  styleUrls: ['./contract-management.component.scss']
})
export class ContractManagementComponent {

  jobId: string = '';
  jobTitle: string = '';
  contracts: MatTableDataSource<any> = new MatTableDataSource();

  @ViewChild(MatSort) sort!: MatSort;

  public hoveredRow: any = null;
  public displayedColumns: string[] = ['freelancer', 'status', 'started', 'finished', 'payment', 'feedback', 'actions'];

  constructor(
    private contractService: ContractService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private snackBarService: SnackBarsService,
    public enumConverter: EnumConverter) {
      this.jobId = this.route.snapshot.paramMap.get('id') as string;
  }

  ngAfterViewInit() {
    this.contracts.sort = this.sort;
  }

  ngOnInit() {
    this.contractService.getByJob(this.jobId).subscribe((contracts) => {
      this.contracts.data = contracts;
      this.jobTitle = this.contracts.data[0].jobTitle;
    });
  }

  finish(contract: Contract) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to finish the contract.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.contractService.finish(this.jobId, contract.id).subscribe(() => {
        this.ngOnInit();
        this.snackBarService.primary('Contract finished successfully');
      });
    });
  }

  terminate(contract: Contract) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to terminate the contract.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.contractService.terminate(this.jobId, contract.id).subscribe(() => {
        this.ngOnInit();
        this.snackBarService.primary('Contract terminated successfully');
      });
    });
  }

  openJobInfoDialog() {
    JobInfoDialogComponent.open(this.dialog, this.jobId);
  }

  openFeedbackDialog(jobId: string, jobTitle: string, contractId: string) {
    FeedbackDialogComponent.open(this.dialog, jobId, jobTitle, contractId);
  }

}
