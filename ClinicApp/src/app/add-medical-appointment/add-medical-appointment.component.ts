import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MedicalAppointment} from '../model/medical-appointment'
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../model/doctor';

@Component({
  selector: 'app-add-medical-appointment',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './add-medical-appointment.component.html',
  styleUrl: './add-medical-appointment.component.css'
})
export class AddMedicalAppointmentComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  //doctors: MedicalAppointment[] = [];
  doctors: Doctor[] = [];
  medicalAppointment: MedicalAppointment;
  medicalAppointmentForm: FormGroup;
  isDisable = false;

  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.medicalAppointmentForm = this.formBuilder.group({});
    this.medicalAppointment = { id: 0, dateTime: new Date().toISOString(), doctorId: 0, patientId: 0,
                               diagnosis: '', diseaseUnit: 0, interview: ''}; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."

  }


  ngOnInit(){
    this.getAllDoctors();
    this.medicalAppointmentForm = this.formBuilder.group({
      medicalAppointmentDate: new FormControl(null, {validators: [Validators.required]}),
      doctorId: new FormControl(null, {validators: [Validators.required]}),
      id: Number,
      patientId: new FormControl(0, {validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required]})
    }); 
  }

  get formId(): FormControl {return this.medicalAppointmentForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formPatientId(): FormControl {return this.medicalAppointmentForm?.get("patientId") as FormControl};
  get formDoctorId(): FormControl {return this.medicalAppointmentForm?.get("doctorId") as FormControl};
  get formMedicalAppointmentDate(): FormControl {return this.medicalAppointmentForm.get("medicalAppointmentDate") as FormControl};

  addMedicalAppointment() {
    const formData = this.medicalAppointmentForm.getRawValue();
    const formattedDate = new Date(formData.medicalAppointmentDate).toISOString();
    formData.medicalAppointmentDate = formattedDate;
    console.log("formatted date:" + formattedDate);
    

    //this.http.post<MedicalAppointment>(this.APIUrl + "/create", this.medicalAppointmentForm.getRawValue())


    this.http.post<MedicalAppointment>(this.APIUrl + "/create", formData)
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
