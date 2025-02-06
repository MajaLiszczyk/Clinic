import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLoginRequest } from '../model/user-login-request';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  readonly LogInUrl = "https://localhost:5001/api/authorization";

  constructor(private http: HttpClient, private router: Router) { }

  logIn(loginRequest: UserLoginRequest): Observable<any> {
    return this.http.post(this.LogInUrl, loginRequest);
  }

  logout(): void {
    localStorage.removeItem('authToken'); // Usuń token JWT
    this.router.navigate(['/log-in']);
  }

  setToken(token: string) {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserId(): string | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      console.log(decodedToken.sub);
      console.log(decodedToken.userId);
      return decodedToken.sub || decodedToken.userId || null;
    } catch (error) {
      console.error('Failed to decode token', error);
      return null;
    }
  }
}