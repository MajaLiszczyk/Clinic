import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Doctor } from '../model/doctor';
import { Specialisation } from '../model/specialisation';
import { DoctorWithSpcecialisations } from '../model/doctor-with-specialisations';
import { ClinicService } from '../services/clinic.service';
import { atLeastOneSelectedValidator, passwordValidator } from '../validators';

@Component({
  selector: 'app-registrant-doctors',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, ReactiveFormsModule],
  templateUrl: './registrant-doctors.component.html',
  styleUrl: './registrant-doctors.component.css'
})
export class RegistrantDoctorsComponent {
  doctors: Doctor[] = [];
  doctorsWithSpecialisations: DoctorWithSpcecialisations[] = [];
  doctorForm: FormGroup;
  isDisable = false;
  specialisations: Specialisation[] = [];
  doctor: Doctor;
  doctorWithSpecialisations: DoctorWithSpcecialisations;
  isEditableMode = false;
  isAddingMode = false; 
  doctorSpecialisationsList: number[] = [];
  specialisationsArray: any;
  registrantId: number = 0;
  passwordVisible = false;

  constructor(private route: ActivatedRoute, private http: HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService) {
    this.doctorForm = this.formBuilder.group({});
    this.doctorWithSpecialisations = { id: 0, name: '', surname: '', doctorNumber: '', specialisationIds: [], isAvailable: true };
    this.doctor = { id: 0, name: '', surname: '', doctorNumber: '', specialisation: [], isAvailable: true };
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId'];
    });
    this.getAllDoctors();
    this.getAllAvailableSpecialisations();
    this.doctorForm = this.formBuilder.group({
      medicalSpecialisationsIds: new FormArray([], { validators: [atLeastOneSelectedValidator()] }),
      id: Number,
      name: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
      surname: new FormControl(null, { validators: [Validators.required, Validators.maxLength(100), Validators.pattern(/^[a-zA-ZąęłńśćżźóĄĘŁŃŚĆŻŹÓ]+$/)] }),
      doctorNumber: new FormControl(null, { validators: [Validators.required, Validators.pattern(/^[1-9][0-9]{6}$/)] }),
      email: new FormControl(null, {
        validators: [Validators.required, Validators.email,
        Validators.maxLength(256)]
      }),
      password: new FormControl(null, { validators: [Validators.required, passwordValidator] }),
    });
  }

  get formId(): FormControl { return this.doctorForm?.get("id") as FormControl };
  get formName(): FormControl { return this.doctorForm?.get("name") as FormControl };
  get formSurname(): FormControl { return this.doctorForm?.get("surname") as FormControl };
  get formDoctorNumber(): FormControl { return this.doctorForm?.get("doctorNumber") as FormControl };
  get formSpecialisations(): FormControl { return this.doctorForm?.get("MedicalSpecialisationsIds") as FormControl };
  get formEmail(): FormControl { return this.doctorForm?.get("email") as FormControl };
  get formPassword(): FormControl { return this.doctorForm?.get("password") as FormControl };

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  getAllAvailableSpecialisations() {
    this.clinicService.getAllAvailableSpecialisations().subscribe(data => {
      this.specialisations = data;
    })
  }

  addDoctor() {
    if (this.doctorForm.invalid) {
      this.doctorForm.markAllAsTouched();
      return;
    }
    const formValue = this.doctorForm.getRawValue();
    this.clinicService.addDoctor(formValue)
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
          this.doctorForm.reset();

          const specialisationsArrayTemp = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
          while (specialisationsArrayTemp.length) {
            specialisationsArrayTemp.removeAt(0);
          }
        },
        error: (err) => {
          console.error("Error occurred:", err);
        }
      });
  }

  onSpecialisationChange(event: Event, specialisation: Specialisation): void {
    const checkbox = event.target as HTMLInputElement;
    this.specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;

    if (checkbox.checked) {
      this.specialisationsArray.push(new FormControl(+checkbox.value));
      specialisation.checked = true;
      this.doctorSpecialisationsList.push(specialisation.id);

    } else {
      const index = this.specialisationsArray.controls.findIndex(
        (ctrl: { value: number; }) => ctrl.value === +checkbox.value
      );
      if (index !== -1) {
        this.specialisationsArray.removeAt(index);
      }
      specialisation.checked = false;
    }
  }

  getAllDoctors() {
    this.clinicService.getAllDoctorsWithSpecialisations().subscribe(data => {
      this.doctorsWithSpecialisations = data;
    })
  }

  edit(doctor: DoctorWithSpcecialisations) {
    this.formId.setValue(doctor.id);
    this.formName.setValue(doctor.name);
    this.formSurname.setValue(doctor.surname);
    this.formDoctorNumber.setValue(doctor.doctorNumber);
    this.formDoctorNumber.setValue(doctor.doctorNumber);
    this.specialisationsArray = this.doctorForm.get('medicalSpecialisationsIds') as FormArray;
    for (let specialisation of this.specialisations) {
      for (let doctorSpecialisation of doctor.specialisationIds) {
        if (specialisation.id == doctorSpecialisation) {
          specialisation.checked = true;
          this.doctorSpecialisationsList.push(doctorSpecialisation); //dodanie starych specjalizacji
          this.specialisationsArray.push(new FormControl(doctorSpecialisation));
        }
      }
    }
    this.isEditableMode = true;
    this.isAddingMode = false;
  }

  addNewDoctor() {
    this.isAddingMode = true;
    this.isEditableMode = false;
    console.log('isAddingMode in AddPatient:', this.isAddingMode);
    this.doctorSpecialisationsList = [];
    this.specialisationsArray = [];
  }

  cancelAdding() {
    this.isAddingMode = false;
    this.isEditableMode = false;
    this.doctorForm.reset();
  }

  cancelEditing() {
    this.isAddingMode = false;
    this.isEditableMode = false;
    this.doctorForm.reset();
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

    const formValue = this.doctorForm.getRawValue();
    this.clinicService.updateDoctor(formValue)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfullyy:", response);
          this.getAllDoctors();
          this.isEditableMode = false;
          this.doctorSpecialisationsList = [];
          this.doctorSpecialisationsList = [];
          this.doctorForm.reset();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })
  }

  delete(doctorId: number) {
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