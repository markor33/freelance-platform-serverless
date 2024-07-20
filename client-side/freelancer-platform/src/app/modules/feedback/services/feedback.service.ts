import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Feedback, FinishedContract } from '../models/feedback.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  constructor(private httpClient: HttpClient) { }

  getByFreelancer(freelancerId: string): Observable<Feedback[]> {
    return this.httpClient.get<Feedback[]>(`api/aggregator/feedback/freelancer/${freelancerId}`);
  }

  getByContract(contractId: string): Observable<FinishedContract> {
    return this.httpClient.get<FinishedContract>(`api/feedback-service/FinishedContract/${contractId}`);
  }

  create(contractId: string, feedback: Feedback): Observable<any> {
    return this.httpClient.post<any>(`api/feedback-service/FinishedContract/${contractId}/feedback`, feedback);
  }

}
