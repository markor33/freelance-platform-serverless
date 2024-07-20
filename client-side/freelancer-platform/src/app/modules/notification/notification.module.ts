import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NOTIFICATION_HANDLER_TOKEN } from './handlers/notification-handler';
import { ProposalSubmittedNotificationHandler } from './handlers/ProposalSubmitted/proposal-submitted-notification.handler';
import { NotificationsDisplayComponent } from './notifications-display/notifications-display.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { InterviewStageStartedNotificationHandler } from './handlers/InterviewStageStarted/interview-stage-started-notification.handler';
import { ProposalPaymentChangedNotificationHandler } from './handlers/ProposalPaymentChanged/proposal-payment-changed-notification.handler';
import { ProposalApprovedNotificationHandler } from './handlers/ProposalApproved/proposal-approved-notification.handler';
import { ContractMadeNotificationHandler } from './handlers/ContractMade/contract-made-notification.handler';
import { ContractFinishedNotificationHandler } from './handlers/ContractFinished/contract-finished-notification.handler';
import { FeedbackSubmittedNotificationHandler } from './handlers/FeedbackSubmitted/feedback-submitted-notification.handler';

@NgModule({
  declarations: [
    NotificationsDisplayComponent
  ],
  imports: [
    CommonModule,
    MatDividerModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule
  ],
  exports: [
    NotificationsDisplayComponent
  ],
  providers: [
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: ProposalSubmittedNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: InterviewStageStartedNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: ProposalPaymentChangedNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: ProposalApprovedNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: ContractMadeNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: ContractFinishedNotificationHandler, multi: true },
    { provide: NOTIFICATION_HANDLER_TOKEN, useClass: FeedbackSubmittedNotificationHandler, multi: true }
  ]
})
export class NotificationModule { }
