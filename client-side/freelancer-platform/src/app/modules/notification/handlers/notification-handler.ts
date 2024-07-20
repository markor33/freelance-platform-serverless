import { InjectionToken } from "@angular/core";
import { NotificationContent } from "../models/notification-content.model";

export interface NotificationHandler {
    getType(): string;
    getContent(data: any): NotificationContent;
    handle(data: any): void;
}

export const NOTIFICATION_HANDLER_TOKEN = new InjectionToken<NotificationHandler>('NotificationHandler');
  