import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Patient } from '../model/patient';
import { RouterLink, RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-get-patients',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './get-patients.component.html',
  styleUrl: './get-patients.component.css'
})
export class GetPatientsComponent {
  readonly APIUrl="https://localhost:5001/api/Patient";
  patients: Patient[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient){
  }
  
  ngOnInit(){
    this.getAllPatients();
  }

  getAllPatients(){
    this.http.get<Patient[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.patients=data;
    })
  }

  edit(patient: Patient){
    this.http.put<Patient>(this.APIUrl+"/update", patient)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(patientId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+patientId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllPatients();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }


}
