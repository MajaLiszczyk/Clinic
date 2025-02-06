import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthorizationService } from '../services/authorization.service';
import { passwordValidator } from '../validators';

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
  res: string = "";
  passwordVisible = false;

  constructor(
    private fb: FormBuilder,
    private authorizationService: AuthorizationService,
    private router: Router
  ) {
    this.logInForm = this.fb.group({
      email: new FormControl(null, { validators: [Validators.required, Validators.email] }),
      password: new FormControl(null, { validators: [Validators.required] }),
    });
  }

  get formEmail(): FormControl { return this.logInForm?.get("email") as FormControl };
  get formPassword(): FormControl { return this.logInForm?.get("password") as FormControl };

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  logIn() {
    if (this.logInForm.invalid) {
      this.logInForm.markAllAsTouched();
      return;
    }
    this.authorizationService.logIn(this.logInForm.getRawValue())
      .subscribe({
        next: (result) => {
          console.log('Token:', result.token);
          console.log('Role:', result.role);
          console.log('UserId:', result.userId);
          console.log('Id:', result.id);
          const { token, role, id, userId } = result;
          this.authorizationService.setToken(result.token);
          if (result.role === 'Patient') {
            this.router.navigate(['/patient-menu', id], { queryParams: { isRegistrantMode: false } });
          } else if (result.role === 'Doctor') {
            this.router.navigate(['/doctor-appointments', id, 0]);
          } else if (result.role === 'Registrant') {
            this.router.navigate(['/registrant', id]);
          } else if (result.role === 'LaboratoryWorker') {
            this.router.navigate(['/laboratory-worker', id, 0]);
          } else if (result.role === 'LaboratorySupervisor') {
            this.router.navigate(['/laboratory-supervisor', id, 0]);
          } else if (result.role === 'Admin') {
            this.router.navigate(['/admin']);
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