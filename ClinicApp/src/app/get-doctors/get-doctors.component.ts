import { Component } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Doctor } from '../model/doctor';
import { Specialisation } from '../model/specialisation';

@Component({
  selector: 'app-get-doctors',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-doctors.component.html',
  styleUrl: './get-doctors.component.css'
})
export class GetDoctorsComponent {
  readonly APIUrl="https://localhost:5001/api/Doctor";
  doctors: Doctor[] = [];
  doctorForm: FormGroup;
  isDisable = false;
  specialisations: Specialisation[] = [];
  isVisible: boolean = false;
  MedicalSpecialisationsIds: number[] = [];



  constructor(private http:HttpClient, private formBuilder: FormBuilder){
    this.doctorForm = this.formBuilder.group({});
  }
  
  ngOnInit(){
    this.getAllDoctors();
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

  get formId(): FormControl {return this.doctorForm?.get("id") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl {return this.doctorForm?.get("name") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSurname(): FormControl {return this.doctorForm?.get("surname") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formDoctorNumber(): FormControl {return this.doctorForm?.get("doctorNumber") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSpecialisations(): FormControl {return this.doctorForm?.get("MedicalSpecialisationsIds") as FormControl}; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


  getAllSpecialisations(){
    this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data =>{
      this.specialisations=data;
    })
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

  getAllDoctors(){
    this.http.get<Doctor[]>(this.APIUrl+"/Get").subscribe(data =>{
      this.doctors=data;
    })
  }

  /*edit(doctor: Doctor){
    this.http.put<Doctor>(this.APIUrl+"/update", doctor)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })
  }*/

  edit(doctor: Doctor){
    this.isVisible = true;  
    this.formId.setValue(doctor.id);
    this.formName.setValue(doctor.name);
    this.formSurname.setValue(doctor.surname);
    this.formDoctorNumber.setValue(doctor.doctorNumber);
    this.formSpecialisations.setValue(doctor.specialisation);

    //this.fillForm(measure);
  }


  update(){
    if(this.doctorForm.invalid){
      this.doctorForm.markAllAsTouched(); 
      return;
    }
    this.http.put<Doctor>(this.APIUrl+"/update", this.doctorForm.getRawValue())
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }

  delete(doctorId: number){
    this.http.delete<string>(this.APIUrl+"/Delete/"+ doctorId)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllDoctors();
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    });
  }


}
