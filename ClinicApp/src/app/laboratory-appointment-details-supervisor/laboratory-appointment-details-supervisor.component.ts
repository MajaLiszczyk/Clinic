import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormControl, Validators, FormArray, FormGroup } from '@angular/forms';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';
import { LaboratoryAppointmentState } from '../model/laboratory-appointment';
import { LaboratoryTestState } from '../model/laboratory-test';

@Component({
  selector: 'app-laboratory-appointment-details-supervisor',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, ReactiveFormsModule],
  templateUrl: './laboratory-appointment-details-supervisor.component.html',
  styleUrl: './laboratory-appointment-details-supervisor.component.css'
})

export class LaboratoryAppointmentDetailsSupervisorComponent {
  appointmentId: number = 0;
  laboratoryAppointment?: LabAppWithPatientLabTestsMedApp;
  LaboratoryAppointmentState = LaboratoryAppointmentState;
  LaboratoryTestState = LaboratoryTestState;
  laboratoryTestsFormArray: FormArray;
  rejectCommentTestForm: FormGroup;
  isAllResultsChecked: boolean = false;

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router, private clinicService: ClinicService) {
    this.rejectCommentTestForm = this.fb.group({
      rejectComment: new FormControl(null),
    });
    this.laboratoryTestsFormArray = this.fb.array([]);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.appointmentId = +params['id'];
      console.log('Received appointmentId:', this.appointmentId);
      this.getLaboratoryAppointmentDetails();
    });
  }

  get formRejectComment(): FormControl { return this.rejectCommentTestForm.get('rejectComment') as FormControl; }

  isAllTestResultsChecked() {
    for (let labTest of this.laboratoryAppointment?.laboratoryTests!) {
      if (labTest.state != LaboratoryTestState.Accepted && labTest.state != LaboratoryTestState.Rejected) {
        this.isAllResultsChecked = false;
        return;
      }
    }
    this.isAllResultsChecked = true;
  }

  getLaboratoryAppointmentDetails() {
    this.clinicService.getLabAppDetailsByLabAppId(this.appointmentId).subscribe({
      next: (data) => {
        this.laboratoryAppointment = data;
        this.initializeLaboratoryTestsFormArray();
        this.isAllTestResultsChecked();
      },
      error: (error) => {
        if (error.status === 403) {
          alert("Forbidden access.");
        } else {
          console.error("Error:", error);
        }
      }
    });

  }

  initializeLaboratoryTestsFormArray() {
    if (this.laboratoryAppointment?.laboratoryTests) {
      this.laboratoryTestsFormArray.clear();
      this.laboratoryAppointment.laboratoryTests.forEach(test => {
        this.laboratoryTestsFormArray.push(this.fb.group({
          rejectComment: new FormControl(test.rejectComment),
        }));
      });
    }
  }

  getLaboratoryTestFormGroup(index: number): FormGroup {
    return this.laboratoryTestsFormArray.at(index) as FormGroup;
  }

  acceptLaboratoryTest(index: number) {
    const form = this.laboratoryTestsFormArray.at(index) as FormGroup;
    const rejectCommentValue = form.value.rejectComment;
    const testId = this.laboratoryAppointment?.laboratoryTests[index].id;

    if (testId !== undefined) {
      const updatePayload = { id: testId, rejectComment: rejectCommentValue };
      this.clinicService.acceptLaboratoryTest(testId)
        .subscribe({
          next: (response) => {
            console.log("Operation completed successfully:", response);
            console.log(`Test with ID ${testId} updated with result: ${rejectCommentValue}`);
            this.getLaboratoryAppointmentDetails();
          },
          error: (error) => {
            console.error("Error occurred:", error);
          }
        });
    }
  }

  rejectLaboratoryTest(index: number) {
    const form = this.laboratoryTestsFormArray.at(index) as FormGroup;
    const rejectCommentControl = form.get('rejectComment') as FormControl;
    if (!rejectCommentControl || rejectCommentControl.value === null || rejectCommentControl.value.trim() === '') {
      rejectCommentControl.markAsTouched();
      return;
    }
    const rejectCommentValue = form.value.rejectComment;
    const testId = this.laboratoryAppointment?.laboratoryTests[index].id;

    if (testId !== undefined) {
      const updatePayload = { id: testId, rejectComment: rejectCommentValue };
      this.clinicService.rejectLaboratoryTest(testId, rejectCommentValue)
        .subscribe({
          next: (response) => {
            console.log("Operation completed successfully:", response);
            console.log(`Test with ID ${testId} updated with result: ${rejectCommentValue}`);
            this.getLaboratoryAppointmentDetails();
          },
          error: (error) => {
            console.error("Error occurred:", error);
          }
        });
    }
  }

  sendToLaboratoryWorker() {
    for (let labTest of this.laboratoryAppointment?.laboratoryTests!) {
      if (labTest.state != LaboratoryTestState.Accepted && labTest.state != LaboratoryTestState.Rejected) {
        this.laboratoryTestsFormArray.markAllAsTouched();
        return;
      }
    }
    this.clinicService.sendLaboratoryTestsToLaboratoryWorker(this.laboratoryAppointment!.laboratoryAppointmentId)
      .subscribe({
        next: (response) => {
          this.router.navigate(['/laboratory-supervisor/' + this.laboratoryAppointment?.supervisorId, 0]);
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }
}
