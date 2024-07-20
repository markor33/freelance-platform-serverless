import { Injectable } from "@angular/core";
import { NotificationContent } from "../../models/notification-content.model";
import { NotificationHandler } from "../notification-handler";
import { FeedbackSubmittedNotification } from "./feedback-submitted-notification.model";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
export class FeedbackSubmittedNotificationHandler implements NotificationHandler {

    constructor(private route: Router) { }

    getType(): string {
        return FeedbackSubmittedNotification.name;
    }

    getContent(data: FeedbackSubmittedNotification): NotificationContent {
        return {
            title: 'You have got new feedback',
            description: `Click to see more details`
        }
    }

    handle(data: FeedbackSubmittedNotification): void {
        this.route.navigate(['/my-contracts']);
    }

}