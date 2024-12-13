import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RouterLink, RouterOutlet } from '@angular/router';

import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, FormsModule } from '@angular/forms';
import { Specialisation } from '../model/specialisation';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';

import { Input } from '@angular/core';
import { MakeAnAppointmentComponent } from "../make-an-appointment/make-an-appointment.component";
import { GetMedicalAppointmentsForPatientComponent } from "../get-medical-appointments-for-patient/get-medical-appointments-for-patient.component";
import { MedicalAppointment } from '../model/medical-appointment';
import { AllMedicalAppointments } from '../model/all-medical-appointments';



@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './doctor-appointments.component.html',
  styleUrl: './doctor-appointments.component.css'
})
export class DoctorAppointmentsComponent {
  doctorId: number = 0;
  allMedicalAppointments: AllMedicalAppointments;
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";

  constructor(private http:HttpClient, private route: ActivatedRoute){
    this.allMedicalAppointments = {pastMedicalAppointments: [], futureMedicalAppointments: []}

  }
  

  ngOnInit(){
    this.route.params.subscribe(params => {
      this.doctorId = +params['doctorId']; // Przypisanie id z URL
      console.log('Received doctorId:', this.doctorId);
    });
    this.getMedicalAppointmentsForDoctor()
  }

  getMedicalAppointmentsForDoctor(){
    this.http.get<AllMedicalAppointments>(this.APIUrl+"/GetByDoctorId/" + this.doctorId).subscribe(data =>{
      this.allMedicalAppointments=data;
      //this.allMedicalAppointments.pastMedicalAppointments[0].dateTime = new Date(2024, 12, 24);
    })
  }

}
