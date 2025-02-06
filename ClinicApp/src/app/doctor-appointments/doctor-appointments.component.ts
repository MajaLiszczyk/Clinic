import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RouterLink, RouterOutlet } from '@angular/router';

import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, FormControl, FormsModule } from '@angular/forms';
import { Specialisation } from '../model/specialisation';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';

import { Input } from '@angular/core';
import { MedicalAppointment } from '../model/medical-appointment';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './doctor-appointments.component.html',
  styleUrl: './doctor-appointments.component.css'
})
export class DoctorAppointmentsComponent {
  doctorId: number = 0;
  allMedicalAppointments: AllMedicalAppointments;
  isRegistrantMode: boolean = false;
  registrantId: number = 0;

  constructor(private http: HttpClient, private route: ActivatedRoute, private clinicService: ClinicService
    , public authorizationService: AuthorizationService) {
    this.allMedicalAppointments = { pastMedicalAppointments: [], futureMedicalAppointments: [] }
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.doctorId = +params['doctorId'];
      this.registrantId = +params['registrantId'];
      console.log('Received doctorId:', this.doctorId);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isRegistrantMode = queryParams['isRegistrantMode'] === 'true';
      console.log('Is registrant mode po params:', this.isRegistrantMode);
    });
    this.getMedicalAppointmentsForDoctor()
  }

  getMedicalAppointmentsForDoctor() {
    this.clinicService.getMedicalAppointmentsForDoctor(this.doctorId).subscribe(data => {
      this.allMedicalAppointments = data;
    })
  }

  logout() {
    this.authorizationService.logout();
  }
}