import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators, FormsModule, ValidationErrors, AbstractControl } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateStruct, NgbDateAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CreateMedicalAppointment } from '../model/create-medical-appointment';
import { Doctor } from '../model/doctor';
import { MedicalAppointment } from '../model/medical-appointment';
import { ClinicService } from '../services/clinic.service';
import { MedicalAppointmentPatientDoctorDto } from '../dtos/medical-appointment-patient-doctor-dto';

@Component({
  selector: 'app-registrant-appointments',
  standalone: true,
  imports: [RouterLink, CommonModule, ReactiveFormsModule, NgbModule],
  templateUrl: './registrant-appointments.component.html',
  styleUrl: './registrant-appointments.component.css',
  providers: [NgbDateNativeAdapter]
})
export class RegistrantAppointmentsComponent {
  isAddNewAppointmentVisible: boolean = false;
  isGetAllAppointmentsVisible: boolean = false;
  doctors: Doctor[] = [];
  medicalAppointment: MedicalAppointment;
  createdAppointment: CreateMedicalAppointment;
  medicalAppointmentForm: FormGroup;
  isDisable = false;
  medicalAppointments: MedicalAppointmentPatientDoctorDto[] = [];
  isAddingMode: boolean = false;
  isShowingAllAppointmentsMode: boolean = false;
  registrantId: number = 0;

  constructor(private route: ActivatedRoute, private http: HttpClient, private formBuilder: FormBuilder,
    private adapter: NgbDateNativeAdapter, private clinicService: ClinicService) {
    this.medicalAppointmentForm = this.formBuilder.group({});
    this.medicalAppointment = {
      id: 0, dateTime: new Date(), doctorId: 0, patientId: 0,
      diagnosis: '', interview: '', isFinished: false, isCancelled: false, cancellingComment: ''
    };
    this.createdAppointment = { dateTime: new Date(), doctorId: 0 };
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId'];
    });
    this.getAllDoctors();
    this.medicalAppointmentForm = this.formBuilder.group({
      date: new FormControl(null, [Validators.required, this.futureOrTodayDateValidator]),
      time: new FormControl(null, [Validators.required]),
      doctorId: new FormControl(null, [Validators.required])
    }, {
      validators: this.dateTimeValidator
    });
  }

  dateTimeValidator(formGroup: FormGroup): ValidationErrors | null {
    const date = formGroup.get('date')?.value;
    const time = formGroup.get('time')?.value;
    return date && time ? null : { dateTimeRequired: true };
  }

  futureOrTodayDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) {
      return null;
    }
    const selectedDate = new Date(control.value.year, control.value.month - 1, control.value.day);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return selectedDate >= today ? null : { notFutureOrToday: true };
  }

  get formDoctorId(): FormControl { return this.medicalAppointmentForm?.get("doctorId") as FormControl };
  get formMedicalAppointmentDate(): FormControl { return this.medicalAppointmentForm.get("date") as FormControl };
  get timeControl(): FormControl { return this.medicalAppointmentForm.get("time") as FormControl };

  getAllMedicalAppointments() {
    this.clinicService.getAllMedicalAppointmentsPatientsDoctors().subscribe(data => {
      this.medicalAppointments = data;
    })
  }

  addNewAppointment() {
    this.isAddNewAppointmentVisible = true;
  }

  openAppointmentForm() {
    this.isAddingMode = true;
  }

  resetForm() {
    this.medicalAppointmentForm.reset({
      date: null,
      time: null,
      doctorId: null
    });
  }

  addMedicalAppointment(): void {
    if (this.medicalAppointmentForm.invalid) {
      this.medicalAppointmentForm.markAllAsTouched();
      return;
    }
    const appointmentData = this.medicalAppointmentForm.getRawValue();
    this.clinicService.addMedicalAppointment(appointmentData).subscribe({
      next: (result: MedicalAppointment) => {
        this.medicalAppointment = result;
        if (this.isShowingAllAppointmentsMode == true) {
          this.getAllMedicalAppointments();
        }
        console.log('Wizyta została utworzona:', result);
        this.resetForm();
      },
      error: (err) => {
        console.error('Wystąpił błąd podczas tworzenia wizyty:', err);
      },
    });
  }

  cancelAdding() {
    this.isAddingMode = false;
    this.medicalAppointmentForm.reset();
    this.resetForm();
  }

  showAllAppointments() {
    this.getAllMedicalAppointments();
    this.isShowingAllAppointmentsMode = true;
  }

  closeAllAppointments() {
    this.isShowingAllAppointmentsMode = false;
  }

  getAllDoctors() {
    this.clinicService.getAllAvailableDoctors().subscribe(data => {
      this.doctors = data;
    })
  }

  delete(medicalAppointmentId: number) {
    this.clinicService.deleteMedicalAppointment(medicalAppointmentId)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllMedicalAppointments();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });
  }
}