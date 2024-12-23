import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AddMedicalAppointmentComponent } from '../add-medical-appointment/add-medical-appointment.component';
import { GetMedicalAppointmentsComponent } from '../get-medical-appointments/get-medical-appointments.component';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
//import { NgbDateNativeAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateNativeAdapter, NgbDateStruct, NgbDateAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CreateMedicalAppointment } from '../model/create-medical-appointment';
import { Doctor } from '../model/doctor';
import { MedicalAppointment } from '../model/medical-appointment';

@Component({
  selector: 'app-registrant-appointments',
  standalone: true,
  imports: [RouterLink, CommonModule, HttpClientModule, ReactiveFormsModule, NgbModule],
  templateUrl: './registrant-appointments.component.html',
  styleUrl: './registrant-appointments.component.css',
  providers: [NgbDateNativeAdapter]
})
export class RegistrantAppointmentsComponent {
  isAddNewAppointmentVisible: boolean = false;
  isGetAllAppointmentsVisible: boolean = false;

  readonly APIUrl = "https://localhost:5001/api/MedicalAppointment";
  //doctors: MedicalAppointment[] = [];
  doctors: Doctor[] = [];
  medicalAppointment: MedicalAppointment;
  createdAppointment: CreateMedicalAppointment;
  medicalAppointmentForm: FormGroup;
  isDisable = false;
  medicalAppointments: MedicalAppointment[] = [];
  isAddingMode: boolean = false;
  isShowingAllAppointmentsMode: boolean = false;
  //medicalAppointmentForm: FormGroup;
  //isDisable = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private adapter: NgbDateNativeAdapter) {
    this.medicalAppointmentForm = this.formBuilder.group({});
    this.medicalAppointment = {
      id: 0, dateTime: new Date(), doctorId: 0, patientId: 0,
      diagnosis: '', interview: '', isFinished: false, isCancelled: false, cancellingComment: ''
    }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    this.createdAppointment = { dateTime: new Date(), doctorId: 0 };
  }

  ngOnInit() {
    this.getAllDoctors();
    this.getAllMedicalAppointments();
    this.medicalAppointmentForm = this.formBuilder.group({
      date: new FormControl(null, { validators: [Validators.required] }),
      time: new FormControl(null, { validators: [Validators.required] }),
      doctorId: new FormControl(null, { validators: [Validators.required] }),
    });
  }

  get formDoctorId(): FormControl { return this.medicalAppointmentForm?.get("doctorId") as FormControl };
  get formMedicalAppointmentDate(): FormControl { return this.medicalAppointmentForm.get("date") as FormControl };
  get timeControl(): FormControl { return this.medicalAppointmentForm.get("time") as FormControl };

  getAllMedicalAppointments(){
    this.http.get<MedicalAppointment[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.medicalAppointments=data;
    })
  }

  addNewAppointment() {
    this.isAddNewAppointmentVisible = true;
  }
  getAllAppointments() {
    this.isGetAllAppointmentsVisible = true;
  }

  openAppointmentForm(){
    this.isAddingMode = true;
  }

  addMedicalAppointment() {   
    console.log("ger raw value:" + this.createdAppointment);
    this.http.post<MedicalAppointment>(this.APIUrl + "/create", this.medicalAppointmentForm.getRawValue())
      .subscribe({
        next: (result: MedicalAppointment) => {
          this.medicalAppointment = result;
        },
        error: (err) => {
          console.error("Error occurred:", err);
        }
      });
  }

  cancelAdding(){
    this.isAddingMode = false;
  }

  showAllAppointments(){
    this.isShowingAllAppointmentsMode = true;
  }

  closeAllAppointments(){
    this.isShowingAllAppointmentsMode = false;
  }

  getAllDoctors(){
    this.http.get<Doctor[]>("https://localhost:5001/api/doctor/Get").subscribe(data =>{
      this.doctors=data;
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
