import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Patient } from '../model/patient';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
import { Specialisation } from '../model/specialisation';
import { MedicalAppointment } from '../model/medical-appointment';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';
import { MedicalAppointmentPatientDoctorDto } from '../dtos/medical-appointment-patient-doctor-dto';
import { MedicalAppointmentDoctorDto } from '../dtos/medical-appointment-doctor-dto';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.css'
})
export class PatientComponent {
  patientId: number = 0;
  isFutureAppointmentsMode: boolean = false;
  isPastAppointmentsMode: boolean = false;
  patient: Patient;
  patients: Patient[] = [];
  isPatientIdSet: boolean = this.patientId !== 0;
  medicalAppointments: MedicalAppointmentDoctorDto[] = [];
  selectedAppointment: MedicalAppointmentDoctorDto;  
  specialisations: Specialisation[] = [];
  chooseSpecialisationForm: FormGroup;
  isDisabled = true;
  selectedSpecialisation: number;
  isMakeAnAppointmentMode: boolean = false;
  isRegistrantMode: boolean = false; 
  futureAppointments: MedicalAppointmentPatientDoctorDto[] = [];
  pastAppointments: MedicalAppointmentPatientDoctorDto[] = [];
  isVisible = false;
  selectedPatientId: number = 0;
  registrantId: number = 0;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, 
              private route: ActivatedRoute, private clinicService: ClinicService, public authorizationService: AuthorizationService,) {
    this.chooseSpecialisationForm = this.formBuilder.group({});
    this.selectedSpecialisation = 0;
    this.selectedAppointment = { id: 0, doctorId: 0, doctorName: "", doctorSurname: "", patientId: 0, interview: '', diagnosis: '', diseaseUnit: 0
                                , dateTime: new Date(), isCancelled: false, isFinished: false, cancellingComment:""}; 
    this.patient = {name: '', surname: '', id: 0, pesel: '', patientNumber: '', isAvailable: true};
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

    this.getAllSpecialisations();
    this.chooseSpecialisationForm = this.formBuilder.group({
      specialisationId: new FormControl(null, { validators: [Validators.required] })
    });
    this.chooseSpecialisationForm.get('specialisationId')?.valueChanges.subscribe(value => {
      this.isDisabled = !value; 
      console.log('Specialisation selected:', value, 'isDisabled:', this.isDisabled);
    });
    console.log('pacjent id: ', this.patientId);
    const userId = this.authorizationService.getUserId(); 
  }


  get formSpecialisationId(): FormControl { return this.chooseSpecialisationForm?.get("specialisationId") as FormControl };

  getAllSpecialisations() {
    this.clinicService.getAllAvailableSpecialisations().subscribe(data => {     
      this.specialisations = data;
    })
  }

  getFutureAppointments(){
    this.clinicService.getFutureMedicalAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.futureAppointments = data;
    })
  }

  getPastAppointments(){
    this.clinicService.getPastMedicalAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.pastAppointments = data;
    })
  }

  openFutureAppointments(){
    this.isFutureAppointmentsMode = true;
    this.getFutureAppointments();
  }

  closeFutureAppointments(){
    this.isFutureAppointmentsMode = false;
  }

  openPastAppointments(){
    this.isPastAppointmentsMode = true;
    this.getPastAppointments();
  }

  closePastAppointments(){
    this.isPastAppointmentsMode = false;
  }

  getPatientById(patientId: number){
    this.clinicService.getPatientById(patientId).subscribe(data => {
      this.patient = data;
    })
  }

  openAppointmentForm(){
    this.isMakeAnAppointmentMode = true;
  }

  cancelAppointmentForm(){
    this.isMakeAnAppointmentMode = false;
    this.medicalAppointments.length = 0;
    this.chooseSpecialisationForm.reset();
  }

  cancel(medicalAppointment: MedicalAppointmentPatientDoctorDto) {
    medicalAppointment.patientId = 0;
    this.clinicService.editMedicalAppointment(medicalAppointment)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getFutureAppointments();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })
  }

  search() {
    this.selectedSpecialisation = this.formSpecialisationId.value;
    this.clinicService.getMedicalAppointmentsBySpecialisationId(this.selectedSpecialisation).subscribe(data => {
      this.medicalAppointments = data;
    })
  }

  chooseAppointment(medicalAppointmentId: number): void {
    console.log("Selected MedicalAppointment ID:", medicalAppointmentId);
    for (let i = 0; i < this.medicalAppointments.length; ++i) {
      if (this.medicalAppointments[i].id == medicalAppointmentId) {
        this.selectedAppointment = this.medicalAppointments[i];
        break;
      }
    }
    this.selectedAppointment.patientId = this.patientId;
    var medApp = this.makeMedicalAppointmentFromDto();
    this.setPatientToAppointment(medApp);
    this.isMakeAnAppointmentMode = false;
    this.medicalAppointments.length = 0;//czyszczenie listy żeby nie było widać starych wyszukiwań
  }

  makeMedicalAppointmentFromDto(): MedicalAppointment{
    var medApp: MedicalAppointment = {
      id: this.selectedAppointment.id,
      dateTime: this.selectedAppointment.dateTime,
      patientId: this.selectedAppointment.patientId,
      doctorId: this.selectedAppointment.doctorId,
      interview: this.selectedAppointment.interview,
      diagnosis: this.selectedAppointment.diagnosis,
      isFinished: this.selectedAppointment.isFinished,
      isCancelled: this.selectedAppointment.isCancelled,
      cancellingComment: this.selectedAppointment.cancellingComment
    }
    return medApp;
  }

  setPatientToAppointment(medApp: MedicalAppointment) {
    this.clinicService.editMedicalAppointmentReturnDto(medApp)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getFutureAppointments()
          this.chooseSpecialisationForm.reset();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });
  }
}