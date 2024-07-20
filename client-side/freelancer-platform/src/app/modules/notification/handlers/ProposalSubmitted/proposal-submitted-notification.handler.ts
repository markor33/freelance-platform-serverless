import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ProposalSubmittedNotification } from "./proposal-submitted-notification.model";
import { NotificationHandler } from "../notification-handler";
import { NotificationContent } from "../../models/notification-content.model";

@Injectable({
    providedIn: 'root'
})
export class ProposalSubmittedNotificationHandler implements NotificationHandler {

    constructor(
        private router: Router) {
    }

    getType(): string {
        return ProposalSubmittedNotification.name;
    }

    getContent(data: ProposalSubmittedNotification): NotificationContent {
        return {
            title: 'New proposal submitted',
            description: `Your job '${data.JobName}' has a new proposal`
        }
    }

    handle(data: ProposalSubmittedNotification): void {
        this.router.navigate(['/job', data.JobId, 'proposal-management']);
    }

}
