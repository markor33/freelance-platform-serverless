import { Component } from '@angular/core';
import { getCurrentUser } from 'aws-amplify/auth';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent {

  constructor(private authService: AuthService, private router: Router) { }

  async ngOnInit() {
    await this.authService.init();
    this.router.navigate(['/']);
  }
}
