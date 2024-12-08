import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Registrant } from '../model/registrant';

@Component({
  selector: 'app-get-registrants',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-registrants.component.html',
  styleUrl: './get-registrants.component.css'
})
export class GetRegistrantsComponent {
  readonly APIUrl="https://localhost:5001/api/Registrant";
  registrants: Registrant[] = [];
  //medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient){
  }
  
  ngOnInit(){
    this.getAllRegistrants();
  }

  getAllRegistrants(){
    this.http.get<Registrant[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.registrants=data;
    })
  }

  edit(registrant: Registrant){
    this.http.put<Registrant>(this.APIUrl+"/update", registrant)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(registrantId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ registrantId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllRegistrants();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

}
