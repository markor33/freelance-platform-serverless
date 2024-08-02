import { Injectable } from '@angular/core';
import {BehaviorSubject, connect, Observable} from 'rxjs';
import { Chat } from '../models/chat.model';
import { HttpClient } from '@angular/common/http';
import { Message } from '../models/message.model';
import { AuthService } from '../../auth/services/auth.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CreateChatRequest } from '../models/create-chat-request.model';
import { Proposal } from '../../job/models/proposal.model';
import { fetchAuthSession } from "aws-amplify/auth"

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private newMessageSource = new BehaviorSubject<Message | null>(null);
  public newMessageObserver = this.newMessageSource.asObservable();

  private newMessageResponseSource = new BehaviorSubject<Message | null>(null);
  public newMessageResponseObserver = this.newMessageResponseSource.asObservable();

  private connection: signalR.HubConnection | null = null;

  private socket!: WebSocket;

  constructor(
    private authService: AuthService,
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {
    this.connect();
  }

  async connect() {
    const { tokens } = await fetchAuthSession();

    this.socket = new WebSocket(`wss://haxc1oapx9.execute-api.eu-central-1.amazonaws.com/dev/?Authorization=${encodeURIComponent(tokens?.accessToken.toString() as string)}`);

    this.socket.onopen = () => {
      console.log('WebSocket connection established');
    };

    this.socket.onclose = () => {
      console.log('WebSocket connection closed');
    };

    this.socket.onerror = (error: Event) => {
      console.error('WebSocket error:', error);
    };

    this.socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log(event)
      console.log(data)
      if (data.action === 'newMessage') {
        const message = data.body;
        message.date = new Date(message.date);
        this.newMessageSource.next(message);
      }
    }
  }

  sendMessage(chatId: string, text: string) {
    const message = JSON.stringify({
      action: 'sendMessage',
      chatId: chatId,
      text: text
    })
    this.socket.send(message);
  }

  get(): Observable<Chat[]> {
    return this.httpClient.get<Chat[]>('api/aggregator-service/chat');
  }

  getMessages(chatId: string): Observable<Message[]> {
    return this.httpClient.get<Message[]>(`api/chat-service/chat/${chatId}/message`);
  }

  create(jobId: string, proposal: Proposal, message: string): Observable<Chat>
  {
    var request: CreateChatRequest = {
      jobId: jobId,
      proposalId: proposal.id,
      freelancerId: proposal.freelancerId,
      initialMessage: message
    };
    return this.httpClient.post<Chat>('api/chat-service/chat', request);
  }

}
