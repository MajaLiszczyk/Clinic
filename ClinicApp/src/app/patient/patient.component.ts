import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
//import { GetMedicalAppointmentsForPatientComponent } from '../get-medical-appointments-for-patient/get-medical-appointments-for-patient.component';
import { MakeAnAppointmentComponent } from '../make-an-appointment/make-an-appointment.component';
import { Patient } from '../model/patient';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
import { Specialisation } from '../model/specialisation';
import { MedicalAppointment } from '../model/medical-appointment';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ClinicService } from '../services/clinic.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [MakeAnAppointmentComponent,
    HttpClientModule, ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.css'
})
export class PatientComponent {
  patientId: number = 0;
  choosePatientForm: FormGroup;
  patient: Patient;
  patients: Patient[] = [];
  isPatientIdSet: boolean = this.patientId !== 0;
  //historyPanel: boolean = false;
  //readonly APIUrl = "https://localhost:5001/api/MedicalAppointment";
  medicalAppointments: ReturnMedicalAppointment[] = [];
  selectedAppointment: ReturnMedicalAppointment;
  specialisations: Specialisation[] = [];
  chooseSpecialisationForm: FormGroup;
  isDisabled = true;
  selectedSpecialisation: number;
  isMakeAnAppointmentMode: boolean = false;
  isRegistrantMode: boolean = false; 

  //get all
  allMedicalAppointments: AllMedicalAppointments;
  isVisible = false;
  //atients: Patient[] = [];
  selectedPatientId: number = 0;
  //choosePatientForm: FormGroup;
  //activeComponent: string = ''; // Nazwa aktywnego komponentu dziecka

  // Metoda do ustawienia aktywnego komponentu
  /*setActiveComponent(componentName: string): void {
    this.activeComponent = componentName;
  }*/

  constructor(private http: HttpClient, private formBuilder: FormBuilder, 
              private route: ActivatedRoute, private clinicService: ClinicService, public authorizationService: AuthorizationService,) {
    this.choosePatientForm = this.formBuilder.group({});
    this.chooseSpecialisationForm = this.formBuilder.group({});
    this.selectedSpecialisation = 0;
    this.selectedAppointment = { id: 0, doctorId: 0, patientId: 0, interview: '', diagnosis: '', diseaseUnit: 0, dateTime: new Date() }; //wymaga, bo - "Property 'doctor' has no initializer and is not definitely assigned in the constructor."
    this.allMedicalAppointments = { pastMedicalAppointments: [], futureMedicalAppointments: [] }
    this.patient = {name: '', surname: '', id: 0, pesel: '', patientNumber: '', isAvailable: true};
  }

  ngOnInit() {
    //jesli w trybie pacjenta - już nie jest 0 :
    //this.patientId = //z logowania
    //this.historyPanel = false;
    this.route.params.subscribe(params => {
      this.patientId = +params['patientId']; // Przypisanie id z URL
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

    this.getAllMedicalAppointments()
    this.choosePatientForm = this.formBuilder.group({
      patientId: new FormControl(null, { validators: [Validators.required] })
    });
  }

  get formSpecialisationId(): FormControl { return this.chooseSpecialisationForm?.get("specialisationId") as FormControl };
  get formPatientId(): FormControl { return this.choosePatientForm?.get("patientId") as FormControl };


  getAllSpecialisations() {
    //this.http.get<Specialisation[]>("https://localhost:5001/api/medicalSpecialisation/Get").subscribe(data => {
    this.clinicService.getAllSpecialisations().subscribe(data => {
      this.specialisations = data;
    })
  }

  getPatientById(patientId: number){
    this.clinicService.getPatientById(patientId).subscribe(data => {
      this.patient = data;
    })
  }

  //getAllMedicalAppointments(patientId: number) {
  getAllMedicalAppointments() {
    //this.clinicService.getMedicalAppointmentsByPatientId(patientId).subscribe(data => {
    this.clinicService.getMedicalAppointmentsByPatientId(this.patientId).subscribe(data => {
      this.allMedicalAppointments = data;
      console.log(this.allMedicalAppointments.pastMedicalAppointments);
      console.log(this.allMedicalAppointments.pastMedicalAppointments.length);
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

  cancel(medicalAppointment: MedicalAppointment) {
    medicalAppointment.patientId = 0;
    //this.http.put<MedicalAppointment>(this.APIUrl + "/update", medicalAppointment)
    this.clinicService.editMedicalAppointment(medicalAppointment)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllMedicalAppointments();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      })

  }

  search() {
    this.selectedSpecialisation = this.formSpecialisationId.value;
    //this.http.get<ReturnMedicalAppointment[]>(this.APIUrl + "/GetBySpecialisation/" + this.selectedSpecialisation).subscribe(data => {
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
    //this.http.put<MedicalAppointment>(this.APIUrl + "/update", this.selectedAppointment)
    this.clinicService.editMedicalAppointmentReturnDto(this.selectedAppointment)
      .subscribe({
        next: (response) => {
          console.log("Action performed successfully:", response);
          this.getAllMedicalAppointments()
          this.chooseSpecialisationForm.reset();
        },
        error: (error) => {
          console.error("Error performing action:", error);
        }
      });
  }

  logout(){
    this.authorizationService.logout();
  }

  /*onPatientChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.patientId = parseInt(selectElement.value, 10); // Pobranie wybranego ID pacjenta
    console.log('Selected Patient ID:', this.patientId);
  } */

}
