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
    patientId: number = 0; //ok
    patient: Patient; //ok
    plannedLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
    inProcessLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = [];
    finishedLaboratoryAppointments: LabAppWithPatientLabTestsMedApp[] = []; //ok
    comissionedLaboratoryTests: GroupWithLabTests[] = []; //ok
    availableLaboratoryAppointments: LaboratoryAppointment[] = []; //ok
    testsGroupToArrange: GroupWithLabTests; //undefined moze byc na poczatku?
    selectedAppointment: LaboratoryAppointment;
    laboratoryAppointmentState = LaboratoryAppointmentState;
    isCommisionedTestsMode: boolean = false;
    isPlannedLabAppMode: boolean = false;
    isInProcessLabAppMode:boolean = false;
    isFinishedLabappMode: boolean = false;

    //chooseLaboratoryAppointmentForm: FormGroup;

    
    isPatientIdSet: boolean = this.patientId !== 0;
    //medicalAppointments: ReturnMedicalAppointment[] = [];
    //selectedAppointment: ReturnMedicalAppointment;
    specialisations: Specialisation[] = [];
    isDisabled = true;
    isMakeAnAppointmentMode: boolean = false;
    isRegistrantMode: boolean = false; 
    //allMedicalAppointments: AllMedicalAppointments;
    isVisible = false;
    selectedPatientId: number = 0;
    registrantId: number = 0;
  
    constructor(private http: HttpClient, private formBuilder: FormBuilder, 
                private route: ActivatedRoute, private clinicService: ClinicService, public authorizationService: AuthorizationService,) {
      //this.choosePatientForm = this.formBuilder.group({});
      //this.selectedAppointment = { id: 0, doctorId: 0, patientId: 0, interview: '', diagnosis: '', diseaseUnit: 0, dateTime: new Date() }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
      //this.allMedicalAppointments = { pastMedicalAppointments: [], futureMedicalAppointments: [] }
      this.patient = {name: '', surname: '', id: 0, pesel: '', patientNumber: '', isAvailable: true};
      this.testsGroupToArrange = {groupId: 0, laboratoryTests: []}
      this.selectedAppointment = { id: 0, laboratoryWorkerId: 0, supervisorId: 0, state: LaboratoryAppointmentState.Empty , dateTime: new Date(), cancelComment: ''}; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    }
  
    ngOnInit() {
      this.route.params.subscribe(params => {
        this.patientId = +params['patientId']; // Przypisanie id z URL
        this.registrantId = +params['registrantId']; 
        console.log('Received patientId:', this.patientId);
        this.getPatientById(this.patientId);
      });
  
      this.route.queryParams.subscribe(queryParams => {
        this.isRegistrantMode = queryParams['isRegistrantMode'] === 'true';
        console.log('Is registrant mode po params:', this.isRegistrantMode);
      });  
      //this.getComissionedLaboratoryTests();
      //this.getFinishedLaboratoryAppointmentsByPatientId();
      //this.getPlannedLaboratoryAppointments();
      for(var comissionedLaboratoryTest of this.comissionedLaboratoryTests){
        console.log("Grupa id");
        console.log(comissionedLaboratoryTest.groupId);
        for(var laboratoryTest of comissionedLaboratoryTest.laboratoryTests){
          console.log("Nazwa testu:")
          console.log(laboratoryTest.laboratoryTestTypeName)
        }
      }  
  
      //this.getAllLaboratoryAppointments() //odpalić dopieor po naciśnięciu przycisku
    }

    chooseLaboratoryAppointment(laboratoryAppointmentId: number){
      this.clinicService.setLaboratoryAppointmentToTestsGroup(this.testsGroupToArrange.groupId, laboratoryAppointmentId)
      .subscribe(data => {
        //this.availableLaboratoryAppointments = data;
        this.getComissionedLaboratoryTests();
        if(this.isPlannedLabAppMode == true){
          this.getPlannedLaboratoryAppointments();
        }
        if(this.isMakeAnAppointmentMode == true){
          this.getAllAvailableLaboratoryAppointments();
        }
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

    reserve(laboratoryTestsGroup: GroupWithLabTests){
      this.testsGroupToArrange = laboratoryTestsGroup
      this.isMakeAnAppointmentMode = true;
      this.getAllAvailableLaboratoryAppointments();
    }

    cancelAppointmentForm(){
      this.isMakeAnAppointmentMode = false;
    }

    openComissioned(){
      this.isCommisionedTestsMode = true;
      this.getComissionedLaboratoryTests();
    }

    openPlanned(){
      this.isPlannedLabAppMode = true;
      this.getPlannedLaboratoryAppointments();
    }

    openInProcess(){
      this.isInProcessLabAppMode = true;
      this.getInProcessLaboratoryAppointmentsByPatientId();
    }

    openFinished(){
      this.isFinishedLabappMode = true;
      this.getFinishedLaboratoryAppointmentsByPatientId();
    }

    cancelPlannedAppointment(laboratoryAppointmentId: number){
      this.clinicService.cancelPlannedAppointment(laboratoryAppointmentId).subscribe(data => {
        this.getComissionedLaboratoryTests();
        this.getPlannedLaboratoryAppointments();
      })
    }

    closeComissionedAppointments(){
      this.isCommisionedTestsMode = false;
    }

    closePlannedAppointments(){
      this.isPlannedLabAppMode = false;     
    }

    closeInProcessAppointments(){
      this.isInProcessLabAppMode = false;     
    }

    closeFinishedAppointments(){
      this.isFinishedLabappMode = false;
    }

    
    getAllAvailableLaboratoryAppointments(){ //gdy chce się umówić 
      this.clinicService.getAllAvailableLaboratoryAppointments().subscribe(data => {
        this.availableLaboratoryAppointments = data;
      })
    }

    getPatientById(patientId: number){
      this.clinicService.getPatientById(patientId).subscribe(data => {
        this.patient = data;
      })
    }

    getComissionedLaboratoryTests(){
      this.clinicService.getComissionedLaboratoryTestsByPatientId(this.patientId).subscribe(data => {
        this.comissionedLaboratoryTests = data;
      })
      for(var comissionedLaboratoryTest of this.comissionedLaboratoryTests){
        console.log("Grupa id");
        console.log(comissionedLaboratoryTest.groupId);
        for(var laboratoryTest of comissionedLaboratoryTest.laboratoryTests){
          console.log("Nazwa testu:")
          console.log(laboratoryTest.laboratoryTestTypeName)
        }
      }    
    }

    getPlannedLaboratoryAppointments(){
      this.clinicService.getPlannedLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
        this.plannedLaboratoryAppointments = data;
        //console.log(this.allMedicalAppointments.pastMedicalAppointments);
        //console.log(this.allMedicalAppointments.pastMedicalAppointments.length);
      })
    }

    getInProcessLaboratoryAppointmentsByPatientId() { //gdy chce zobaczyc zrealizowane
      this.clinicService.getInProcessLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
        this.inProcessLaboratoryAppointments = data;
        //(error) => console.error('Error fetching appointments:', error)
      })
    }

    getFinishedLaboratoryAppointmentsByPatientId() { //gdy chce zobaczyc zrealizowane
      this.clinicService.getFinishedLaboratoryAppointmentsByPatientId(this.patientId).subscribe(data => {
        this.finishedLaboratoryAppointments = data;
        //(error) => console.error('Error fetching appointments:', error)
      })
    }

    

}
