import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Doctor } from '../model/doctor';
import { MedicalAppointment } from '../model/medical-appointment';
import { DiagnosticTestType } from '../model/diagnostic-test-type';
import { Specialisation } from '../model/specialisation';
import { DoctorWithSpcecialisations } from '../model/doctor-with-specialisations';
import { Patient } from '../model/patient';
import { DiagnosticTest } from '../model/diagnostic-test';
import { AllMedicalAppointments } from '../model/all-medical-appointments';
import { ReturnMedicalAppointment } from '../model/return-medical-appointment';
//import { HttpClientModule } from '@angular/common/http'; // Import modułu


@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  //constructor() { }
  readonly DoctorUrl = "https://localhost:5001/api/doctor";
  readonly MedicalAppointmentUrl = "https://localhost:5001/api/MedicalAppointment";
  readonly DiagnosticTestTypeUrl = "https://localhost:5001/api/DiagnosticTestType";
  readonly SpecialisationUrl = "https://localhost:5001/api/medicalSpecialisation";
  readonly PatientUrl = "https://localhost:5001/api/patient";
  readonly DiagnosticTesttUrl = "https://localhost:5001/api/DiagnosticTest";

  



  constructor(private http: HttpClient) { }

  //PATIENT
  getAllPatients(): Observable<Patient[]>{
     return this.http.get<Patient[]>(this.PatientUrl+"/Get")
  }

  addPatient(patient: any): Observable<Patient> { //inny typ
    return this.http.post<Patient>(this.PatientUrl + "/create", patient)}

  updatePatient(patient: any): Observable<Patient>{ // inny typ
    return this.http.put<Patient>(this.PatientUrl+"/update", patient)
  }

  deletePatient(patientId: number): Observable<string>{
    return this.http.delete<string>(this.PatientUrl+"/Delete/"+patientId)}

  //DOCTOR
  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.DoctorUrl + "/Get");
  }

  getAllDoctorsWithSpecialisations(): Observable<DoctorWithSpcecialisations[]> {
    return this.http.get<DoctorWithSpcecialisations[]>(this.DoctorUrl + "/GetWithSpecialisations")
  }

  addDoctor(doctor: any): Observable<Doctor> { //inny typ
    return this.http.post<Doctor>(this.DoctorUrl + "/create", doctor);
  }

  updateDoctor(doctor: any): Observable<Doctor>{ //inny typ
    return this.http.put<Doctor>(this.DoctorUrl + "/update", doctor)
  }

  deleteDoctor(doctorId: number): Observable<string>{
    return this.http.delete<string>(this.DoctorUrl + "/Delete/" + doctorId)
  }

  //SPECIALISATIONS
  getAllSpecialisations(): Observable<Specialisation[]> {
    return this.http.get<Specialisation[]>(this.SpecialisationUrl+"/Get");
  }

  addSpecialisation(specialisation: any): Observable<Specialisation> { //inny typ
    return this.http.post<Specialisation>(this.SpecialisationUrl + "/create", specialisation) // Bez obiektu opakowującego
  }

  updateSpecialisation(specialisation: any): Observable<Specialisation>{ //inny typ
    return this.http.put<Specialisation>(this.SpecialisationUrl+"/update", specialisation)
  }

  deleteSpecialisation(dspecialisationId: number): Observable<string>{
    return this.http.delete<string>(this.SpecialisationUrl+"/Delete/"+ dspecialisationId)}

  //MEDICAL APPOINTMENT

  getAllMedicalAppointments(): Observable<MedicalAppointment[]> {
    return this.http.get<MedicalAppointment[]>(this.MedicalAppointmentUrl + "/Get");
  }

  getMedicalAppointmentById(appointmentId: number): Observable<MedicalAppointment> {
    return this.http.get<MedicalAppointment>(this.MedicalAppointmentUrl + "/Get/" + appointmentId)
  }

  getMedicalAppointmentsForDoctor(doctorId: number): Observable<AllMedicalAppointments>{
    return this.http.get<AllMedicalAppointments>(this.MedicalAppointmentUrl+"/GetByDoctorId/" + doctorId)
  };

  getMedicalAppointmentsByPatientId(patientId: number): Observable<AllMedicalAppointments>{
    return this.http.get<AllMedicalAppointments>(this.MedicalAppointmentUrl+"/GetByPatientId/" + patientId);
  }

  getMedicalAppointmentsBySpecialisationId(specialisationId: number): Observable<ReturnMedicalAppointment[]>{
    return this.http.get<ReturnMedicalAppointment[]>(this.MedicalAppointmentUrl+"/GetBySpecialisation/" + specialisationId);    
  }

  addMedicalAppointment(appointment: MedicalAppointment): Observable<MedicalAppointment> {
    return this.http.post<MedicalAppointment>(this.MedicalAppointmentUrl + "/create", appointment);
  }

  editMedicalAppointment(medicalAppointment: MedicalAppointment): Observable<MedicalAppointment> {
    return this.http.put<MedicalAppointment>(this.MedicalAppointmentUrl + "/update", medicalAppointment);
  }

  editMedicalAppointmentReturnDto(selectedAppointment: ReturnMedicalAppointment): Observable<MedicalAppointment> { //różnie się tylko dto od powyższego edit
    return this.http.put<MedicalAppointment>(this.MedicalAppointmentUrl+"/update", selectedAppointment)
  }

  editMedicalAppointmentCancel(medicalAppointment: MedicalAppointment): Observable<MedicalAppointment>{

    const headers = new HttpHeaders().set('Content-Type', 'application/json'); // Tworzenie nagłówków
    return this.http.put<MedicalAppointment>(this.MedicalAppointmentUrl + "/update", medicalAppointment, { headers });
  }

  finishMedicalAppointment(finishAppointmentDto: any): Observable<string>{ //inny typ x2
    const headers = new HttpHeaders().set('Content-Type', 'application/json'); // Tworzenie nagłówków
    //return this.http.post(this.MedicalAppointmentUrl + "/FinishMedicalAppointment", finishAppointmentDto,  {headers})
    return this.http.post<string>(this.MedicalAppointmentUrl + "/FinishMedicalAppointment", finishAppointmentDto,  {headers})

  }

  deleteMedicalAppointment(medicalAppointmentId: number): Observable<string> {
    return this.http.delete<string>(this.MedicalAppointmentUrl + "/Delete/" + medicalAppointmentId);
  }

  //DIAGNOSTIC TEST
  getDiagnosticTestsByAppointmentId(appointmentId: number): Observable<DiagnosticTest[]> {
    return this.http.get<DiagnosticTest[]>(this.DiagnosticTesttUrl + "/GetByMedicalAppointmentId/" + appointmentId);
    }

  //DIAGNOSTIC TEST TYPE

  getAllDiagnosticTestTypes(): Observable<DiagnosticTestType[]> {
    return this.http.get<DiagnosticTestType[]>(this.DiagnosticTestTypeUrl + "/Get");
  }

  updateDiagnosticTestType(diagnosticTesType: any): Observable<DiagnosticTestType> { //może w sumie byc DiagnosticTestType
    return this.http.put<DiagnosticTestType>(this.DiagnosticTestTypeUrl + "/update", diagnosticTesType);
  }

  deleteDiagnosticTestType(testTypeId: number): Observable<string> {
    return this.http.delete<string>(this.DiagnosticTestTypeUrl + "/Delete/" + testTypeId)
  }

  addDiagosticTestType(diagnosticTestType: any): Observable<DiagnosticTestType> { //może w sumie byc DiagnosticTestType
    return this.http.post<DiagnosticTestType>(this.DiagnosticTestTypeUrl + "/create", diagnosticTestType) // Bez obiektu opakowującego
  }




}
