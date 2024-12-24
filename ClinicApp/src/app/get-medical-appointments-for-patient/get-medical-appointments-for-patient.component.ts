import { Component, Input } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup , Validators, FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MedicalAppointment } from '../model/medical-appointment';
import { Patient } from '../model/patient';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ClinicService } from '../services/clinic.service';


@Component({
  selector: 'app-get-medical-appointments-for-patient',
  standalone: true,
  imports: [HttpClientModule,  ReactiveFormsModule, CommonModule],
  templateUrl: './get-medical-appointments-for-patient.component.html',
  styleUrl: './get-medical-appointments-for-patient.component.css'
})
export class GetMedicalAppointmentsForPatientComponent {

  //readonly APIUrl="https://localhost:5001/api/MedicalAppointment";
  //pastMedicalAppointments: MedicalAppointment[] = [];
  //futureMedicalAppointments: MedicalAppointment[] = [];
  allMedicalAppointments: AllMedicalAppointments;
  isVisible = false;
  patients: Patient[]= [];
  selectedPatientId: number = 0;
  choosePatientForm: FormGroup;

  @Input()
  patientId: number = 0;



  constructor(private http:HttpClient, private formBuilder: FormBuilder, private clinicService: ClinicService){
    this.choosePatientForm = this.formBuilder.group({});
    this.allMedicalAppointments = {pastMedicalAppointments: [], futureMedicalAppointments: []}

  }
  
  ngOnInit(){
    this.getAllPatients();
    this.getAllMedicalAppointments(this.patientId)
    this.choosePatientForm = this.formBuilder.group({
      patientId: new FormControl(null, {validators: [Validators.required]})
    });
    //this.getAllMedicalAppointments();
  }

  get formPatientId(): FormControl {return this.choosePatientForm?.get("patientId") as FormControl};


  getAllPatients(){
    //this.http.get<Patient[]>("https://localhost:5001/api/patient/Get").subscribe(data =>{
    this.clinicService.getAllPatients().subscribe(data =>{
      this.patients=data;
    })
  }

  getAllMedicalAppointments(patientId:number){
    //this.http.get<AllMedicalAppointments>(this.APIUrl+"/GetByPatientId/" + patientId).subscribe(data =>{
    this.clinicService.getMedicalAppointmentsByPatientId(patientId).subscribe(data =>{
      this.allMedicalAppointments=data;
      console.log(this.allMedicalAppointments.pastMedicalAppointments);
      console.log(this.allMedicalAppointments.pastMedicalAppointments.length);
    })
  }

  /*showPatientAppointment(){
    this.selectedPatientId = this.formPatientId.value;
    this.http.get<ReturnMedicalAppointment[]>(this.APIUrl+"/GetBySpecialisation/" + this.selectedSpecialisation).subscribe(data =>{
      this.medicalAppointments=data;
    })   
  } */


  cancel(medicalAppointment: MedicalAppointment){
    medicalAppointment.patientId = 0;
    //this.http.put<MedicalAppointment>(this.APIUrl+"/update", medicalAppointment)
    this.clinicService.editMedicalAppointment(medicalAppointment)
    .subscribe({
      next: (response) => {
        console.log("Action performed successfully:", response);
        this.getAllMedicalAppointments(this.patientId);
      },
      error: (error) => {
        console.error("Error performing action:", error);
      }
    })

  }


}
