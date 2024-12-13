import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Patient } from '../model/patient';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-add-patient',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink, RouterOutlet],
  templateUrl: './add-patient.component.html',
  styleUrl: './add-patient.component.css'
})

export class AddPatientComponent {
  readonly APIUrl="https://localhost:5001/api/patient";
  patients: Patient[] = [];
  patientForm: FormGroup;
  patientId: number = 0;
  name: string = '';
  surname: string = '';
  pesel: string = '';
  isDisable = false;
  patient: Patient = { id: 0, pesel: '', name: '', surname: '' };
  isFormVisible: boolean = false;
  
  @Input() isAddingMode: boolean = false; // Odbiera zmienną od rodzica
  @Output() isAddingModeChange = new EventEmitter<boolean>(); // Wysyła zmiany do rodzica


  constructor(private http:HttpClient, private formBuilder: FormBuilder){ //formbuilder do formGroup
    this.patientForm = this.formBuilder.group({});
  }

  ngOnInit(){
    //3this.getMeasures(); //do tablicy measures[] zaciągane są wszystkie pomiary z getAll
    this.patientForm = this.formBuilder.group({
      id: Number,
      name: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required]}),
      surname: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required]}),
      pesel: new FormControl('', {validators: [Validators.minLength(11), Validators.maxLength(11), Validators.required]})
    });
    //this.isLoaded = true;
  }

  /*toggleAddingMode() {
    this.isAddingMode = !this.isAddingMode;
    this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  } */

  get formId(): FormControl {return this.patientForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl {return this.patientForm?.get("name") as FormControl};
  get formSurname(): FormControl {return this.patientForm?.get("surname") as FormControl};
  get formPesel(): FormControl {return this.patientForm?.get("pesel") as FormControl};

  getPatients(){
    this.http.get<Patient[]>(this.APIUrl+"/GetAll").subscribe(data =>{
      this.patients=data;
    })
  }

  addNewPatient(){
    this.isFormVisible = true;
    //this.isAddingMode = !this.isAddingMode;
    this.isAddingMode = true;
    this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  }

  cancel(){
    this.isFormVisible = false;
    this.isAddingMode = false;
    this.isAddingModeChange.emit(this.isAddingMode);

  }

  setName(event: Event){
    const inputElement  = event.target as HTMLInputElement; 
    inputElement
    this.name = inputElement.value;
  }

  setSurname(event: Event){
    const inputElement  = event.target as HTMLInputElement; 
    inputElement
    this.surname = inputElement.value;
  }

  setPesel(event: Event){
    const inputElement = event.target as HTMLInputElement;
    inputElement
    this.pesel = inputElement.value;
  }


  addPatient(){
    /*this.patient.id = this.patientId;
    this.patient.pesel = this.pesel;
    this.patient.name = this.name;
    this.patient.surname = this.surname;*/
    this.http.post<Patient>(this.APIUrl + "/create", this.patientForm.getRawValue()) // Bez obiektu opakowującego
    .subscribe({
      next: (result: Patient) => {
        this.patient = result; // Zwrócony obiekt przypisany do zmiennej
      },
      error: (err) => {
        console.error("Error occurred:", err); // Obsługa błędów
      }
    });
  }

}
