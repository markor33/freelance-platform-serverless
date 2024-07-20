import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Chat } from '../models/chat.model';
import { HttpClient } from '@angular/common/http';
import { Message } from '../models/message.model';
import { AuthService } from '../../auth/services/auth.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CreateChatRequest } from '../models/create-chat-request.model';
import { Proposal } from '../../job/models/proposal.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private newMessageSource = new BehaviorSubject<Message | null>(null);
  public newMessageObserver = this.newMessageSource.asObservable();

  private newMessageResponseSource = new BehaviorSubject<Message | null>(null);
  public newMessageResponseObserver = this.newMessageResponseSource.asObservable();

  private connection: signalR.HubConnection | null = null;

  constructor(
    private authService: AuthService,
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService) { 
      this.authService.userObserver.subscribe((user) => {
        if (user === null) {
          this.connection?.stop();
          return;
        }
        this.configureConnection();
      });
  }

  sendMessage(chatId: string, text: string): Promise<void> {
    return this.connection?.send('newMessage', chatId, text) as Promise<void>;
  }

  get(): Observable<Chat[]> {
    return this.httpClient.get<Chat[]>('api/aggregator/chat');
  }

  getMessages(chatId: string): Observable<Message[]> {
    return this.httpClient.get<Message[]>(`api/notifychat-service/chat/${chatId}/messages`);
  }

  create(jobId: string, proposal: Proposal, message: string): Observable<Chat> 
  {
    var request: CreateChatRequest = {
      jobId: jobId,
      proposalId: proposal.id,
      freelancerId: proposal.freelancerId,  
      initialMessage: message
    };
    return this.httpClient.post<Chat>('api/notifychat-service/chat', request);
  }

  private configureConnection() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`hub/chat`, { accessTokenFactory: () => this.jwtHelper.tokenGetter()})
      .build();

    this.connection.start()
    .then(() => console.log('Chat OK'))
    .catch((err) => console.log(err));

    this.connection.on('newMessage', (message: Message) => {
      message.date = new Date(message.date);
      this.newMessageSource.next(message);
    });

    this.connection.on('newMessageResponse', (message: Message) => {
      message.date = new Date(message.date);
      this.newMessageResponseSource.next(message);
    });
  }

}
