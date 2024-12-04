import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { Doctor } from '../model/doctor';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Specialisation } from '../model/specialisation';

@Component({
  selector: 'app-add-doctor',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './add-doctor.component.html',
  styleUrl: './add-doctor.component.css'
})

export class AddDoctorComponent {
  readonly APIUrl="https://localhost:5001/api/doctor";
  doctors: Doctor[] = [];
  specialisations: Specialisation[] = [];
  MedicalSpecialisationsIds: number[] = [];
  doctorForm: FormGroup;
  isDisable = false;
  doctor: Doctor;

  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.doctorForm = this.formBuilder.group({});
    this.doctor = { id: 0,  name: '', surname: '', doctorNumber: '', specialisation: [] }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }

  ngOnInit(){
    this.getAllSpecialisations();
    this.doctorForm = this.formBuilder.group({
      MedicalSpecialisationsIds: new FormArray([], {validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required]}),
      //MedicalSpecialisationsIds: this.doctorForm.get('specialisationsIdForm'), // Powiązanie
      id: Number,
      name: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required]}),
      surname: new FormControl('', {validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required]}),
      doctorNumber: new FormControl('', {validators: [Validators.minLength(1), Validators.maxLength(10), Validators.required]}),
        
    });
    
  }

  get formId(): FormControl {return this.doctorForm?.get("id") as FormControl};
  get formName(): FormControl {return this.doctorForm?.get("name") as FormControl};
  get formSurname(): FormControl {return this.doctorForm?.get("surname") as FormControl};
  get formDoctorNumber(): FormControl {return this.doctorForm?.get("doctorNumber") as FormControl};
  //get formSpecialisations(): FormControl {return this.doctorForm?.get("doctorNumber") as FormControl};

  addDoctor(){
    console.log('Form Value before:', this.doctorForm.getRawValue());
    const formValue = this.doctorForm.getRawValue();
    //formValue.MedicalSpecialisationsIds = formValue.specialisationsIdForm;
    //console.log('Form Value after:', this.doctorForm.getRawValue());
    //this.http.post<Doctor>(this.APIUrl + "/create", this.doctorForm.getRawValue()) // Bez obiektu opakowującego

    this.http.post<Doctor>(this.APIUrl + "/create", formValue) // Bez obiektu opakowującego
    .subscribe({
      next: (result: Doctor) => {
        this.doctor = result; 
      },
      error: (err) => {
        console.error("Error occurred:", err); 
      }
    });
  }

  getAllSpecialisations(){
    this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data =>{
      this.specialisations=data;
    })
  }

  // Getter dla FormArray
  get specialisationsArray(): FormArray {
    return this.doctorForm.get('specialisationsIdForm') as FormArray;
  }

  onSpecialisationChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const specialisationsArray = this.doctorForm.get('MedicalSpecialisationsIds') as FormArray;
    //this.specialisationsIds = this.doctorForm.get('specialisationsIdForm') as FormArray;
  
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
    //this.MedicalSpecialisationsIds = specialisationsArray.value;

  }

 /* onSpecialisationChange(event: any): void {
    //const selectedSpecialisation = event.target.value;
    this.selectedSpecialisation = event.target.value;
    if (event.target.checked) {
      // Dodaj specjalizację do FormArray, jeśli zaznaczone
      this.specialisationsArray.push(new FormControl(this.selectedSpecialisation));
    } else {
      // Usuń specjalizację z FormArray, jeśli odznaczone
      const index = this.specialisationsArray.controls.findIndex(
        (control) => control.value === this.selectedSpecialisation
      );
      this.specialisationsArray.removeAt(index);
    }
  }*/

  

  /*getAllSpecialisations(){
    this.http.get<Specialisation>(this.APIUrl + "/get", this.doctorForm.getRawValue()) // Bez obiektu opakowującego
    .subscribe({
      next: (result: Doctor) => {
        this.doctor = result; 
      },
      error: (err) => {
        console.error("Error occurred:", err); 
      }
    });
  }*/
}
