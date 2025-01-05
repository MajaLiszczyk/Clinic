import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})

export class LoginComponent {
  logInForm: FormGroup;
  errorMessage: string | null = null;
  res:  string = "";
  passwordVisible = false;



  constructor(
    private fb: FormBuilder,
    private authorizationService: AuthorizationService,
    private router: Router
  ) {
    this.logInForm = this.fb.group({
      email: new FormControl(null, { validators: [Validators.required, Validators.email, Validators.maxLength(256)] }),
      password: new FormControl(null, { validators: [Validators.required, Validators.minLength(6), Validators.maxLength(100)] }),
    });
  }

  get formEmail(): FormControl { return this.logInForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formPassword(): FormControl { return this.logInForm?.get("password") as FormControl };

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  logIn() {
      //this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
      if (this.logInForm.invalid) {
        this.logInForm.markAllAsTouched();
        return;
      }
      this.authorizationService.logIn(this.logInForm.getRawValue()) // Bez obiektu opakowującego
        .subscribe({
          next: (result) => {
            console.log('Token:', result.token);
            console.log('Role:', result.role);
            console.log('UserId:', result.userId);
            console.log('Id:', result.id);
            const {token, role, id, userId } = result;
            this.authorizationService.setToken(result.token);
            //this.res = result; // Zwrócony obiekt przypisany do zmiennej
            if (result.role === 'Patient') {
              this.router.navigate(['/patient', id], { queryParams: { isRegistrantMode: false } });
            } else if (result.role === 'Doctor') {
              this.router.navigate(['/doctor-appointments', id]);
            } else if (result.role === 'Registrant') {
              this.router.navigate(['/registrant']);
            }
          },
          error: (error) => {
            this.errorMessage = 'Invalid username or password';
            console.error('Login failed:', error);
            alert('Login failed: Invalid email or password');
          },
        });
    }
}