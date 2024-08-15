import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './modules/auth/auth.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FreelancerModule } from './modules/freelancer/freelancer.module';
import { SharedModule } from './modules/shared/shared.module';
import { LayoutModule } from './modules/layout/layout.module';
import { JsonDateInterceptor } from './modules/shared/utils/json-date-interceptor';
import { JobModule } from './modules/job/job.module';
import { NotificationModule } from './modules/notification/notification.module';
import { ContractModule } from './modules/contract/contract.module';
import { FeedbackModule } from './modules/feedback/feedback.module';

import { Amplify } from "aws-amplify"
import { AuthService } from './modules/auth/services/auth.service';
import {JwtModule} from "@auth0/angular-jwt";
import {JwtInterceptor} from "./modules/auth/helpers/jwt.interceptor";
Amplify.configure({
    Auth: {
      Cognito: {
        userPoolId: "eu-central-1_yP2OhxI3R",
        userPoolClientId: "423ebmefbtlmsquaubaok92dtb",
        loginWith: {
          oauth: {
            domain: 'freelance-platform.auth.eu-central-1.amazoncognito.com',
            scopes: ['email', 'openid', 'FRS/rw'],
            redirectSignIn: ['http://localhost:4200/auth-callback'],
            redirectSignOut: ['http://localhost:4200/'],
            responseType: 'code',
          }
        }
      },
    },
  })

export function initializeApp(authService: AuthService) {
  return async () => {
    await authService.init();
  };
}

@NgModule({
    declarations: [
        AppComponent,
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        SharedModule,
        AuthModule,
        FreelancerModule,
        JobModule,
        ContractModule,
        NotificationModule,
        FeedbackModule,
        LayoutModule,
        JwtModule.forRoot({
            config: {
              tokenGetter: () => JSON.parse(localStorage.getItem('jwt') as string)
            }
        })
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
        { provide: HTTP_INTERCEPTORS, useClass: JsonDateInterceptor, multi: true},
        AuthService,
        {
          provide: APP_INITIALIZER,
          useFactory: initializeApp,
          deps: [AuthService],
          multi: true
        }
    ],
})
export class AppModule { }
