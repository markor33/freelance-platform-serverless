import { Injectable } from "@angular/core";
import { NotificationContent } from "../../models/notification-content.model";
import { NotificationHandler } from "../notification-handler";
import { ContractMadeNotification } from "./contract-made-notification.model";
import { MatDialog } from "@angular/material/dialog";
import { ProposalInfoDialogComponent } from "src/app/modules/job/proposals-management/dialogs/proposal-info-dialog/proposal-info-dialog.component";

@Injectable({
    providedIn: 'root'
})
export class ContractMadeNotificationHandler implements NotificationHandler {

    constructor(private dialog: MatDialog) { }

    getType(): string {
        return ContractMadeNotification.name;
    }

    getContent(data: ContractMadeNotification): NotificationContent {
        return {
            title: 'New contract',
            description: `Freelancer has accepted your offer for '${data.JobTitle}' job`
        }
    }

    handle(data: ContractMadeNotification): void {
        this.dialog.open(ProposalInfoDialogComponent, {
            width: '50%',
            height: '80%',
            data: { jobId: data.JobId, proposalId: data.ProposalId }
        });
    }

}