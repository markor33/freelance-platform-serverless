import { Inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { NOTIFICATION_HANDLER_TOKEN, NotificationHandler } from '../handlers/notification-handler';
import { Notification } from '../models/notification.model';
import { AuthService } from '../../auth/services/auth.service';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notificationsSource =  new BehaviorSubject<Notification[]>([]);
  public notificationObserver = this.notificationsSource.asObservable();

  private newNotificationReceivedSource = new BehaviorSubject<boolean>(false);
  public newNotificationReceivedObserver = this.newNotificationReceivedSource.asObservable();

  private handlersMap: Map<string, NotificationHandler> = new Map<string, NotificationHandler>();

  private connection: signalR.HubConnection | null = null;

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private jwtHelper: JwtHelperService,
    @Inject(NOTIFICATION_HANDLER_TOKEN) private notificationHandlers: NotificationHandler[]) {

      this.authService.userObserver.subscribe((user) => {
        if (user === null) {
          this.notificationsSource.next([]);
          this.connection?.stop();
          return;
        }
        // this.configureConnection(user.domainId);
      });

      for (let notificationHandler of this.notificationHandlers)
        this.handlersMap.set(notificationHandler.getType(), notificationHandler);
  }

  getContent(notification: Notification) {
    return this.handlersMap.get(notification.type)?.getContent(notification.data);
  }

  notificationClicked(notification: Notification) {
    this.handlersMap.get(notification.type)?.handle(notification.data);
    this.notificationChecked(notification).subscribe();
  }

  notificationChecked(notification: Notification): Observable<any> {
    return this.httpClient.put<any>(`api/notifychat-service/notification/${notification.id}`, {})
      .pipe(
        map(() => {
          notification.isChecked = true;
        })
      );
  }

  delete(): Observable<any> {
    let ids: string[] = this.notificationsSource.value.map((notification) => notification.id);
    return this.httpClient.put<any>(`api/notifychat-service/notification`, ids)
      .pipe(
        map(() => {
          this.notificationsSource.next([]);
        })
      );
  }

  private configureConnection(userId: string) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`hub/notifications`, { accessTokenFactory: () => this.jwtHelper.tokenGetter()})
      .build();

    this.connection.start()
    .then(() => console.log('Notf OK'))
    .catch((err) => console.log(err));
    
    this.connection.on('newNotification', (notification: Notification) => {
    this.notificationsSource.value.push(notification);
    this.newNotificationReceivedSource.next(true);
    });

    this.connection.on('getNotifications', (notifications: Notification[]) => {
      this.notificationsSource.next(notifications);
    });

  }

}
