import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Patient } from '../model/patient';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { ClinicService } from '../services/clinic.service';


@Component({
  selector: 'app-registrant-patients',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink, RouterOutlet],
  templateUrl: './registrant-patients.component.html',
  styleUrl: './registrant-patients.component.css'
})
export class RegistrantPatientsComponent {

  //readonly APIUrl = "https://localhost:5001/api/patient";
  patients: Patient[] = [];
  patientForm: FormGroup;
  patientId: number = 0;
  name: string = '';
  surname: string = '';
  pesel: string = '';
  isDisable = false;
  patient: Patient = { id: 0, pesel: '', name: '', surname: '', patientNumber: '' };
  isFormVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) { //formbuilder do formGroup
    this.patientForm = this.formBuilder.group({});
  }

  ngOnInit() {
    this.getAllPatients();
    this.patientForm = this.formBuilder.group({
      id: Number,
      name: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required] }),
      surname: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required] }),
      pesel: new FormControl('', { validators: [Validators.minLength(11), Validators.maxLength(11), Validators.required] })
    });
  }

  get formId(): FormControl { return this.patientForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.patientForm?.get("name") as FormControl };
  get formSurname(): FormControl { return this.patientForm?.get("surname") as FormControl };
  get formPesel(): FormControl { return this.patientForm?.get("pesel") as FormControl };

  getAllPatients(){
    //this.http.get<Patient[]>(this.APIUrl+"/Get").subscribe(data =>{
    this.clinicService.getAllPatients().subscribe(data =>{
      this.patients=data;
    })
  }

  /*getPatients() {
    this.http.get<Patient[]>(this.APIUrl + "/GetAll").subscribe(data => {
      this.patients = data;
    })
  } */

  edit(patient: Patient){
    this.isEditableMode = true;  
    this.isAddingMode = false; //niepotrzebne?
    this.formId.setValue(patient.id);
    this.formName.setValue(patient.name);
    this.formSurname.setValue(patient.surname);
    this.formPesel.setValue(patient.pesel);
  }

  update(){
    if(this.patientForm.invalid){
      this.patientForm.markAllAsTouched(); 
      return;
    }
    const patientData = this.patientForm.getRawValue();
    //this.http.put<Patient>(this.APIUrl+"/update", this.patientForm.getRawValue())
    this.clinicService.updatePatient(patientData)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllPatients();
        this.isEditableMode = false;
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }


  delete(patientId: number){
    //this.http.delete<string>(this.APIUrl+"/Delete/"+patientId)
    this.clinicService.deletePatient(patientId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllPatients();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

  addNewPatient() {
    this.isFormVisible = true;
    this.isAddingMode = true;
    this.isEditableMode = false;
    //this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  }

  cancelAdding() {
    this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  cancelEditing() {
    this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }


  addPatient() {
    //this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
    this.clinicService.addPatient(this.patientForm.getRawValue()) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Patient) => {
          this.patient = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllPatients();
          this.isAddingMode = false;
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      });
  }


}
