import { Component } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { Notification } from '../models/notification.model';
import { NotificationContent } from '../models/notification-content.model';

@Component({
  selector: 'app-notifications-display',
  templateUrl: './notifications-display.component.html',
  styleUrls: ['./notifications-display.component.scss']
})
export class NotificationsDisplayComponent {

  notifications: Notification[] = [];
  notificationsContent: NotificationContent[] = [];

  constructor(private notificationService: NotificationService) {
    this.notificationService.notificationObserver.subscribe((notifications) => {
      this.notificationsContent = notifications.map((notification) => this.notificationService.getContent(notification)) as NotificationContent[];
      this.notifications = notifications;
    });
  }

  notificationClicked(notification: Notification) {
    this.notificationService.notificationClicked(notification);
    // this.notificationService.notificationChecked(notification).subscribe();
  }

  clear() {
    this.notificationService.delete().subscribe();
  }

}
