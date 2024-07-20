import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompleteRegisterDialogComponent } from '../../freelancer/profile-setup-dialog/profile-setup-dialog.component';
import { AuthService } from '../../auth/services/auth.service';
import { NotificationService } from '../../notification/services/notification.service';
import { ChatService } from '../../chat/services/chat.service';
import { Router } from '@angular/router';
import { FreelancerService } from '../../freelancer/services/freelancer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  showNotifications: boolean = false;
  showNewNotificationDot: boolean = false;
  showNewChatMessageDot: boolean = false;
  isUserLogged: boolean = false;
  userRole: string = '';
  userDomainId: string | undefined;

  constructor(
    private dialog: MatDialog,
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService,
    private chatService: ChatService,
    private freelancerService: FreelancerService,) {
      this.authService.userObserver.subscribe({
        next: (user) => {
          this.userDomainId = user?.userId;
          this.isUserLogged = this.authService.isLogged();
          this.userRole = this.authService.getUserRole();
        }
      });

      this.notificationService.newNotificationReceivedObserver.subscribe((res) => this.showNewNotificationDot = res);

      this.chatService.newMessageObserver.subscribe((res) => {
        if (res === null) this.showNewChatMessageDot = false;
        else this.showNewChatMessageDot = true;
      });

      this.freelancerService.profileSetupCompletedObserver.subscribe((isCompleted) => {
        if (isCompleted === false)
          CompleteRegisterDialogComponent.open(this.dialog);
      });
  }

  async ngOnInit() {
    if (this.userRole === 'Freelancer')
      this.freelancerService.get(this.userDomainId as string).subscribe();
    this.route();
  }

  route() {
    if (this.userRole === 'Freelancer')
      this.router.navigate(['job']);
    else
      this.router.navigate(['job-management']);
  }

  navigateToProfile() {
    this.authService.userObserver.subscribe((user) => {
      this.router.navigate([`/freelancer/profile/${user?.userId}`]);
    });
  }

  openNotificationsDisplay() {
    this.showNotifications = !this.showNotifications;
    this.showNewNotificationDot = false;
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }

}
