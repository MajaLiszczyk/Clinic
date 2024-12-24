import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Doctor } from '../model/doctor';
import { Specialisation } from '../model/specialisation';
import { DoctorWithSpcecialisations } from '../model/doctor-with-specialisations';
import { ClinicService } from '../services/clinic.service';

@Component({
  selector: 'app-registrant-doctors',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './registrant-doctors.component.html',
  styleUrl: './registrant-doctors.component.css'
})
export class RegistrantDoctorsComponent {
  //isAddNewDoctorVisible: boolean = false;
  //readonly APIUrl = "https://localhost:5001/api/Doctor";
  doctors: Doctor[] = [];
  doctorsWithSpecialisations: DoctorWithSpcecialisations[] = [];
  doctorForm: FormGroup;
  isDisable = false;
  specialisations: Specialisation[] = [];
  //MedicalSpecialisationsIds: number[] = [];
  doctor: Doctor;
  doctorWithSpecialisations: DoctorWithSpcecialisations;
  isEditableMode = false;  
  isAddingMode = false; //niepotrzebne?
  doctorSpecialisationsList: number[] = [];





  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.doctorForm = this.formBuilder.group({});
    this.doctorWithSpecialisations = { id: 0, name: '', surname: '', doctorNumber: '', specialisationIds: [] }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    this.doctor = { id: 0, name: '', surname: '', doctorNumber: '', specialisation: [] }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }




  ngOnInit() {
    this.getAllDoctors();
    this.getAllSpecialisations();
    this.doctorForm = this.formBuilder.group({
      medicalSpecialisationsIds: new FormArray([]),
      id: Number,
      name: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(30), Validators.required] }),
      surname: new FormControl('', { validators: [Validators.minLength(2), Validators.maxLength(60), Validators.required] }),
      doctorNumber: new FormControl(''), 
    });
  }

  get formId(): FormControl { return this.doctorForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.doctorForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSurname(): FormControl { return this.doctorForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formDoctorNumber(): FormControl { return this.doctorForm?.get("doctorNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSpecialisations(): FormControl { return this.doctorForm?.get("MedicalSpecialisationsIds") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


  getAllSpecialisations() {
    this.clinicService.getAllSpecialisations().subscribe(data => {
      this.specialisations = data;
    })
    /*this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data => {
      this.specialisations = data;
    }) */
  }

  /*cancel() {
    this.isFormVisible = true;
  } */

  addDoctor() {
    console.log('Form Value before:', this.doctorForm.getRawValue());
    const formValue = this.doctorForm.getRawValue();
    this.clinicService.addDoctor(formValue) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Doctor) => {
          this.doctor = result;
          this.getAllDoctors();
          //this.doctorForm.reset;
        },
        error: (err) => {
          console.error("Error occurred:", err);
        }
      });
    /*console.log('Form Value before:', this.doctorForm.getRawValue());
    const formValue = this.doctorForm.getRawValue();
    this.http.post<Doctor>(this.APIUrl + "/create", formValue) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Doctor) => {
          this.doctor = result;
          this.getAllDoctors();
          //this.doctorForm.reset;
        },
        error: (err) => {
          console.error("Error occurred:", err);
        }
      });*/
      
  }

  // Getter dla FormArray
  /*get specialisationsArray(): FormArray {
    return this.doctorForm.get('specialisationsIdForm') as FormArray;
  } */


  onSpecialisationChange(event: Event, specialisation: Specialisation): void {
    const checkbox = event.target as HTMLInputElement;
    const specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
    
    //this.MedicalSpecialisationsIds = this.doctorForm.get('MedicalSpecialisationsIds') as FormArray;
    //this.specialisationsIds = this.doctorForm.get('specialisationsIdForm') as FormArray;

    if (checkbox.checked) {
      specialisationsArray.push(new FormControl(+checkbox.value)); // Zamiana na number
      specialisation.checked = true;
      this.doctorSpecialisationsList.push(specialisation.id); //lista starych i nowych specek

    } else {
      const index = specialisationsArray.controls.findIndex(
        ctrl => ctrl.value === +checkbox.value
      );
      if (index !== -1) {
        specialisationsArray.removeAt(index);
      }
      specialisation.checked = false;
    }
    //this.MedicalSpecialisationsIds = specialisationsArray.value;

  }

  getAllDoctors() {
    this.clinicService.getAllDoctorsWithSpecialisations().subscribe(data => {
      this.doctorsWithSpecialisations = data;
    })
    /*this.http.get<DoctorWithSpcecialisations[]>(this.APIUrl + "/GetWithSpecialisations").subscribe(data => {
      this.doctorsWithSpecialisations = data;
    }) */
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

  //edit(doctor: Doctor) {
  edit(doctor: DoctorWithSpcecialisations) {
    //this.doctorSpecialisations = 
    this.formId.setValue(doctor.id);
    this.formName.setValue(doctor.name);
    this.formSurname.setValue(doctor.surname);
    this.formDoctorNumber.setValue(doctor.doctorNumber);
    //this.formSpecialisations.setValue(doctor.specialisation);
    for (let specialisation of this.specialisations) {
      for (let doctorSpecialisation of doctor.specialisationIds) {
        if(specialisation.id == doctorSpecialisation){
          specialisation.checked = true;
          this.doctorSpecialisationsList.push(doctorSpecialisation); //dodanie starych specjalizacji
        }      
      }
    }
    //this.formSpecialisations.setValue(doctor.specialisationIds);
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
      console.log(this.doctorForm.invalid);
      console.log(this.doctorForm);
      console.log('cala lista:', this.doctorSpecialisationsList );
      return;
    } 

    const requestBody = {
      ...this.doctorForm.getRawValue(),
      specialisationsList: this.doctorSpecialisationsList
    };

    //this.http.put<Doctor>(this.APIUrl + "/update", requestBody) //WERSJA BEZ SERWISU
    this.clinicService.updateDoctor(requestBody)
    .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })

      this.doctorSpecialisationsList = []; //czyszczenie listy?

  }

  delete(doctorId: number) {
    //this.http.delete<string>(this.APIUrl + "/Delete/" + doctorId)
    this.clinicService.deleteDoctor(doctorId)
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
