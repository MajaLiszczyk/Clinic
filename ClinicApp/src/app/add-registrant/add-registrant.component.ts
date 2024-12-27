import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Registrant } from '../model/registrant';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-registrant',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule],
  templateUrl: './add-registrant.component.html',
  styleUrl: './add-registrant.component.css'
})
export class AddRegistrantComponent {

  readonly APIUrl="https://localhost:5001/api/Registrant";
  //patients: Registrant[] = [];
  registrantForm: FormGroup;
  //patientId: number = 0;
  //name: string = '';
  //surname: string = '';
  //pesel: string = '';
  isDisable = false;
  registrant: Registrant = { id: 0, registrantNumber: '', name: '', surname: '' , isAvailable: true };

  constructor(private http:HttpClient, private formBuilder: FormBuilder){ //formbuilder do formGroup
    this.registrantForm = this.formBuilder.group({});
  }

  ngOnInit(){
    //3this.getMeasures(); //do tablicy measures[] zaciągane są wszystkie pomiary z getAll
    this.registrantForm = this.formBuilder.group({
      id: Number,
      name: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required]}),
      surname: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required]}),
      registrantNumber: new FormControl('', {validators: [Validators.minLength(1), Validators.maxLength(10), Validators.required]})
    });
    //this.isLoaded = true;
  }

  get formId(): FormControl {return this.registrantForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl {return this.registrantForm?.get("name") as FormControl};
  get formSurname(): FormControl {return this.registrantForm?.get("surname") as FormControl};
  get formRegistrantNumber(): FormControl {return this.registrantForm?.get("registrantNumber") as FormControl};

  /*getPatients(){
    this.http.get<Patient[]>(this.APIUrl+"/GetAll").subscribe(data =>{
      this.patients=data;
    })
  } */

  /*setName(event: Event){
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
  } */


  addRegistrant(){
    /*this.patient.id = this.patientId;
    this.patient.pesel = this.pesel;
    this.patient.name = this.name;
    this.patient.surname = this.surname;*/
    this.http.post<Registrant>(this.APIUrl + "/create", this.registrantForm.getRawValue()) // Bez obiektu opakowującego
    .subscribe({
      next: (result: Registrant) => {
        this.registrant = result; // Zwrócony obiekt przypisany do zmiennej
      },
      error: (err) => {
        console.error("Error occurred:", err); // Obsługa błędów
      }
    });
  }

}
