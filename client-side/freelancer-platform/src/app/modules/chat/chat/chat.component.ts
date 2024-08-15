import { Component } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { Chat } from '../models/chat.model';
import { AuthService } from '../../auth/services/auth.service';
import { Message } from '../models/message.model';
import { MatDialog } from '@angular/material/dialog';
import { JobInfoDialogComponent } from '../../job/jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { ProposalInfoDialogComponent } from '../../job/proposals-management/dialogs/proposal-info-dialog/proposal-info-dialog.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent {

  chats: Chat[] = [];
  activeChat: Chat | null = null;
  messages: Message[] = [];
  role: string = '';

  messageText: string = '';

  constructor(
    private chatService: ChatService,
    private authService: AuthService,
    private dialog: MatDialog) {
      this.role = this.authService.getUserRole();
  }

  ngOnInit() {
    this.chatService.get().subscribe((chats) => this.chats = chats);
    this.chatService.newMessageObserver.subscribe((message) => this.newMessageReceived(message));
  }

  openChat(chat: Chat) {
    this.activeChat = chat;
    this.chatService.getMessages(chat.id).subscribe((messages) => this.messages = messages);
  }

  openJobDialog() {
    this.dialog.open(JobInfoDialogComponent, {
      width: '50%',
      height: '80%',
      data: { jobId: this.activeChat?.jobId }
    });
  }

  openProposalDialog() {
    console.log(this.activeChat);
    this.dialog.open(ProposalInfoDialogComponent, {
      width: '50%',
      height: '80%',
      data: { jobId: this.activeChat?.jobId, proposalId: this.activeChat?.proposalId }
    });
  }

  sendMessage() {
    this.chatService.sendMessage(this.activeChat?.id as string, this.messageText);
  }

  newMessageReceived(message: Message | null) {
    if (message === null)
      return;
    if (this.activeChat?.id === message.chatId)
      this.messages.push(message);
  }

  getOppositeParticipant(chat: Chat): any {
    if (this.role == "Employeer")
      return {
        name: chat.freelancerName,
        isActive: chat.isFreelancerActive
      }
    else
      return {
        name: chat.clientName,
        isActive: chat.isClientActive
      }
  }

  getSender(message: Message) {
    if (this.activeChat?.clientId == message.senderId)
      return { name: this.activeChat.clientName}
    else
    return { name: this.activeChat?.freelancerName}
  }

}
