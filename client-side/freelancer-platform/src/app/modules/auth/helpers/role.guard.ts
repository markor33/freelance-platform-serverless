import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard {

    constructor(private router: Router, private authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const requiredRole = route.data['roles'] as Array<string>;
        if (requiredRole.includes(this.authService.getUserRole()))
            return true;

        this.router.navigate([''], { queryParams: { returnUrl: state.url } });
        return false;
    }

}