import { Component } from '@angular/core';
import { LaboratoryWorker } from '../model/laboratory-worker';
import { LaboratorySupervisor } from '../model/laboratory-supervisor';
import { LaboratoryAppointment, LaboratoryAppointmentState } from '../model/laboratory-appointment';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, AbstractControl, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgbModule, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { ClinicService } from '../services/clinic.service';
import { CreateMedicalAppointment } from '../model/create-medical-appointment';
import { MedicalAppointment } from '../model/medical-appointment';
import { CreateLaboratoryAppointment } from '../model/create-laboratory-appointment';
import { LaboratoryAppointmentWorkerSupervisor } from '../dtos/laboratory-appointment-worker-supervisor';


@Component({
  selector: 'app-registrant-laboratory-appointments',
  standalone: true,
  imports: [RouterLink, CommonModule, ReactiveFormsModule, NgbModule],
  templateUrl: './registrant-laboratory-appointments.component.html',
  styleUrl: './registrant-laboratory-appointments.component.css',
  providers: [NgbDateNativeAdapter]
})
export class RegistrantLaboratoryAppointmentsComponent {
  laboratoryAppointmentState = LaboratoryAppointmentState;
  isAddNewAppointmentVisible: boolean = false;
  laboratoryWorkers: LaboratoryWorker[] = [];
  laboratorySupervisors: LaboratorySupervisor[] = [];
  laboratoryAppointment: LaboratoryAppointment;
  laboratoryAppointmentForm: FormGroup;
  isDisable = false;
  laboratoryAppointments: LaboratoryAppointmentWorkerSupervisor[] = [];
  isAddingMode: boolean = false;
  isShowingAllAppointmentsMode: boolean = false;
  registrantId: number = 0;

  constructor(private route: ActivatedRoute, private http: HttpClient, private formBuilder: FormBuilder,
    private adapter: NgbDateNativeAdapter, private clinicService: ClinicService) {
    this.laboratoryAppointmentForm = this.formBuilder.group({});
    this.laboratoryAppointment = {
      id: 0, dateTime: new Date(), laboratoryWorkerId: 0, supervisorId: 0, state: LaboratoryAppointmentState.Empty, cancelComment: ''
    };
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.registrantId = +params['registrantId'];
    });
    this.getAllAvailableLaboratoryWorkers();
    this.getAllAvailableSupervisors();
    this.laboratoryAppointmentForm = this.formBuilder.group({
      date: new FormControl(null, [Validators.required, this.futureOrTodayDateValidator]),
      time: new FormControl(null, [Validators.required]),
      laboratoryWorkerId: new FormControl(null, [Validators.required]),
      supervisorId: new FormControl(null, [Validators.required])
    }, {
      validators: this.dateTimeValidator
    });
  }

  get formLaboratoryWorkerId(): FormControl { return this.laboratoryAppointmentForm?.get("laboratoryWorkerId") as FormControl };
  get formSupervisorId(): FormControl { return this.laboratoryAppointmentForm?.get("supervisorId") as FormControl };
  get formMedicalAppointmentDate(): FormControl { return this.laboratoryAppointmentForm.get("date") as FormControl };
  get timeControl(): FormControl { return this.laboratoryAppointmentForm.get("time") as FormControl };

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

  getAllLaboratoryAppointments() {
    this.clinicService.getAllLaboratoryAppointmentsWorkersSupervisors().subscribe(data => {
      this.laboratoryAppointments = data;
    })
  }

  addNewAppointment() {
    this.isAddNewAppointmentVisible = true;
  }

  openAppointmentForm() {
    this.isAddingMode = true;
  }

  resetForm() {
    this.laboratoryAppointmentForm.reset({
      date: null,
      time: null,
      doctorId: null
    });
  }

  addLaboratoryAppointment(): void {
    if (this.laboratoryAppointmentForm.invalid) {
      this.laboratoryAppointmentForm.markAllAsTouched();
      return;
    }
    const appointmentData = this.laboratoryAppointmentForm.getRawValue();
    this.clinicService.addLaboratoryAppointment(appointmentData).subscribe({
      next: (result: LaboratoryAppointment) => {
        this.laboratoryAppointment = result;
        if (this.isShowingAllAppointmentsMode == true) {
          this.getAllLaboratoryAppointments();
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
    this.laboratoryAppointmentForm.reset();
    this.resetForm();
  }

  showAllAppointments() {
    this.getAllLaboratoryAppointments();
    this.isShowingAllAppointmentsMode = true;
  }

  closeAllAppointments() {
    this.isShowingAllAppointmentsMode = false;
  }

  getAllAvailableSupervisors() {
    this.clinicService.getAllAvailableLaboratorySupervisors().subscribe(data => {
      this.laboratorySupervisors = data;
    })
  }

  getAllAvailableLaboratoryWorkers() {
    this.clinicService.getAllAvailableLaboratoryWorkers().subscribe(data => {
      this.laboratoryWorkers = data;
    })
  }

  edit(laboratoryAppointment: LaboratoryAppointment) {
    this.clinicService.editLaboratoryAppointment(laboratoryAppointment)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })
  }

  delete(laboratoryAppointmentId: number) {
    this.clinicService.deleteLaboratoryAppointment(laboratoryAppointmentId)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllLaboratoryAppointments();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });
  }
}
