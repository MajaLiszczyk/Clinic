import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';
import { LaboratoryAppointmentState } from '../model/laboratory-appointment';
import { LaboratoryTest, LaboratoryTestState } from '../model/laboratory-test';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-laboratory-appointment-details',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './laboratory-appointment-details.component.html',
  styleUrl: './laboratory-appointment-details.component.css'
})
export class LaboratoryAppointmentDetailsComponent {

  appointmentId: number = 0;
  laboratoryAppointment?: LabAppWithPatientLabTestsMedApp;
  LaboratoryAppointmentState = LaboratoryAppointmentState;
  LaboratoryTestState = LaboratoryTestState;
  isCancelClicked: boolean = false;
  cancelAppointmentForm: FormGroup;
  resultTestForm: FormGroup;
  laboratoryTestsFormArray: FormArray;
  isAllResultsCompleted: boolean = false;
  laboratoryTests: LaboratoryTest[] = [];

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router, private clinicService: ClinicService) {
    this.cancelAppointmentForm = this.fb.group({
      cancelComment: new FormControl(null, { validators: [Validators.required] }),
    });
    this.resultTestForm = this.fb.group({
      result: new FormControl(null, { validators: [Validators.required] }),
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

  getLaboratoryTestFormGroup(index: number): FormGroup {
    return this.laboratoryTestsFormArray.at(index) as FormGroup;
  }

  get formCancelComment(): FormControl { return this.cancelAppointmentForm.get('cancelComment') as FormControl; }

  getLaboratoryAppointmentDetails() {
    this.clinicService.getLabAppDetailsByLabAppId(this.appointmentId).subscribe(data => {
      this.laboratoryAppointment = data;
      this.initializeLaboratoryTestsFormArray();
      this.isAllTestResultsCompleted();
    })
  }

  initializeLaboratoryTestsFormArray() {
    if (this.laboratoryAppointment?.laboratoryTests) {
      this.laboratoryTestsFormArray.clear();
      this.laboratoryAppointment.laboratoryTests.forEach(test => {
        this.laboratoryTestsFormArray.push(this.fb.group({
          result: new FormControl(test.result, [Validators.required]),
        }));
      });
    }
  }

  saveLaboratoryTest(index: number) {
    const form = this.laboratoryTestsFormArray.at(index) as FormGroup;
    if (form.invalid) {
      this.laboratoryTestsFormArray.at(index) as FormGroup;
      return;
    }
    const resultValue = form.value.result;

    const testId = this.laboratoryAppointment?.laboratoryTests[index].id;

    if (testId !== undefined) {
      const updatePayload = { id: testId, result: resultValue };
      this.clinicService.saveLaboratoryTestResult(testId, resultValue)
        .subscribe({
          next: (response) => {
            console.log("Operation completed successfully:", response);
            console.log(`Test with ID ${testId} updated with result: ${resultValue}`);
            this.getLaboratoryAppointmentDetails(); // Odśwież dane po zapisie
          },
          error: (error) => {
            console.error("Error occurred:", error);
          }
        });
    }
  }

  saveCancelComment() {
    if (this.cancelAppointmentForm.invalid) {
      this.cancelAppointmentForm.markAllAsTouched();
      return;
    }

    this.laboratoryAppointment!.cancelComment = this.cancelAppointmentForm.get('cancelComment')?.value || null;
    this.laboratoryAppointment!.state = LaboratoryAppointmentState.Cancelled;

    this.clinicService.makeCancelledLaboratoryAppointment(this.laboratoryAppointment!.laboratoryAppointmentId
      , this.cancelAppointmentForm.get('cancelComment')?.value || null)
      .subscribe({
        next: (response) => {
          console.log("Operation completed successfully:", response);
          this.router.navigate(['/laboratory-worker/' + this.laboratoryAppointment?.laboratoryWorkerId]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }

  cancelCancelComment() {
    this.isCancelClicked = false;
  }

  finishAppointment() {
    this.clinicService.finishLaboratoryAppointment(this.laboratoryAppointment!.laboratoryAppointmentId)
      .subscribe({
        next: (response) => {
          this.router.navigate(['/laboratory-worker/' + this.laboratoryAppointment?.laboratoryWorkerId, 0]); //ID MOŻE Z SESJI?
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });

  }
  cancelAppointment() {
    this.isCancelClicked = true;
  }

  isAllTestResultsCompleted() {
    for (let labTest of this.laboratoryAppointment?.laboratoryTests!) {
      if (labTest.state != LaboratoryTestState.Completed && labTest.state != LaboratoryTestState.Rejected
        && labTest.state != LaboratoryTestState.Accepted) {
        this.isAllResultsCompleted = false;
        return;
      }
    }
    this.isAllResultsCompleted = true;
  }

  sendToSupervisor() {
    for (let labTest of this.laboratoryAppointment?.laboratoryTests!) {
      if (labTest.result == null) {
        this.laboratoryTestsFormArray.markAllAsTouched();
        return;
      }
    }
    this.clinicService.sendLaboratoryTestsToSupervisor(this.laboratoryAppointment!.laboratoryAppointmentId)
      .subscribe({
        next: (response) => {
          this.router.navigate(['/laboratory-worker/' + this.laboratoryAppointment?.laboratoryWorkerId, 0]);
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }

  sendToPatient() {
    for (let labTest of this.laboratoryAppointment?.laboratoryTests!) {
      if (labTest.state != this.LaboratoryTestState.Accepted) {
        this.cancelAppointmentForm.markAllAsTouched();
        return;
      }
    }
    this.clinicService.sendLaboratoryTestsResultsToPatient(this.laboratoryAppointment!.laboratoryAppointmentId)
      .subscribe({
        next: (response) => {
          this.router.navigate(['/laboratory-worker/' + this.laboratoryAppointment?.laboratoryWorkerId, 0]);
        },
        error: (error) => {
          console.error("Error occurred:", error);
        }
      });
  }
}
