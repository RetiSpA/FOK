import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

import { User } from '../models/user';
import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly key = "FOK-DATA";

  constructor(
    private http: HttpClient,
    private router: Router,
    private configSvc: ConfigurationService
  ) {
  }

  get logged(): boolean {
    let user = JSON.parse(sessionStorage.getItem(this.key)) as User;
    return !!user;
  }

  get user(): User {
    return JSON.parse(sessionStorage.getItem(this.key)) as User;
  }

  private getApiUrl(action: string) {
    return `${this.configSvc.config['gatewayApiClient']}/api/user/${action}`;
  }

  login(username: string, password: string) {
    return this.http.post(this.getApiUrl("authenticate"), { username, password })
      .pipe(map((resp: User) => {
        sessionStorage.setItem(this.key, JSON.stringify(resp));
        return resp;
      }),
        catchError(() => {
          return of(null);
        }));
  }

  logout() {
    sessionStorage.removeItem(this.key);
    this.router.navigate(["/"]);
  }
}
