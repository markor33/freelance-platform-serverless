import { Injectable } from "@angular/core";
import { NotificationContent } from "../../models/notification-content.model";
import { NotificationHandler } from "../notification-handler";
import { InterviewStageStartedNotification } from "./interview-stage-started-notification.model";

@Injectable({
    providedIn: 'root'
})
export class InterviewStageStartedNotificationHandler implements NotificationHandler {

    getType(): string {
        return InterviewStageStartedNotification.name;
    }

    getContent(data: InterviewStageStartedNotification): NotificationContent {
        return {
            title: 'Interview stage started',
            description: `Client is interested for your application on '${data.JobTitle}' job. Check your inbox.`
        }
    }

    handle(data: InterviewStageStartedNotification): void {

    }

}