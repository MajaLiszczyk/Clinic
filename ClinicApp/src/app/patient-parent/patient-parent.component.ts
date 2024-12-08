import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';

import { Component } from '@angular/core';
import { MakeAnAppointmentComponent } from "../make-an-appointment/make-an-appointment.component";
import { GetMedicalAppointmentsForPatientComponent } from "../get-medical-appointments-for-patient/get-medical-appointments-for-patient.component";


@Component({
  selector: 'app-patient-parent',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink, RouterOutlet],
  templateUrl: './patient-parent.component.html',
  styleUrl: './patient-parent.component.css'
})
export class PatientParentComponent {
  patientId = 0;
  choosePatientForm: FormGroup;
  patients: Patient[]= [];
  isPatientChosen: boolean = false;
  
  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.choosePatientForm = this.formBuilder.group({});
  }

  ngOnInit(){
    this.getAllPatients();
    this.choosePatientForm = this.formBuilder.group({
      patientId: new FormControl(null, {validators: [Validators.required]})
    }); 
  }

  get formPatientId(): FormControl {return this.choosePatientForm?.get("patientId") as FormControl};

  getAllPatients(){
    this.http.get<Patient[]>("https://localhost:5001/api/patient/Get").subscribe(data =>{
      this.patients=data;
    })
  }

  onPatientChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.patientId = parseInt(selectElement.value, 10); // Pobranie wybranego ID pacjenta
    console.log('Selected Patient ID:', this.patientId);
    this.isPatientChosen = true;
  }

}
