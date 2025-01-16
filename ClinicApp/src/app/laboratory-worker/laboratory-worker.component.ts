import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-laboratory-worker',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './laboratory-worker.component.html',
  styleUrl: './laboratory-worker.component.css'
})
export class LaboratoryWorkerComponent {
  isRegistrantMode: boolean = false;
  laboratoryWorkerId: number = 0;
  isFutureLabAppMode: boolean = false;
  isWaitingForFillMode: boolean = false;
  isWaitingForSupervisorMode: boolean = false;
  isToBeFixedMode: boolean = false;
  isReadyForPatientMode: boolean = false;
  isSentToPatientMode: boolean = false;
  isCancelledLabAppMode: boolean = false;
  futureAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  waitingForFillAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  waitingForSupervisorAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  toBeFixedAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  readyForPatientAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  sentToPatientAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
  cancelledAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok  

  constructor(private route: ActivatedRoute, private clinicService: ClinicService
    , public authorizationService: AuthorizationService) { }

  ngOnInit(){
    this.route.params.subscribe(params => {
      this.laboratoryWorkerId = +params['laboratoryWorkerId']; // Przypisanie id z URL
      console.log('Received doctorId:', this.laboratoryWorkerId);
    });
    this.route.queryParams.subscribe(queryParams => {
      this.isRegistrantMode = queryParams['isRegistrantMode'] === 'true';
      console.log('Is registrant mode po params:', this.isRegistrantMode);
    });
    //this.getMedicalAppointmentsForDoctor()
  }

  getFutureLabAppsByLabWorkerId(){
    this.clinicService.getFutureLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.futureAppointments=data;
    })
  }

  getWaitingForFillLabAppsByLabWorkerId(){
    this.clinicService.getWaitingForFillLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.waitingForFillAppointments=data;
    })
  }

  getWaitingForSupervisorLabAppsByLabWorkerId(){
    this.clinicService.getWaitingForSupervisorLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.waitingForSupervisorAppointments=data;
    })
  }

  getToBeFixedLabAppsByLabWorkerId(){
    this.clinicService.getToBeFixedLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.toBeFixedAppointments=data;
    })
  }

  getReadyForPatientLabAppsByLabWorkerId(){
    this.clinicService.getReadyForPatientLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.readyForPatientAppointments=data;
    })
  }

  getSentToPatientLabAppsByLabWorkerId(){
    this.clinicService.getSentToPatientLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.sentToPatientAppointments=data;
    })
  }

  getCancelledLabAppsByLabWorkerId(){
    this.clinicService.getCancelledLabAppsByLabWorkerId(this.laboratoryWorkerId).subscribe(data =>{
      this.cancelledAppointments=data;
    })
  }

  logout(){
    this.authorizationService.logout();
  }


  openFutureLabApp() {
    this.isFutureLabAppMode = true;
    this.getFutureLabAppsByLabWorkerId();
  }

  closeFutureLabApp() {
    this.isFutureLabAppMode = false;
  }

  openWaitingForFillLabApp() {
    this.isWaitingForFillMode = true;
    this.getWaitingForFillLabAppsByLabWorkerId();
  }

  closeWaitingForFillLabApp() {
    this.isWaitingForFillMode = false;
  }

  openWaitingForSupervisorLabApp() {
    this.isWaitingForSupervisorMode = true;
    this.getWaitingForSupervisorLabAppsByLabWorkerId();
  }

  closeWaitingForSupervisorLabApp() {
    this.isWaitingForSupervisorMode = false;
  }
  openToBeFixedLabApp() {
    this.isToBeFixedMode = true;
    this.getToBeFixedLabAppsByLabWorkerId();
  }

  closeToBeFixedLabApp() {
    this.isToBeFixedMode = false;
  }
  openReadyForPatientLabApp() {
    this.isReadyForPatientMode = true;
    this.getReadyForPatientLabAppsByLabWorkerId();
  }

  closeReadyForPatientLabApp() {
    this.isReadyForPatientMode = false;
  }
  openSentToPatientLabApp() {
    this.isSentToPatientMode = true;
    this.getSentToPatientLabAppsByLabWorkerId();
  }

  closeSentToPatientLabApp() {
    this.isSentToPatientMode = false;
  }
  openCancelledLabApp() {
    this.isCancelledLabAppMode = true;
    this.getCancelledLabAppsByLabWorkerId();
  }

  closeCancelledLabApp() {
    this.isCancelledLabAppMode = false;
  }


}
