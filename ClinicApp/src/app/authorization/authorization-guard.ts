import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';


@Injectable({
  providedIn: 'root',
})
export class AuthorizationGuard implements CanActivate {
  constructor(private authorizationService: AuthorizationService, private router: Router) {}

  canActivate(): boolean {
    if (this.authorizationService.isLoggedIn()) {
      return true;
    }
    this.router.navigate(['/login-in']);
    return false;
  }
}