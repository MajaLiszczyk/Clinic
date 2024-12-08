import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';

@Component({
  selector: 'app-get-specialisations',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-specialisations.component.html',
  styleUrl: './get-specialisations.component.css'
})
export class GetSpecialisationsComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalSpecialisation";
  specialisations: Specialisation[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient){
  }
  
  ngOnInit(){
    this.getAllSpecialisations();
  }

  getAllSpecialisations(){
    this.http.get<Specialisation[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.specialisations=data;
    })

  }

  edit(specialisation: Specialisation){
    this.http.put<Specialisation>(this.APIUrl+"/update", specialisation)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(dspecialisationId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ dspecialisationId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllSpecialisations();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

}
