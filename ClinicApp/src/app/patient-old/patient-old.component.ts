import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
import { ActivatedRoute } from '@angular/router';

import { Component, Input } from '@angular/core';
import { MakeAnAppointmentComponent } from "../make-an-appointment/make-an-appointment.component";
import { GetMedicalAppointmentsForPatientComponent } from "../get-medical-appointments-for-patient/get-medical-appointments-for-patient.component";

@Component({
  selector: 'app-patient-old',
  standalone: true,
  imports: [MakeAnAppointmentComponent, GetMedicalAppointmentsForPatientComponent, 
            HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './patient-old.component.html',
  styleUrl: './patient-old.component.css'
})

export class PatientOldComponent {
  //@Input()
  patientId: number = 0;
  choosePatientForm: FormGroup;
  patients: Patient[]= [];
  //isDisabled: boolean = true;
  isPatientIdSet: boolean = this.patientId!==0;
  historyPanel: boolean = false;

  activeComponent: string = ''; // Nazwa aktywnego komponentu dziecka

  // Metoda do ustawienia aktywnego komponentu
  setActiveComponent(componentName: string): void {
    this.activeComponent = componentName;
  }

  constructor(private http:HttpClient, private formBuilder: FormBuilder, private route: ActivatedRoute){
    this.choosePatientForm = this.formBuilder.group({});
    /*this.route.params.subscribe(params => {
      this.patientId = params['patientId']; // Odczytanie parametru z URL
    });*/
  }

  ngOnInit(){
    //jesli w trybie pacjenta - już nie jest 0 :
    //this.patientId = //z logowania
    this.historyPanel = false;
    //this.getAllPatients();
    /*this.choosePatientForm = this.formBuilder.group({
      patientId: new FormControl(null, {validators: [Validators.required]})
    }); */
    this.route.params.subscribe(params => {
      this.patientId = +params['patientId']; // Przypisanie id z URL
      console.log('Received patientId:', this.patientId);
    });
  }

  //get formPatientId(): FormControl {return this.choosePatientForm?.get("patientId") as FormControl};

  /*getAllPatients(){
    this.http.get<Patient[]>("https://localhost:5001/api/patient/Get").subscribe(data =>{
      this.patients=data;
    })
  }*/

  onPatientChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.patientId = parseInt(selectElement.value, 10); // Pobranie wybranego ID pacjenta
    console.log('Selected Patient ID:', this.patientId);
    //this.isDisabled = false;
  }

}
