import { Component } from '@angular/core';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { MedicalAppointment} from '../model/medical-appointment'
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../model/doctor';
//import {  } from 'bootstrap/dist/css/bootstrap.min.css';
import { NgbDateNativeAdapter, NgbDateStruct, NgbDateAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CreateMedicalAppointment } from '../model/create-medical-appointment';




@Component({
  selector: 'app-add-medical-appointment',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule, NgbModule, ],
  templateUrl: './add-medical-appointment.component.html',
  styleUrl: './add-medical-appointment.component.css',
  providers: [NgbDateNativeAdapter]
})
export class AddMedicalAppointmentComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  //doctors: MedicalAppointment[] = [];
  doctors: Doctor[] = [];
  medicalAppointment: MedicalAppointment;
  createdAppointment: CreateMedicalAppointment;
  medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient, private formBuilder: FormBuilder, private adapter: NgbDateNativeAdapter){
    this.medicalAppointmentForm = this.formBuilder.group({});
    this.medicalAppointment = { id: 0, dateTime: new Date(), doctorId: 0, patientId: 0,
                               diagnosis: '', diseaseUnit: 0, interview: '', isFinished: false, isCancelled: false}; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    this.createdAppointment = {dateTime: new Date(), doctorId: 0}; 
    //this.createdAppointment = {doctorId: 0}; 

  }


  ngOnInit(){
    this.getAllDoctors();
    this.medicalAppointmentForm = this.formBuilder.group({
      date: new FormControl(null, {validators: [Validators.required]}),
      time: new FormControl (null, {validators: [Validators.required]}),
      doctorId: new FormControl(null, {validators: [Validators.required]}),
      }); 
  }

  get formDoctorId(): FormControl {return this.medicalAppointmentForm?.get("doctorId") as FormControl};
  get formMedicalAppointmentDate(): FormControl {return this.medicalAppointmentForm.get("date") as FormControl};
  get timeControl(): FormControl {return this.medicalAppointmentForm.get("time") as FormControl};




  addMedicalAppointment() {
    //const date = this.formMedicalAppointmentDate?.value; // Data z kalendarza
    //const time = this.timeControl?.value; // Godzina i minuta

    

    //if (date && time) {
      // Tworzenie obiektu DateTime

      //ODKOMENTOWAC
      /*const dateAndTime = new Date(date.year, date.month - 1, date.day, time.hour, time.minute);
      console.log('Zarejestrowano datę i godzinę:', dateAndTime);
      const formData = this.medicalAppointmentForm.getRawValue();
      this.createdAppointment.dateTime = dateAndTime;
      this.createdAppointment.doctorId = this.formDoctorId.value;*/


    //}

    
    //const formattedDate = new Date(formData.medicalAppointmentDate).toISOString();
    //formData.medicalAppointmentDate = formattedDate;
    //console.log("formatted date:" + formattedDate);
    //console.log("data: " + this.medicalAppointmentForm.getRawValue().dateTime);

    //console.log("ger raw value:" + this.medicalAppointmentForm.getRawValue());
    console.log("ger raw value:" + this.createdAppointment);

    //this.http.post<MedicalAppointment>(this.APIUrl + "/create", this.medicalAppointmentForm.getRawValue())

    //const headers = new HttpHeaders().set('Content-Type', 'application/json');

    //this.http.post<MedicalAppointment>(this.APIUrl + "/create", JSON.stringify(this.createdAppointment), { headers })
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

  /*onDoctorChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const specialisationsArray = this.medicalAppointmentForm.get('DoctorId') as FormControl;
  
    if (checkbox.checked) {
      specialisationsArray.push(new FormControl(+checkbox.value)); // Zamiana na number
    } else {
      const index = specialisationsArray.controls.findIndex(
        ctrl => ctrl.value === +checkbox.value
      );
      if (index !== -1) {
        specialisationsArray.removeAt(index);
      }
    }
  } */

  getAllDoctors(){
    this.http.get<Doctor[]>("https://localhost:5001/api/doctor/Get").subscribe(data =>{
      this.doctors=data;
    })
  }

}
