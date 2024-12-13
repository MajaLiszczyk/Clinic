import { Component } from '@angular/core';
import { GetPatientsComponent } from "../get-patients/get-patients.component";
import { AddPatientComponent } from "../add-patient/add-patient.component";
//import { ActivatedRoute } from '@angular/router';
import { RouterLink, RouterOutlet } from '@angular/router';

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MedicalAppointment} from '../model/medical-appointment'
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../model/doctor';


@Component({
  selector: 'app-registrant-patients',
  standalone: true,
  imports: [GetPatientsComponent, AddPatientComponent, HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './registrant-patients.component.html',
  styleUrl: './registrant-patients.component.css'
})
export class RegistrantPatientsComponent {
  isAddNewPatientVisible: boolean = false;



}
