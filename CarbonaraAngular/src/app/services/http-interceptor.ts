import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';

import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';

/** Pass untouched request through to the next request handler. */
@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor (
        private authenticationService: AuthenticationService
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (this.authenticationService.isLoggedIn())
        {
            req = req.clone({
                setHeaders: {
                    Authorization: "Bearer " + this.authenticationService.getToken()
                }
            });
        }

        return next.handle(req);
    }
}