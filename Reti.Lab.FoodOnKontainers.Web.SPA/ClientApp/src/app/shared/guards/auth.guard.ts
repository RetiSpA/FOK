import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { UserService } from '../services/user.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(
        private svc: UserService,
        private router: Router
    ) { }

    canActivate() {        
        if (this.svc.logged) {
            return true;
        }
        this.router.navigate(["/"]);
        return false;
    }
}