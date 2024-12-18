import { Component } from '@angular/core';
import { ClinicService } from '../services/clinic.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Patient } from '../model/patient';

@Component({
  selector: 'app-your-data',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './your-data.component.html',
  styleUrl: './your-data.component.css',
  //providers: [ClinicService] //bedzie trzeba
})
export class YourDataComponent {
  readonly APIUrl="https://localhost:5001/api/patient";
  patientId: number = 0;
  pesel: string = 'pesel';
  name: string = 'imie';
  surname: string = 'nazwisko';
  isDisable = false;
  patient: Patient = { id: 0, pesel: '', name: '', surname: '', patientNumber: '' };


  constructor(private service: ClinicService, private http:HttpClient){
  }

  setPatientId(event: Event){
    const inputElement  = event.target as HTMLInputElement; 
    inputElement
    this.patientId = +inputElement.value;
  }

  getPatient(){
    //this.patientId = id;
    this.http.get<Patient>(this.APIUrl+"/get/" + this.patientId).subscribe((result: Patient) =>{
      this.patient = result
    })
   this.isDisable = true;
  }

  /*getPatient(id: number){
    this.patientId = id;
    this.http.get<Patient>(this.APIUrl+"/get/" + id).subscribe((result: Patient) =>{
      this.patient = result
    })
   this.isDisable = true;
  } */

}

