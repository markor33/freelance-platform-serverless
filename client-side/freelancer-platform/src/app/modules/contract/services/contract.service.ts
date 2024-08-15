import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Contract } from '../models/contract.model';

@Injectable({
  providedIn: 'root'
})
export class ContractService {

  constructor(private httpClient: HttpClient) { }

  getByJob(jobId: string): Observable<Contract[]> {
    return this.httpClient.get<Contract[]>(`api/aggregator-service/contract/job/${jobId}`);
  }

  getByClient(clientId: string): Observable<Contract[]> {
    return this.httpClient.get<Contract[]>(`api/aggregator-service/contract/client/${clientId}`);
  }

  getByFreelancer(): Observable<Contract[]> {
    return this.httpClient.get<Contract[]>(`api/job-service/job/contract/freelancer`);
  }

  finish(jobId: string, contractId: string): Observable<any> {
    return this.httpClient.put<any>(`local/api/job/${jobId}/contract/${contractId}/status/finished`, {});
  }

  terminate(jobId: string, contractId: string): Observable<any> {
    return this.httpClient.put<any>(`local/api/job/${jobId}/contract/${contractId}/status/terminated`, {});
  }

  create(jobId: string, proposalId: string): Observable<any> {
    return this.httpClient.post<any>(`local/api/job/${jobId}/contract/proposal/${proposalId}`, {});
  }

}
