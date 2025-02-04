import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';

import { Component } from '@angular/core';
//import { MakeAnAppointmentComponent } from "../make-an-appointment/make-an-appointment.component";
import { AuthorizationService } from '../services/authorization.service';
//import { GetMedicalAppointmentsForPatientComponent } from "../get-medical-appointments-for-patient/get-medical-appointments-for-patient.component";


@Component({
  selector: 'app-registrant',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink, RouterOutlet],
  templateUrl: './registrant.component.html',
  styleUrl: './registrant.component.css'
})
export class RegistrantComponent {
  registrantId: number = 0;
  constructor(private route: ActivatedRoute, private http:HttpClient, private formBuilder: FormBuilder, public authorizationService: AuthorizationService ){
  }

  ngOnInit(){
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId']; // Przypisanie id z URL
      console.log('Received registrant:', this.registrantId);
    });
  }

  logout(){
    this.authorizationService.logout();
  }

}
