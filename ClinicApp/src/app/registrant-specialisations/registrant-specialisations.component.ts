import { Component } from '@angular/core';
//import { AddSpecialisationComponent } from '../add-specialisation/add-specialisation.component';
//import { GetSpecialisationsComponent } from '../get-specialisations/get-specialisations.component';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Specialisation } from '../model/specialisation';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ClinicService } from '../services/clinic.service';

@Component({
  selector: 'app-registrant-specialisations',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './registrant-specialisations.component.html',
  styleUrl: './registrant-specialisations.component.css'
})
export class RegistrantSpecialisationsComponent {
  isAddNewSpecialisationVisible: boolean = false;
  specialisations: Specialisation[] = [];
  specialisationF: FormGroup;
  specialisation: Specialisation = { id: 0, name: '', checked: false, isAvailable: true };
  isDisable = false;
  isVisible: boolean = false;
  isAddingMode: boolean = false;
  isEditableMode: boolean = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.specialisationF = this.formBuilder.group({});
  }

  ngOnInit() {
    this.getAllSpecialisations();
    this.specialisationF = this.formBuilder.group({
      id: Number,
      name: new FormControl(null, { validators: [Validators.required] })
    });
  }

  get formId(): FormControl { return this.specialisationF?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.specialisationF?.get("name") as FormControl };

  getAllSpecialisations(){
    this.clinicService.getAllSpecialisations().subscribe(data =>{
      this.specialisations=data;
    })
  }

  edit(specialisation: Specialisation){
    this.isVisible = true;  
    this.isAddingMode = false; //niepotrzebne?
    this.formId.setValue(specialisation.id);
    this.formName.setValue(specialisation.name);
    this.isEditableMode = true;
  }

  update(){
    if(this.specialisationF.invalid){
      this.specialisationF.markAllAsTouched(); 
      return;
    }
    this.clinicService.updateSpecialisation(this.specialisationF.getRawValue())
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllSpecialisations();
        this.isEditableMode = false;
        this.specialisationF.reset();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })
  }

  delete(dspecialisationId: number){
    this.clinicService.deleteSpecialisation(dspecialisationId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllSpecialisations();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }

  addNewSpecialisation() {
    this.isAddingMode = true;
    this.isEditableMode = false;
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  }

  cancelAdding() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.specialisationF.reset();
  }

  cancelEditing() {
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    this.specialisationF.reset();
  }

  addSpecialisation() {
    if(this.specialisationF.invalid){
      this.specialisationF.markAllAsTouched(); 
      return;
    }
    this.clinicService.addSpecialisation(this.specialisationF.getRawValue()) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Specialisation) => {
          this.specialisation = result; // Zwrócony obiekt przypisany do zmiennej
          this.getAllSpecialisations();
          this.isAddingMode = false;
          this.specialisationF.reset();
        },
        error: (err) => {
          console.error("Error occurred:", err); // Obsługa błędów
        }
      });
  }
}