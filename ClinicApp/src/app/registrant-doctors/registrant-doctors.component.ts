import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Doctor } from '../model/doctor';
import { Specialisation } from '../model/specialisation';
import { DoctorWithSpcecialisations } from '../model/doctor-with-specialisations';
import { ClinicService } from '../services/clinic.service';
import { atLeastOneSelectedValidator } from '../validators';

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
  specialisationsArray: any;





  constructor(private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.doctorForm = this.formBuilder.group({});
    this.doctorWithSpecialisations = { id: 0, name: '', surname: '', doctorNumber: '', specialisationIds: [], isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    this.doctor = { id: 0, name: '', surname: '', doctorNumber: '', specialisation: [], isAvailable: true }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
  }




  ngOnInit() {
    this.getAllDoctors();
    this.getAllAvailableSpecialisations();
    this.doctorForm = this.formBuilder.group({
      medicalSpecialisationsIds: new FormArray([], { validators: [atLeastOneSelectedValidator()] }),
      id: Number,
      name: new FormControl('', { validators: [Validators.minLength(1), Validators.maxLength(60), Validators.required] }),
      surname: new FormControl('', { validators: [Validators.minLength(1), Validators.maxLength(60), Validators.required] }),
      doctorNumber: new FormControl(''/*, {validators: [Validators.required]}*/),
    });
  }

  get formId(): FormControl { return this.doctorForm?.get("id") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formName(): FormControl { return this.doctorForm?.get("name") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSurname(): FormControl { return this.doctorForm?.get("surname") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formDoctorNumber(): FormControl { return this.doctorForm?.get("doctorNumber") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?
  get formSpecialisations(): FormControl { return this.doctorForm?.get("MedicalSpecialisationsIds") as FormControl }; //CZYM GROZI ZNAK ZAPYTANIA TUTAJ?


  getAllAvailableSpecialisations() {
    this.clinicService.getAllAvailableSpecialisations().subscribe(data => {
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
    if (this.doctorForm.invalid) {
      this.doctorForm.markAllAsTouched();
      return;
    }
    console.log('Form Value before:', this.doctorForm.getRawValue());
    const formValue = this.doctorForm.getRawValue();
    /*const requestBody = {
      ...this.doctorForm.getRawValue(),
      specialisationsList: this.doctorSpecialisationsList
    };*/
    /*const requestBody = {
      id: this.doctorForm.get('id')?.value,
      name: this.doctorForm.get('name')?.value,
      surname: this.doctorForm.get('surname')?.value,
      doctorNumber: this.doctorForm.get('doctorNumber')?.value,
      medicalSpecialisationsIds: this.doctorSpecialisationsList,
    };*/
    this.clinicService.addDoctor(formValue) // Bez obiektu opakowującego
      .subscribe({
        next: (result: Doctor) => {
          this.doctor = result;
          this.getAllDoctors();
          this.isAddingMode = false;
          this.doctorSpecialisationsList = [];
          this.doctorSpecialisationsList = [];
          this.doctorForm.reset();
          this.formDoctorNumber.setValue('');
          this.formId.setValue('');

          //CZYSZCZENIE COMBOBOXA:
          const specialisationsArrayTemp = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
          while (specialisationsArrayTemp.length) {
            specialisationsArrayTemp.removeAt(0); // Usuwanie każdego kontrolera
          }
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
    //var specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
    this.specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;



    //this.MedicalSpecialisationsIds = this.doctorForm.get('MedicalSpecialisationsIds') as FormArray;
    //this.specialisationsIds = this.doctorForm.get('specialisationsIdForm') as FormArray;

    if (checkbox.checked) {
      this.specialisationsArray.push(new FormControl(+checkbox.value)); // Zamiana na number
      specialisation.checked = true;
      this.doctorSpecialisationsList.push(specialisation.id); //lista starych i nowych specek

    } else {
      const index = this.specialisationsArray.controls.findIndex(
        (ctrl: { value: number; }) => ctrl.value === +checkbox.value
      );
      if (index !== -1) {
        this.specialisationsArray.removeAt(index);
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
    this.specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
    //this.formSpecialisations.setValue(doctor.specialisation);
    for (let specialisation of this.specialisations) {
      for (let doctorSpecialisation of doctor.specialisationIds) {
        if (specialisation.id == doctorSpecialisation) {
          specialisation.checked = true;
          this.doctorSpecialisationsList.push(doctorSpecialisation); //dodanie starych specjalizacji
          this.specialisationsArray.push(new FormControl(doctorSpecialisation)); // Zamiana na number

          //this.specialisationsArray.push(doctorSpecialisation);

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
    this.doctorSpecialisationsList = [];
    this.specialisationsArray = [];
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
      console.log('cala lista:', this.doctorSpecialisationsList);
      return;
    }

    const requestBody = {
      ...this.doctorForm.getRawValue(),
      specialisationsList: this.doctorSpecialisationsList
    };

    //this.http.put<Doctor>(this.APIUrl + "/update", requestBody) //WERSJA BEZ SERWISU
    const formValue = this.doctorForm.getRawValue();
    this.clinicService.updateDoctor(formValue)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfullyy:", response);
          this.getAllDoctors();
          this.isEditableMode = false;
          this.doctorSpecialisationsList = []; //czyszczenie listy?
          this.doctorSpecialisationsList = [];
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })



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
