import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { CreateProposalCommand } from '../models/commands/create-proposal-cmmand.model';
import { Proposal, ProposalStatus } from '../models/proposal.model';
import { AuthService } from '../../auth/services/auth.service';
import { Answer } from '../models/answer.model';
import { EditProposalPaymentCommand } from '../models/commands/edit-proposal-payment-command-model';

@Injectable({
  providedIn: 'root'
})
export class ProposalService {

  freelancerId: string = '';

  private isConfirmedSource: BehaviorSubject<boolean | null> = new BehaviorSubject<boolean | null>(null);
  public isConfirmedObserver: Observable<boolean | null> = this.isConfirmedSource.asObservable();

  httpOptions = {
    headers: { 'Content-Type': 'application/json' }
  };

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) { 
    this.authService.userObserver.subscribe((user) => {
      this.freelancerId = user?.userId as string;
    })
  }

  get(id: string): Observable<Proposal> {
    return this.httpClient.get<Proposal>(`api/job/proposal/${id}`, this.httpOptions);
  }

  getAnswers(id: string):Observable<Answer[]> {
    return this.httpClient.get<Answer[]>(`api/job/job/proposal/${id}/answers`, this.httpOptions);
  }

  getByJobId(jobId: string): Observable<Proposal[]> {
    return this.httpClient.get<Proposal[]>(`api/aggregator/job/${jobId}/proposal`, this.httpOptions);
  }

  create(createProposalCommand: CreateProposalCommand): Observable<Observable<boolean | null>> {
    createProposalCommand.freelancerId = this.freelancerId;
    return this.httpClient.post<Proposal>(`api/job/proposal`, createProposalCommand, this.httpOptions)
    .pipe(
      map((proposal) => {
        this.isConfirmed(proposal.id);
        return this.isConfirmedObserver;
      })
    );
  }

  editPayment(editProposalPaymentCommand: EditProposalPaymentCommand): Observable<any> {
    return this.httpClient.put<any>(`api/job/job/${editProposalPaymentCommand.jobId}/proposal/${editProposalPaymentCommand.proposalId}/payment`, editProposalPaymentCommand);
  }

  clientApprove(jobId: string, proposalId: string): Observable<any> {
    return this.httpClient.put<any>(`api/job/job/${jobId}/proposal/${proposalId}/status/approved`, {});
  }

  isConfirmed(id: string): void {
    const intervalId = setInterval(() => {
      this.get(id).subscribe({
        next: (proposal) => {
          if (proposal.status === ProposalStatus.SENT)
            this.isConfirmedSource.next(true);
        },
        error: (err) => this.isConfirmedSource.next(false)
      });
    }, 500);

    this.isConfirmedObserver.subscribe((isConfirmed) => {
      if (isConfirmed === null)
        return;
      clearInterval(intervalId);
      this.isConfirmedSource.next(null);
    });
  }

}
