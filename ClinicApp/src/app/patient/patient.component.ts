import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
//import { MakeAnAppointmentComponent } from '../make-an-appointment/make-an-appointment.component';
import { Patient } from '../model/patient';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
import { Specialisation } from '../model/specialisation';
import { MedicalAppointment } from '../model/medical-appointment';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';
import { MedicalAppointmentPatientDoctorDto } from '../dtos/medical-appointment-patient-doctor-dto';

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
  //choosePatientForm: FormGroup;
  patient: Patient;
  patients: Patient[] = [];
  isPatientIdSet: boolean = this.patientId !== 0;
  medicalAppointments: ReturnMedicalAppointment[] = [];
  selectedAppointment: ReturnMedicalAppointment;
  specialisations: Specialisation[] = [];
  chooseSpecialisationForm: FormGroup;
  isDisabled = true;
  selectedSpecialisation: number;
  isMakeAnAppointmentMode: boolean = false;
  isRegistrantMode: boolean = false; 
  //allMedicalAppointments: AllMedicalAppointments;
  futureAppointments: MedicalAppointmentPatientDoctorDto[] = [];
  pastAppointments: MedicalAppointmentPatientDoctorDto[] = [];
  isVisible = false;
  selectedPatientId: number = 0;
  registrantId: number = 0;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, 
              private route: ActivatedRoute, private clinicService: ClinicService, public authorizationService: AuthorizationService,) {
    //this.choosePatientForm = this.formBuilder.group({});
    this.chooseSpecialisationForm = this.formBuilder.group({});
    this.selectedSpecialisation = 0;
    this.selectedAppointment = { id: 0, doctorId: 0, patientId: 0, interview: '', diagnosis: '', diseaseUnit: 0, dateTime: new Date() }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    //this.allMedicalAppointments = { pastMedicalAppointments: [], futureMedicalAppointments: [] }
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
      this.isDisabled = !value; // Ustawienie isEnabled na true, jeśli wartość jest wybrana
      console.log('Specialisation selected:', value, 'isDisabled:', this.isDisabled);
    });
    console.log('pacjent id: ', this.patientId);

    const userId = this.authorizationService.getUserId(); //NIEPOTRZEBNE, BO BACKEND SAM ODCZYTUJE...

    //this.getAllMedicalAppointments()
    /*this.choosePatientForm = this.formBuilder.group({
      patientId: new FormControl(null, { validators: [Validators.required] })
    });*/
  }


  get formSpecialisationId(): FormControl { return this.chooseSpecialisationForm?.get("specialisationId") as FormControl };
  //get formPatientId(): FormControl { return this.choosePatientForm?.get("patientId") as FormControl };


  getAllSpecialisations() {
    //this.clinicService.getAllSpecialisations().subscribe(data => {
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

  openPastAppointments(){
    this.isPastAppointmentsMode = true;
    this.getPastAppointments();
  }

  getPatientById(patientId: number){
    this.clinicService.getPatientById(patientId).subscribe(data => {
      this.patient = data;
    })
  }

  /*getAllMedicalAppointments() {
    this.clinicService.getMedicalAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.allMedicalAppointments = data;
      console.log(this.allMedicalAppointments.pastMedicalAppointments);
      console.log(this.allMedicalAppointments.pastMedicalAppointments.length);
    })
  } */

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
    this.setPatientToAppointment(this.selectedAppointment);
    this.isMakeAnAppointmentMode = false;
    this.medicalAppointments.length = 0;//czyszczenie listy żeby nie było widać starych wyszukiwań
  }

  setPatientToAppointment(selectedAppointment: ReturnMedicalAppointment) {
    this.clinicService.editMedicalAppointmentReturnDto(this.selectedAppointment)
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

  /*logout(){
    this.authorizationService.logout();
  } */
}