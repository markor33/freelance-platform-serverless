import { Component } from '@angular/core';
import { JobService } from '../services/job.service';
import { Job } from '../models/job.model';
import { MatDialog } from '@angular/material/dialog';
import { ApplyDialogComponent } from '../apply-dialog/apply-dialog.component';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { ProfessionService } from '../../shared/services/profession.service';
import { Profession } from '../../shared/models/profession.mode';
import { SearchJobFilters } from '../models/search-job-filters.model';
import { ExperienceLevel } from '../../shared/models/experience-level.model';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { PaymentType } from '../models/payment.model';
import { SearchJob } from '../models/search-job.model';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-job-search',
  templateUrl: './job-search.component.html',
  styleUrls: ['./job-search.component.scss']
})
export class JobSearchComponent {

  professions: Profession[] = [];
  // jobs: SearchJob[] = [];
  jobs: Job[] = [];

  queryText: string = '';
  searchJobFilters: SearchJobFilters = new SearchJobFilters();
  experienceLevels = ExperienceLevel;
  paymentTypes = PaymentType;

  constructor(
    private jobService: JobService,
    private dialog: MatDialog,
    private professionService: ProfessionService,
    private authService: AuthService,
    public enumConverter: EnumConverter) {
    }

  ngOnInit() {
    this.professionService.get().subscribe((professios) => this.professions = professios);
    this.search();
  }

  search() {
    this.jobService.getAll().subscribe((jobs) => this.jobs = jobs);
    // this.jobService.search(this.queryText, this.searchJobFilters).subscribe((jobs) => this.jobs = jobs);
  }

  onProfessionChange(event: MatCheckboxChange, profession: string) {
    if (event.checked) {
      this.searchJobFilters.professions.push(profession);
    }
    else {
      const index = this.searchJobFilters.professions.indexOf(profession);
      if (index >= 0) {
          this.searchJobFilters.professions.splice(index, 1);
      }
    }
    this.search();
  }

  onExperienceLevelChange(event: MatCheckboxChange, experienceLevel: ExperienceLevel) {
    if (event.checked) {
      this.searchJobFilters.experienceLevels.push(experienceLevel);
    }
    else {
      const index = this.searchJobFilters.experienceLevels.indexOf(experienceLevel);
      if (index >= 0) {
          this.searchJobFilters.experienceLevels.splice(index, 1);
      }
    }
    this.search();
  }

  onPaymentTypeChange(event: MatCheckboxChange, paymentType: PaymentType) {
    if (event.checked) {
      this.searchJobFilters.paymentTypes.push(paymentType);
      this.search();
    }
    else {
      const index = this.searchJobFilters.paymentTypes.indexOf(paymentType);
      if (index >= 0) {
          this.searchJobFilters.paymentTypes.splice(index, 1);
      }
    }
    this.search();
  }

  getRange(value: number): number[] {
    return Array(Math.floor(value)).fill(0).map((x, i) => i);
  }

  apply(jobId: string) {
    ApplyDialogComponent.open(this.dialog, jobId);
  }

}
