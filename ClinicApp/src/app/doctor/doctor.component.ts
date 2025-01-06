/*import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Doctor } from '../model/doctor';
import { FormBuilder, FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ClinicService } from '../services/clinic.service';

@Component({
  selector: 'app-doctor',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, HttpClientModule, CommonModule],
  templateUrl: './doctor.component.html',
  styleUrl: './doctor.component.css'
})
export class DoctorComponent {
  chooseDoctorForm: FormGroup;
  doctorId = 0;
  doctors: Doctor[]= [];
  isDoctorChosen: boolean = false;

  constructor(private http:HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService){
    this.chooseDoctorForm = this.formBuilder.group({});
  }

  ngOnInit(){
    this.getAllDoctors();
    this.chooseDoctorForm = this.formBuilder.group({
      doctorId: new FormControl(null, {validators: [Validators.required]})
    }); 
  }

  get formDoctortId(): FormControl {return this.chooseDoctorForm?.get("doctorId") as FormControl};

  getAllDoctors(){
    //this.http.get<Doctor[]>("https://localhost:5001/api/doctor/Get").subscribe(data =>{
    this.clinicService.getAllDoctors().subscribe(data =>{
      this.doctors=data;
    })
  }

  onDoctorChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.doctorId = parseInt(selectElement.value, 10); // Pobranie wybranego ID pacjenta
    console.log('Selected Patient ID:', this.doctorId);
    this.isDoctorChosen = true;
  }


} */
