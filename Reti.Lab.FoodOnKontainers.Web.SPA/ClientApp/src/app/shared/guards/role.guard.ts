import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';

import { UserService } from '../services/user.service';

@Injectable()
export class RoleGuard implements CanActivate {

    constructor(
        private svc: UserService
    ) { }

    canActivate(route: ActivatedRouteSnapshot) {
        let roles = route.data.roles as Array<string>;
        if (this.svc.user && roles.indexOf(this.svc.user.role) > -1) {
            return true;
        }
        return false;
    }
}