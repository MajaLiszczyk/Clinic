import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';


@Component({
  selector: 'app-add-specialisation',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule],
  templateUrl: './add-specialisation.component.html',
  styleUrl: './add-specialisation.component.css'
})
export class AddSpecialisationComponent {
  readonly APIUrl="https://localhost:5001/api/MedicalSpecialisation";
  specialisations: Specialisation[] = [];
  specialisationForm: FormGroup;
  specialisation: Specialisation = { id: 0, name: '', checked: false , isAvailable: true};

  constructor(private http:HttpClient, private formBuilder: FormBuilder){ 
    this.specialisationForm = this.formBuilder.group({});
  }

  ngOnInit(){
    this.specialisationForm = this.formBuilder.group({
      id: Number,
      name: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(50), Validators.required]})
    });
  }

  get formId(): FormControl {return this.specialisationForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl {return this.specialisationForm?.get("name") as FormControl};

  getSpecialisations(){
    this.http.get<Specialisation[]>(this.APIUrl+"/GetAll").subscribe(data =>{
      this.specialisations=data;
    })
  }

 


  addSpecialisation(){
    this.http.post<Specialisation>(this.APIUrl + "/create", this.specialisationForm.getRawValue()) // Bez obiektu opakowującego
    .subscribe({
      next: (result: Specialisation) => {
        this.specialisation = result; // Zwrócony obiekt przypisany do zmiennej
      },
      error: (err) => {
        console.error("Error occurred:", err); // Obsługa błędów
      }
    });
  }

}
