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

  constructor(
    private fb: FormBuilder,
    private authorizationService: AuthorizationService,
    private router: Router
  ) {
    this.logInForm = this.fb.group({
      email: new FormControl('', { validators: [Validators.minLength(1), Validators.maxLength(60), Validators.required] }),
      password: new FormControl('', { validators: [Validators.minLength(1), Validators.maxLength(60), Validators.required] }),
    });
  }

  get formEmail(): FormControl { return this.logInForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formPassword(): FormControl { return this.logInForm?.get("password") as FormControl };



  logIn() {
      //this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
      if (this.logInForm.invalid) {
        this.logInForm.markAllAsTouched();
        return;
      }
      this.authorizationService.logIn(this.logInForm.getRawValue()) // Bez obiektu opakowującego
        .subscribe({
          next: (result) => {
            const {token, role, id } = result;
            this.authorizationService.setToken(token);
            //this.res = result; // Zwrócony obiekt przypisany do zmiennej
            if (role === 'Patient') {
              this.router.navigate(['/patient', id], { queryParams: { isRegistrantMode: false } });
            } else if (role === 'Doctor') {
              this.router.navigate(['/doctor-appointments', id]);
            } else if (role === 'Registrant') {
              this.router.navigate(['/registrant']);
            }
          },
          error: (error) => {
            this.errorMessage = 'Invalid username or password';
          },
        });
    }
}