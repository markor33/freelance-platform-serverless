import { Injectable } from "@angular/core";
import { NotificationHandler } from "../notification-handler";
import { NotificationContent } from "../../models/notification-content.model";
import { MatDialog } from "@angular/material/dialog";
import { ProposalPaymentChangedNotification } from "./proposal-payment-changed-notification.model";
import { ProposalInfoDialogComponent } from "src/app/modules/job/proposals-management/dialogs/proposal-info-dialog/proposal-info-dialog.component";

@Injectable({
    providedIn: 'root'
})
export class ProposalPaymentChangedNotificationHandler implements NotificationHandler {

    constructor(private dialog: MatDialog) { }

    getType(): string {
        return ProposalPaymentChangedNotification.name;
    }

    getContent(data: ProposalPaymentChangedNotification): NotificationContent {
        return {
            title: 'Proposals payment changed',
            description: `Payment has been changed for '${data.JobTitle}' job you applied earlier`
        }
    }

    handle(data: ProposalPaymentChangedNotification): void {
        this.dialog.open(ProposalInfoDialogComponent, {
            width: '50%',
            height: '80%',
            data: { jobId: data.JobId, proposalId: data.ProposalId }
        });
    }
    
}