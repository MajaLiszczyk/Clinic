import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../model/doctor';

@Component({
  selector: 'app-get-doctors',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-doctors.component.html',
  styleUrl: './get-doctors.component.css'
})
export class GetDoctorsComponent {
  readonly APIUrl="https://localhost:5001/api/Doctor";
  doctors: Doctor[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient){
  }
  
  ngOnInit(){
    this.getAllDoctors();
  }

  getAllDoctors(){
    this.http.get<Doctor[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.doctors=data;
    })
  }

  edit(doctor: Doctor){
    this.http.put<Doctor>(this.APIUrl+"/update", doctor)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(doctorId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ doctorId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllDoctors();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }


}
