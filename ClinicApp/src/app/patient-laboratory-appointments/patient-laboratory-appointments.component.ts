import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { Patient } from '../model/patient';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
import { Specialisation } from '../model/specialisation';
import { AuthorizationService } from '../services/authorization.service';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';
import { GroupWithLabTests } from '../dtos/group-labTests-dto';
import { CommonModule } from '@angular/common';
import { LaboratoryAppointment, LaboratoryAppointmentState } from '../model/laboratory-appointment';

@Component({
  selector: 'app-patient-laboratory-appointments',
  standalone: true,
  imports: [RouterLink, HttpClientModule, ReactiveFormsModule, CommonModule,],
  templateUrl: './patient-laboratory-appointments.component.html',
  styleUrl: './patient-laboratory-appointments.component.css'
})
export class PatientLaboratoryAppointmentsComponent {
  patientId: number = 0;
  patient: Patient;
  plannedLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = [];
  inProcessLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = [];
  finishedLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = [];
  comissionedLaboratoryTests: GroupWithLabTests[] = [];
  availableLaboratoryAppointments: LaboratoryAppointment[] = [];
  testsGroupToArrange: GroupWithLabTests;
  selectedAppointment: LaboratoryAppointment;
  laboratoryAppointmentState = LaboratoryAppointmentState;
  isCommisionedTestsMode: boolean = false;
  isPlannedLabAppMode: boolean = false;
  isInProcessLabAppMode: boolean = false;
  isFinishedLabappMode: boolean = false;

  isPatientIdSet: boolean = this.patientId !== 0;
  specialisations: Specialisation[] = [];
  isDisabled = true;
  isMakeAnAppointmentMode: boolean = false;
  isRegistrantMode: boolean = false;
  isVisible = false;
  selectedPatientId: number = 0;
  registrantId: number = 0;

  constructor(private http: HttpClient, private formBuilder: FormBuilder,
    private route: ActivatedRoute, private clinicService: ClinicService, public authorizationService: AuthorizationService,) {
    this.patient = { name: '', surname: '', id: 0, pesel: '', patientNumber: '', isAvailable: true };
    this.testsGroupToArrange = { groupId: 0, laboratoryTests: [] }
    this.selectedAppointment = { id: 0, laboratoryWorkerId: 0, supervisorId: 0, state: LaboratoryAppointmentState.Empty, dateTime: new Date(), cancelComment: '' };
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.patientId = +params['patientId'];
      this.registrantId = +params['registrantId'];
      console.log('Received patientId:', this.patientId);
      this.getPatientById(this.patientId);
    });

    this.route.queryParams.subscribe(queryParams => {
      this.isRegistrantMode = queryParams['isRegistrantMode'] === 'true';
      console.log('Is registrant mode po params:', this.isRegistrantMode);
    });

    for (var comissionedLaboratoryTest of this.comissionedLaboratoryTests) {
      console.log("Grupa id");
      console.log(comissionedLaboratoryTest.groupId);
      for (var laboratoryTest of comissionedLaboratoryTest.laboratoryTests) {
        console.log("Nazwa testu:")
        console.log(laboratoryTest.laboratoryTestTypeName)
      }
    }
  }

  chooseLaboratoryAppointment(laboratoryAppointmentId: number) {
    this.clinicService.setLaboratoryAppointmentToTestsGroup(this.testsGroupToArrange.groupId, laboratoryAppointmentId)
      .subscribe(data => {
        this.getComissionedLaboratoryTests();
        if (this.isPlannedLabAppMode == true) {
          this.getPlannedLaboratoryAppointments();
        }
        this.isMakeAnAppointmentMode = false;
      })
  }

  chooseAppointment(medicalAppointmentId: number): void {
    for (let i = 0; i < this.availableLaboratoryAppointments.length; ++i) {
      if (this.availableLaboratoryAppointments[i].id == medicalAppointmentId) {
        this.selectedAppointment = this.availableLaboratoryAppointments[i];
        break;
      }
    }
    this.chooseLaboratoryAppointment(this.selectedAppointment.id)
    this.isMakeAnAppointmentMode = false;
    this.availableLaboratoryAppointments.length = 0;//czyszczenie listy żeby nie było widać starych wyszukiwań
  }

  reserve(laboratoryTestsGroup: GroupWithLabTests) {
    this.testsGroupToArrange = laboratoryTestsGroup
    this.isMakeAnAppointmentMode = true;
    this.getAllAvailableLaboratoryAppointments();
  }

  cancelAppointmentForm() {
    this.isMakeAnAppointmentMode = false;
  }

  openComissioned() {
    this.isCommisionedTestsMode = true;
    this.getComissionedLaboratoryTests();
  }

  openPlanned() {
    this.isPlannedLabAppMode = true;
    this.getPlannedLaboratoryAppointments();
  }

  openInProcess() {
    this.isInProcessLabAppMode = true;
    this.getInProcessLaboratoryAppointmentsByPatientId();
  }

  openFinished() {
    this.isFinishedLabappMode = true;
    this.getFinishedLaboratoryAppointmentsByPatientId();
  }

  cancelPlannedAppointment(laboratoryAppointmentId: number) {
    this.clinicService.cancelPlannedAppointment(laboratoryAppointmentId).subscribe(data => {
      this.getComissionedLaboratoryTests();
      this.getPlannedLaboratoryAppointments();
    })
  }

  closeComissionedAppointments() {
    this.isCommisionedTestsMode = false;
  }

  closePlannedAppointments() {
    this.isPlannedLabAppMode = false;
  }

  closeInProcessAppointments() {
    this.isInProcessLabAppMode = false;
  }

  closeFinishedAppointments() {
    this.isFinishedLabappMode = false;
  }

  getAllAvailableLaboratoryAppointments() {
    this.clinicService.getAllAvailableLaboratoryAppointments().subscribe(data => {
      this.availableLaboratoryAppointments = data;
    })
  }

  getPatientById(patientId: number) {
    this.clinicService.getPatientById(patientId).subscribe(data => {
      this.patient = data;
    })
  }

  getComissionedLaboratoryTests() {
    this.clinicService.getComissionedLaboratoryTestsByPatientId(this.patientId).subscribe(data => {
      this.comissionedLaboratoryTests = data;
    })
    for (var comissionedLaboratoryTest of this.comissionedLaboratoryTests) {
      console.log("Grupa id");
      console.log(comissionedLaboratoryTest.groupId);
      for (var laboratoryTest of comissionedLaboratoryTest.laboratoryTests) {
        console.log("Nazwa testu:")
        console.log(laboratoryTest.laboratoryTestTypeName)
      }
    }
  }

  getPlannedLaboratoryAppointments() {
    this.clinicService.getPlannedLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.plannedLaboratoryAppointments = data;
    })
  }

  getInProcessLaboratoryAppointmentsByPatientId() {
    this.clinicService.getInProcessLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.inProcessLaboratoryAppointments = data;
    })
  }

  getFinishedLaboratoryAppointmentsByPatientId() {
    this.clinicService.getFinishedLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.finishedLaboratoryAppointments = data;
    })
  }
}
