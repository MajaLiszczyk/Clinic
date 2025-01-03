import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLoginRequest } from '../model/user-login-request';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  readonly LogInUrl = "https://localhost:5001/api/authorization";


  constructor(private http: HttpClient) { }

  logIn(loginRequest: UserLoginRequest): Observable<any> {
    return this.http.post(this.LogInUrl, loginRequest);
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
      return decodedToken.sub || decodedToken.userId || null; // 'sub' to standardowe pole w JWT
    } catch (error) {
      console.error('Failed to decode token', error);
      return null;
    }
  }
  
}
