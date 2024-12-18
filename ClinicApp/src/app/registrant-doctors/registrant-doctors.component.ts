import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Doctor } from '../model/doctor';
import { Specialisation } from '../model/specialisation';

@Component({
  selector: 'app-registrant-doctors',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './registrant-doctors.component.html',
  styleUrl: './registrant-doctors.component.css'
})
export class RegistrantDoctorsComponent {
  //isAddNewDoctorVisible: boolean = false;
  readonly APIUrl = "https://localhost:5001/api/Doctor";
  doctors: Doctor[] = [];
  doctorForm: FormGroup;
  isDisable = false;
  specialisations: Specialisation[] = [];
  //isVisible: boolean = false;
  MedicalSpecialisationsIds: number[] = [];
  doctor: Doctor;
  //isFormVisible: boolean = true;
  isEditableMode = false;  
  isAddingMode = false; //niepotrzebne?





  constructor(private http: HttpClient, private formBuilder: FormBuilder) {
    this.doctorForm = this.formBuilder.group({});
    this.doctor = { id: 0, name: '', surname: '', doctorNumber: '', specialisation: [] }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }




  ngOnInit() {
    this.getAllDoctors();
    this.getAllSpecialisations();
    this.doctorForm = this.formBuilder.group({
      MedicalSpecialisationsIds: new FormArray([], { validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required] }),
      //MedicalSpecialisationsIds: this.doctorForm.get('specialisationsIdForm'), // Powiązanie
      id: Number,
      name: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required] }),
      surname: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required] }),
      doctorNumber: new FormControl('', { validators: [Validators.minLength(1), Validators.maxLength(10), Validators.required] }),

    });
  }

  get formId(): FormControl { return this.doctorForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.doctorForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSurname(): FormControl { return this.doctorForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formDoctorNumber(): FormControl { return this.doctorForm?.get("doctorNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSpecialisations(): FormControl { return this.doctorForm?.get("MedicalSpecialisationsIds") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


  getAllSpecialisations() {
    this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data => {
      this.specialisations = data;
    })
  }

  /*cancel() {
    this.isFormVisible = true;
  } */

  addDoctor() {
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

  getAllDoctors() {
    this.http.get<Doctor[]>(this.APIUrl + "/Get").subscribe(data => {
      this.doctors = data;
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

  edit(doctor: Doctor) {
    //this.isVisible = true;
    this.formId.setValue(doctor.id);
    this.formName.setValue(doctor.name);
    this.formSurname.setValue(doctor.surname);
    this.formDoctorNumber.setValue(doctor.doctorNumber);
    this.formSpecialisations.setValue(doctor.specialisation);
    this.isEditableMode = true;  
    this.isAddingMode = false; //niepotrzebne?


    //this.fillForm(measure);
  }


  addNewDoctor() {
    //this.isFormVisible = true;
    this.isAddingMode = true;
    this.isEditableMode = false;
    //this.isAddingModeChange.emit(this.isAddingMode); // Informuje rodzica o zmianie
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
  }


  cancelAdding() {
    //this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }

  cancelEditing() {
    //this.isFormVisible = false;
    this.isAddingMode = false;
    this.isEditableMode = false; //niepotrzebne?
    //this.isAddingModeChange.emit(this.isAddingMode);
  }


  update() {
    if (this.doctorForm.invalid) {
      this.doctorForm.markAllAsTouched();
      return;
    }
    this.http.put<Doctor>(this.APIUrl + "/update", this.doctorForm.getRawValue())
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })

  }

  delete(doctorId: number) {
    this.http.delete<string>(this.APIUrl + "/Delete/" + doctorId)
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
