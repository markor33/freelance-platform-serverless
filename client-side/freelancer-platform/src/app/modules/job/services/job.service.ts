import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateJobCommand } from '../models/commands/create-job-command.model';
import { Job } from '../models/job.model';
import { AuthService } from '../../auth/services/auth.service';
import { SearchJob } from '../models/search-job.model';
import { SearchJobFilters } from '../models/search-job-filters.model';
import { EditJobCommand } from '../models/commands/edit-job-command.model';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  clientId: string = '';

  httpOptions = {
    headers: { 'Content-Type': 'application/json' }
  };

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService) {
    this.authService.userObserver.subscribe((user) => {
      this.clientId = user?.userId as string;
    });
  }

  search(queryText: string, filters: SearchJobFilters): Observable<SearchJob[]>  {
    let url = `api/aggregator/job?`;
    if (queryText !== '') url += `queryText=${queryText}&`;
    const professions = filters.professions.map((p) => `Professions=${p}&`);
    if (professions.length !== 0) url += professions.join('');
    const experienceLevels = filters.experienceLevels.map((exp) => `ExperienceLevels=${exp}&`);
    if (experienceLevels.length !== 0) url += experienceLevels.join('');
    const paymentTypes = filters.paymentTypes.map((p) => `PaymentTypes=${p}&`);
    if (paymentTypes.length !== 0) url += paymentTypes.join('')

    return this.httpClient.get<SearchJob[]>(url);
  }

  create(createJobCommand: CreateJobCommand): Observable<Job> {
    createJobCommand.clientId = this.clientId;
    return this.httpClient.post<Job>('local/api/job', createJobCommand, this.httpOptions);
  }

  edit(editJobCommand: EditJobCommand): Observable<Job> {
    return this.httpClient.put<Job>(`local/api/job/${editJobCommand.jobId}`, editJobCommand, this.httpOptions);
  }

  getAll(): Observable<Job[]> {
    return this.httpClient.get<Job[]>('api/job-service/job');
  }

  get(id: string): Observable<Job> {
    return this.httpClient.get<Job>(`api/job-service/job/${id}`);
  }

  getByClient(): Observable<Job[]> {
    return this.httpClient.get<Job[]>(`api/job-service/job/client`, this.httpOptions);
  }

  done(id: string): Observable<void> {
    return this.httpClient.put<any>(`api/job/job/${id}/status/done`, this.httpOptions);
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete<any>(`api/job/job/${id}`, this.httpOptions);
  }

}
