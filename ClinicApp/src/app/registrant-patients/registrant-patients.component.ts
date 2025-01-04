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
  patient: Patient = { id: 0, pesel: '', name: '', surname: '', patientNumber: '', isAvailable: true };
  isFormVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;
  isCreateAccountMode: boolean = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) { //formbuilder do formGroup
    this.patientForm = this.formBuilder.group({});
  }

  ngOnInit() {
    this.getAllPatients();
    this.patientForm = this.formBuilder.group({
      id: Number,
      name: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)]}),
      surname: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)]}),
      pesel: new FormControl(null, { validators: [Validators.minLength(11), Validators.maxLength(11), Validators.required, Validators.pattern(/^\d{11}$/)]}),
      email: new FormControl(null, { validators: [Validators.required, Validators.email, // Sprawdza poprawność adresu email
        Validators.maxLength(256)] }),
      password: new FormControl(null, { validators: [Validators.required, Validators.minLength(6),
        Validators.maxLength(100)] }),
    });
  }

  get formId(): FormControl { return this.patientForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.patientForm?.get("name") as FormControl };
  get formSurname(): FormControl { return this.patientForm?.get("surname") as FormControl };
  get formPesel(): FormControl { return this.patientForm?.get("pesel") as FormControl };
  get formEmail(): FormControl { return this.patientForm?.get("email") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formPassword(): FormControl { return this.patientForm?.get("password") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


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
        this.patientForm.reset();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }


  delete(patientId: number){
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
    this.isCreateAccountMode = false;
    this.setConditionalValidation();
    //this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  }

  addNewAccount() {
    this.isFormVisible = true;
    this.isAddingMode = false;
    this.isEditableMode = false;
    this.isCreateAccountMode = true;
    this.setConditionalValidation();
    //this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isCreateAccountMode: ', this.isCreateAccountMode);
  }

  setConditionalValidation() {
    const emailC = this.patientForm.get('email');
    const passwordC = this.patientForm.get('password');

    if (this.isCreateAccountMode) {
      // Dodaj walidator `required`
      emailC?.setValidators([Validators.required]);
      passwordC?.setValidators([Validators.required]);

    } else {
      // Usuń walidator `required`
      emailC?.clearValidators();
      passwordC?.clearValidators();
    }
    // Uruchom ponowną walidację
    emailC?.updateValueAndValidity();
    passwordC?.updateValueAndValidity();
  }

  cancelAdding() {
    this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.isCreateAccountMode = false;
    this.patientForm.reset();
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  cancelEditing() { //do wyrzucenia, kalka powyzszej
    this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.isCreateAccountMode = false; 
    this.patientForm.reset();
    //this.isAddingModeChange.emit(this.isAddingMode);
  }


  addPatient() {
    //this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      return;
    }
    this.clinicService.addPatient(this.patientForm.getRawValue()) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Patient) => {
          this.patient = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllPatients();
          this.isAddingMode = false;
          this.patientForm.reset();
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      });
  }

  createPatientAccount() {
    //this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      return;
    }
    this.clinicService.createPatientAccount(this.patientForm.getRawValue()) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Patient) => {
          this.patient = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllPatients();
          this.isCreateAccountMode = false;
          this.patientForm.reset();
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      });
  }


}
