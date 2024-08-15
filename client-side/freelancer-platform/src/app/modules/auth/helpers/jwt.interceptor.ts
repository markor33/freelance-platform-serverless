import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import {from, Observable, switchMap} from 'rxjs';
import { fetchAuthSession } from "aws-amplify/auth"

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor() { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return from(fetchAuthSession()).pipe(
        switchMap(({ tokens }) => {
          if (tokens?.accessToken) {
            request = request.clone({
              setHeaders: {
                Authorization: `Bearer ${tokens.accessToken}`
              }
            });
          }
          return next.handle(request);
        })
      );
    }
}
