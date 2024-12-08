import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MedicalAppointment } from '../model/medical-appointment';

@Component({
  selector: 'app-get-medical-appointments',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-medical-appointments.component.html',
  styleUrl: './get-medical-appointments.component.css'
})
export class GetMedicalAppointmentsComponent {


  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  medicalAppointments: MedicalAppointment[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient){
  }
  
  ngOnInit(){
    this.getAllMedicalAppointments();
  }

  getAllMedicalAppointments(){
    this.http.get<MedicalAppointment[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.medicalAppointments=data;
    })
  }

  edit(medicalAppointment: MedicalAppointment){
    this.http.put<MedicalAppointment>(this.APIUrl+"/update", medicalAppointment)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(medicalAppointmentId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ medicalAppointmentId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllMedicalAppointments();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }
}
