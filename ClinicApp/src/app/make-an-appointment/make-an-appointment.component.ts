import { Component, Input } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MedicalAppointment} from '../model/medical-appointment'
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';

@Component({
  selector: 'app-make-an-appointment',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './make-an-appointment.component.html',
  styleUrl: './make-an-appointment.component.css'
})
export class MakeAnAppointmentComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  medicalAppointments: ReturnMedicalAppointment[] = [];
  selectedAppointment: ReturnMedicalAppointment;
  specialisations: Specialisation[] = [];
  //patients: Patient[]= [];
  chooseSpecialisationForm: FormGroup;
  //choosePatientForm: FormGroup;
  isDisabled = true;
  selectedSpecialisation: number;

  @Input()
  patientId: number = 0;

  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.chooseSpecialisationForm = this.formBuilder.group({});
    //this.choosePatientForm = this.formBuilder.group({});
    this.selectedSpecialisation = 0;
    this.selectedAppointment = { id: 0, doctorId: 0, patientId: 0, interview: '', diagnosis: '', diseaseUnit: 0, dateTime: new Date()}; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }

  /*ngOnInit(){
    this.getAllSpecialisations();
    //this.getAllPatients();
    this.chooseSpecialisationForm = this.formBuilder.group({
      specialisationId: new FormControl(null, {validators: [Validators.required]})
    });
    //this.choosePatientForm = this.formBuilder.group({
    //  patientId: new FormControl(null, {validators: [Validators.required]})
    //}); 
  } */

  ngOnInit(): void {
    this.getAllSpecialisations();
    this.chooseSpecialisationForm = this.formBuilder.group({
      //specialisationId: [null, Validators.required]
      specialisationId: new FormControl(null, {validators: [Validators.required]})
    });

    this.chooseSpecialisationForm.get('specialisationId')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Specialisation selected:', value, 'isDisabled:', this.isDisabled);
    });
    console.log('pacjent id: ', this.patientId);
  }





  get formSpecialisationId(): FormControl {return this.chooseSpecialisationForm?.get("specialisationId") as FormControl};
  //get formPatientId(): FormControl {return this.choosePatientForm?.get("patientId") as FormControl};

  /*getAllPatients(){
    this.http.get<Patient[]>("https://localhost:5001/api/patient/Get").subscribe(data =>{
      this.patients=data;
    })
  }*/

  getAllSpecialisations(){
    this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data =>{
      this.specialisations=data;
    })
  }

  search(){
    this.selectedSpecialisation = this.formSpecialisationId.value;
    this.http.get<ReturnMedicalAppointment[]>(this.APIUrl+"/GetBySpecialisation/" + this.selectedSpecialisation).subscribe(data =>{
      this.medicalAppointments=data;
    })   
  }

  chooseAppointment(medicalAppointmentId: number): void {
    console.log("Selected MedicalAppointment ID:", medicalAppointmentId);
    for (let i=0; i < this.medicalAppointments.length; ++i) {
      if(this.medicalAppointments[i].id == medicalAppointmentId){
        this.selectedAppointment = this.medicalAppointments[i];
        break;
      }
    }
    //this.selectedAppointment.patientId = this.formPatientId.value;
    this.selectedAppointment.patientId = this.patientId;
    this.setPatientToAppointment(this.selectedAppointment);
  }


  setPatientToAppointment(selectedAppointment: ReturnMedicalAppointment) {
    this.http.put<MedicalAppointment>(this.APIUrl+"/update", this.selectedAppointment)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

}
