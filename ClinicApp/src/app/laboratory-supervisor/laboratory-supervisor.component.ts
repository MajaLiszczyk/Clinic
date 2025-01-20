import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';

@Component({
  selector: 'app-laboratory-supervisor',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './laboratory-supervisor.component.html',
  styleUrl: './laboratory-supervisor.component.css'
})
export class LaboratorySupervisorComponent {
  isRegistrantMode: boolean = false;
  laboratorySupervisorId: number = 0;
  isWaitingForReviewMode: boolean = false;
  isSentBackMode: boolean = false;
  isAcceptedMode: boolean = false;
  waitingForReviewAppointments: LabAppWithPatientLabTestsMedApp[] = [];
  sentBackAppointments: LabAppWithPatientLabTestsMedApp[] = [];
  acceptedAppointments: LabAppWithPatientLabTestsMedApp[] = [];

  constructor(private route: ActivatedRoute, private clinicService: ClinicService
      , public authorizationService: AuthorizationService) { }
  
    ngOnInit(){
      this.route.params.subscribe(params => {
        this.laboratorySupervisorId = +params['laboratorySupervisorId']; // Przypisanie id z URL
        console.log('Received laboratorySupervisorId:', this.laboratorySupervisorId);
      });
      this.route.queryParams.subscribe(queryParams => {
        this.isRegistrantMode = queryParams['isRegistrantMode'] === 'true';
        console.log('Is registrant mode po params:', this.isRegistrantMode);
      });
      //this.getMedicalAppointmentsForDoctor()
    }

    logout(){
      this.authorizationService.logout();
    }
    openWaitingForReviewLabApp(){
      this.isWaitingForReviewMode = true;
      this.getWaitingForReviewLabAppBySupervisorId();
    }
    closeWaitingForReviewLabApp(){
      this.isWaitingForReviewMode = false;
    }
    openSentBackLabApp(){
      this.isSentBackMode = true;
      this.getSentBackLabAppBySupervisorId();
    }
    closeSentBackLabApp(){
      this.isSentBackMode = false;
    }
    openAcceptedLabApp(){
      this.isAcceptedMode = true;
      this.getAcceptedLabAppBySupervisorId();
    }
    closeAcceptedLabApp(){
      this.isAcceptedMode = false;
    }

    getWaitingForReviewLabAppBySupervisorId(){
      this.clinicService.getWaitingForReviewLabAppsBySupervisorId(this.laboratorySupervisorId).subscribe(data =>{
        this.waitingForReviewAppointments=data;
      })
    }
    getSentBackLabAppBySupervisorId(){
      this.clinicService.getSentBackLabAppsBySupervisorId(this.laboratorySupervisorId).subscribe(data =>{
        this.sentBackAppointments=data;
      })

    }
    getAcceptedLabAppBySupervisorId(){
      this.clinicService.getAcceptedLabAppsBySupervisorId(this.laboratorySupervisorId).subscribe(data =>{
        this.acceptedAppointments=data;
      })

    }

  

    

}
