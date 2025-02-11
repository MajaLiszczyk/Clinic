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
import { UserLoginRequest } from '../model/user-login-request';
import { LaboratoryWorker } from '../model/laboratory-worker';
import { LaboratorySupervisor } from '../model/laboratory-supervisor';
import { LaboratoryTestType } from '../model/laboratory-test-type';
import { LaboratoryAppointment } from '../model/laboratory-appointment';
import { LaboratoryTest } from '../model/laboratory-test';
import { LabAppWithPatientLabTestsMedApp } from '../dtos/labApp-patient-labTests-medApp-dto';
import { GroupWithLabTests } from '../dtos/group-labTests-dto';
import { LaboratoryTestsGroup } from '../model/laboratory-tests-group';
import { MedicalAppointmentPatientDoctorDto } from '../dtos/medical-appointment-patient-doctor-dto';
import { LaboratoryAppointmentWorkerSupervisor } from '../dtos/laboratory-appointment-worker-supervisor';
import { Registrant } from '../model/registrant';
import { MedicalAppointmentDoctorDto } from '../dtos/medical-appointment-doctor-dto';
import { MedicalAppointmentPatientDto } from '../dtos/medical-appointment-patient-dto';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  readonly DoctorUrl = "https://localhost:5001/api/doctor";
  readonly MedicalAppointmentUrl = "https://localhost:5001/api/MedicalAppointment";
  readonly DiagnosticTestTypeUrl = "https://localhost:5001/api/DiagnosticTestType";
  readonly SpecialisationUrl = "https://localhost:5001/api/medicalSpecialisation";
  readonly PatientUrl = "https://localhost:5001/api/patient";
  readonly RegistrantUrl = "https://localhost:5001/api/registrant";
  readonly DiagnosticTesttUrl = "https://localhost:5001/api/DiagnosticTest";
  readonly LogInUrl = "https://localhost:5001/api/authorization/login";
  readonly RegistrationUrl = "https://localhost:5001/api/Registration";
  readonly LaboratoryWorkerUrl = "https://localhost:5001/api/LaboratoryWorker";
  readonly LaboratorySupervisorUrl = "https://localhost:5001/api/LaboratorySupervisor";
  readonly LaboratoryTestTypeUrl = "https://localhost:5001/api/LaboratoryTestType";
  readonly LaboratoryTestUrl = "https://localhost:5001/api/LaboratoryTest";
  readonly LaboratoryAppointmentUrl = "https://localhost:5001/api/LaboratoryAppointment";
  readonly LaboratoryTestsGroupUrl = "https://localhost:5001/api/LaboratoryTestsGroup";

  constructor(private http: HttpClient) { }


  //REGISTRANT
  getAllRegistrants(): Observable<Registrant[]> {
    return this.http.get<Registrant[]>(this.RegistrantUrl + "/Get");
  }
  createRegistrantAccount(registrant: any): Observable<Registrant> {
    return this.http.post<Registrant>(this.RegistrationUrl + "/RegisterRegistrant", registrant)
  }


  //PATIENT
  getPatientById(patientId: number) {
    return this.http.get<Patient>(this.PatientUrl + "/Get/" + patientId);
  }
  getAllPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.PatientUrl + "/Get")
  }
  getAllAvailablePatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.PatientUrl + "/GetAvailable");
  }
  addPatient(patient: any): Observable<Patient> {
    return this.http.post<Patient>(this.PatientUrl + "/create", patient)
  }
  createPatientAccount(patient: any): Observable<Patient> {
    return this.http.post<Patient>(this.RegistrationUrl + "/RegisterPatient", patient)
  }
  updatePatient(patient: any): Observable<Patient> {
    return this.http.put<Patient>(this.PatientUrl + "/update", patient)
  }
  deletePatient(patientId: number): Observable<string> {
    const url = `${this.PatientUrl}/TransferToArchive/${patientId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' })
  }


  //LABORATORY TEST
  getLaboratoryTestsByAppointmentId(id: number): Observable<LaboratoryTest[]> {
    return this.http.get<LaboratoryTest[]>(this.LaboratoryTestUrl + "/GetByMedicalAppointmentId/" + id);
  }
  getComissionedLaboratoryTestsByPatientId(id: number): Observable<GroupWithLabTests[]> {
    return this.http.get<GroupWithLabTests[]>(this.LaboratoryTestUrl + "/GetComissionedLaboratoryTestsWithGroupByPatientId/" + id);
  }
  getLaboratoryTestsByLabAppId(id: number): Observable<LaboratoryTest[]> {
    return this.http.get<LaboratoryTest[]>(this.LaboratoryTestUrl + "/GetLaboratoryTestsByLabAppId/" + id);
  }
  saveLaboratoryTestResult(id: number, resultValue: string): Observable<LaboratoryTest> {
    const body = { resultValue };
    return this.http.put<LaboratoryTest>(this.LaboratoryTestUrl + "/SaveLaboratoryTestResult/" + id, body, { headers: { 'Content-Type': 'application/json' } });
    { responseType: 'text' as 'json' }
  }

  //supervisor
  acceptLaboratoryTest(id: number): Observable<LaboratoryTest> {
    return this.http.put<LaboratoryTest>(this.LaboratoryTestUrl + "/acceptLaboratoryTest/" + id, null);
  }
  rejectLaboratoryTest(id: number, rejectCommentValue: string): Observable<LaboratoryTest> {
    const rejectCommentDto = { rejectCommentValue };
    return this.http.put<LaboratoryTest>(this.LaboratoryTestUrl + "/rejectLaboratoryTest/" + id, rejectCommentDto, { headers: { 'Content-Type': 'application/json' } });
  }
  sendLaboratoryTestsToLaboratoryWorker(laboratoryAppointmentId: number): Observable<LaboratoryTest> {
    return this.http.put<LaboratoryTest>(this.LaboratoryAppointmentUrl + "/sendLaboratoryTestsToLaboratoryWorker/" + laboratoryAppointmentId, null);
  }

  //LABORATORY TEST TYPE
  getAllLaboratoryTestTypes(): Observable<LaboratoryTestType[]> {
    return this.http.get<LaboratoryTestType[]>(this.LaboratoryTestTypeUrl + "/Get");
  }
  getAllAvailableLaboratoryTestTypes(): Observable<LaboratoryTestType[]> {
    return this.http.get<LaboratoryTestType[]>(this.LaboratoryTestTypeUrl + "/GetAvailable");
  }
  updateLAboratoryTestType(laboratoryTestType: any): Observable<LaboratoryTestType> {
    return this.http.put<LaboratoryTestType>(this.LaboratoryTestTypeUrl + "/update", laboratoryTestType);
  }
  transferToArchiveLaboratoryTestType(testTypeId: number): Observable<string> {
    const url = `${this.LaboratoryTestTypeUrl}/TransferToArchive/${testTypeId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' })
  }
  addLaboratoryTestType(laboratoryTestType: any): Observable<LaboratoryTestType> {
    return this.http.post<LaboratoryTestType>(this.LaboratoryTestTypeUrl + "/create", laboratoryTestType)
  }


  //LABORATORY TESTS GROUP
  setLaboratoryAppointmentToTestsGroup(groupId: number, laboratoryAppointmentId: number): Observable<LaboratoryTestsGroup> {
    const url = `${this.LaboratoryTestsGroupUrl}/UpdateLaboratoryAppointmentToGroup/${groupId}/${laboratoryAppointmentId}`;
    return this.http.put<LaboratoryTestsGroup>(url, null);
  }

  //LABORATORY APPOINTMENT
  getAllLaboratoryAppointments(): Observable<LaboratoryAppointment[]> {
    return this.http.get<LaboratoryAppointment[]>(this.LaboratoryAppointmentUrl + "/Get");
  }
  getAllLaboratoryAppointmentsWorkersSupervisors(): Observable<LaboratoryAppointmentWorkerSupervisor[]> {
    return this.http.get<LaboratoryAppointmentWorkerSupervisor[]>(this.LaboratoryAppointmentUrl + "/GetWithWorkersSupervisors");
  }
  getLabAppDetailsByLabAppId(id: number): Observable<LabAppWithPatientLabTestsMedApp> {
    return this.http.get<LabAppWithPatientLabTestsMedApp>(this.LaboratoryAppointmentUrl + "/GetLabAppDetailsByLabAppId/" + id);
  }
  getAllAvailableLaboratoryAppointments(): Observable<LaboratoryAppointment[]> {
    return this.http.get<LaboratoryAppointment[]>(this.LaboratoryAppointmentUrl + "/GetAvailableLaboratoryAppointments");
  }
  isAllLaboratoryTestsResultsCompleted(id: number): Observable<LaboratoryAppointment[]> {
    return this.http.get<LaboratoryAppointment[]>(this.LaboratoryAppointmentUrl + "/IsAllLaboratoryTestsResultsCompleted/" + id);
  }
  getPlannedLaboratoryAppointmentsByPatientId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getPlannedLaboratoryAppointmentsByPatientId/" + id);
  }
  getFinishedLaboratoryAppointmentsByPatientId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getFinishedLaboratoryAppointmentsByPatientId/" + id);
  }
  getInProcessLaboratoryAppointmentsByPatientId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getInProcessLaboratoryAppointmentsByPatientId/" + id);
  }
  addLaboratoryAppointment(appointment: LaboratoryAppointment): Observable<LaboratoryAppointment> {
    return this.http.post<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/create", appointment);
  }
  editLaboratoryAppointment(laboratoryAppointment: LaboratoryAppointment): Observable<LaboratoryAppointment> {
    return this.http.put<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/update", laboratoryAppointment);
  }
  deleteLaboratoryAppointment(laboratoryAppointmentId: number): Observable<string> {
    return this.http.delete<string>(this.LaboratoryAppointmentUrl + "/Delete/" + laboratoryAppointmentId);
  }
  cancelPlannedAppointment(id: number): Observable<string> {
    return this.http.put<string>(this.LaboratoryAppointmentUrl + "/cancelPlannedAppointment/" + id, null, { responseType: 'text' as 'json' });
  }
  //laboratory-appointments ze strony lab-worker
  getFutureLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getFutureLabAppsByLabWorkerId/" + id);
  }
  getWaitingForFillLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getWaitingForFillLabAppsByLabWorkerId/" + id);
  }
  getWaitingForSupervisorLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getWaitingForSupervisorLabAppsByLabWorkerId/" + id);
  }
  getToBeFixedLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getToBeFixedLabAppsByLabWorkerId/" + id);
  }
  getReadyForPatientLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getReadyForPatientLabAppsByLabWorkerId/" + id);
  }
  getSentToPatientLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getSentToPatientLabAppsByLabWorkerId/" + id);
  }
  getCancelledLabAppsByLabWorkerId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getCancelledLabAppsByLabWorkerId/" + id);
  }
  //lab app ze strony supervisor
  getWaitingForReviewLabAppsBySupervisorId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getWaitingForReviewLabAppsBySupervisorId/" + id);
  }
  getAcceptedLabAppsBySupervisorId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getAcceptedLabAppsBySupervisorId/" + id);
  }
  getSentBackLabAppsBySupervisorId(id: number): Observable<LabAppWithPatientLabTestsMedApp[]> {
    return this.http.get<LabAppWithPatientLabTestsMedApp[]>(this.LaboratoryAppointmentUrl + "/getSentBackLabAppsBySupervisorId/" + id);
  }
  //Obsługa wizyt przez lab workera
  makeCancelledLaboratoryAppointment(id: number, cancelComment: string): Observable<LaboratoryAppointment> {
    return this.http.put<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/makeCancelledLaboratoryAppointment/" + id, cancelComment);
  }
  finishLaboratoryAppointment(id: number): Observable<LaboratoryAppointment> {
    return this.http.put<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/FinishLaboratoryAppointment/" + id, null);
  }
  sendLaboratoryTestsToSupervisor(id: number): Observable<LaboratoryAppointment> {
    return this.http.put<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/sendLaboratoryTestsToSupervisor/" + id, null);
  }
  sendLaboratoryTestsResultsToPatient(id: number): Observable<LaboratoryAppointment> {
    return this.http.put<LaboratoryAppointment>(this.LaboratoryAppointmentUrl + "/sendLaboratoryTestsResultsToPatient/" + id, null);
  }

  //LABORATORY WORKER
  getAllLaboratoryWorkers(): Observable<LaboratoryWorker[]> {
    return this.http.get<LaboratoryWorker[]>(this.LaboratoryWorkerUrl + "/Get");
  }
  getAllAvailableLaboratoryWorkers(): Observable<LaboratoryWorker[]> {
    return this.http.get<LaboratoryWorker[]>(this.LaboratoryWorkerUrl + "/GetAvailable");
  }
  createLaboratoryWorkerAccount(laboratoryWorker: any): Observable<LaboratoryWorker> {
    return this.http.post<LaboratoryWorker>(this.RegistrationUrl + "/RegisterLaboratoryWorker", laboratoryWorker)
  }
  updateLaboratoryWorker(laboratoryWorker: any): Observable<LaboratoryWorker> {
    return this.http.put<LaboratoryWorker>(this.LaboratoryWorkerUrl + "/update", laboratoryWorker)
  }
  archiveLaboratoryWorker(laboratoryWorkerId: number): Observable<string> {
    const url = `${this.LaboratoryWorkerUrl}/TransferToArchive/${laboratoryWorkerId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' })
  }

  //LABORATORY SUPERVISOR
  getAllLaboratorySupervisors(): Observable<LaboratorySupervisor[]> {
    return this.http.get<LaboratorySupervisor[]>(this.LaboratorySupervisorUrl + "/Get");
  }
  getAllAvailableLaboratorySupervisors(): Observable<LaboratorySupervisor[]> {
    return this.http.get<LaboratorySupervisor[]>(this.LaboratorySupervisorUrl + "/GetAvailable");
  }
  createLaboratorySupervisorAccount(laboratorySupervisor: any): Observable<LaboratorySupervisor> {
    return this.http.post<LaboratorySupervisor>(this.RegistrationUrl + "/RegisterLaboratorySupervisor", laboratorySupervisor)
  }
  updateLaboratorySupervisor(laboratorySupervisor: any): Observable<LaboratorySupervisor> {
    return this.http.put<LaboratorySupervisor>(this.LaboratorySupervisorUrl + "/update", laboratorySupervisor)
  }
  archiveLaboratorySupervisor(laboratorySupervisorId: number): Observable<string> {
    const url = `${this.LaboratoryWorkerUrl}/TransferToArchive/${laboratorySupervisorId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' })
  }

  //DOCTOR
  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.DoctorUrl + "/Get");
  }
  getAllAvailableDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.DoctorUrl + "/GetAvailable");
  }
  getAllDoctorsWithSpecialisations(): Observable<DoctorWithSpcecialisations[]> {
    return this.http.get<DoctorWithSpcecialisations[]>(this.DoctorUrl + "/GetWithSpecialisations")
  }
  addDoctor(doctor: any): Observable<Doctor> {
    return this.http.post<Doctor>(this.RegistrationUrl + "/RegisterDoctor", doctor);
  }
  updateDoctor(doctor: any): Observable<Doctor> {
    return this.http.put<Doctor>(this.DoctorUrl + "/update", doctor)
  }
  deleteDoctor(doctorId: number): Observable<string> {
    const url = `${this.DoctorUrl}/TransferToArchive/${doctorId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' });
  }

  //SPECIALISATIONS
  getAllSpecialisations(): Observable<Specialisation[]> {
    return this.http.get<Specialisation[]>(this.SpecialisationUrl + "/Get");
  }
  getAllAvailableSpecialisations(): Observable<Specialisation[]> {
    return this.http.get<Specialisation[]>(this.SpecialisationUrl + "/GetAvailable");
  }


  addSpecialisation(specialisation: any): Observable<Specialisation> {
    return this.http.post<Specialisation>(this.SpecialisationUrl + "/create", specialisation)
  }
  updateSpecialisation(specialisation: any): Observable<Specialisation> {
    return this.http.put<Specialisation>(this.SpecialisationUrl + "/update", specialisation)
  }
  deleteSpecialisation(dspecialisationId: number): Observable<string> {
    const url = `${this.SpecialisationUrl}/TransferToArchive/${dspecialisationId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' });
  }

  //MEDICAL APPOINTMENT
  getAllMedicalAppointments(): Observable<MedicalAppointment[]> {
    return this.http.get<MedicalAppointment[]>(this.MedicalAppointmentUrl + "/Get");
  }

  getFutureMedicalAppointmentsByPatientId(patientId: number): Observable<MedicalAppointmentPatientDoctorDto[]> {
    return this.http.get<MedicalAppointmentPatientDoctorDto[]>(this.MedicalAppointmentUrl + "/GetFutureMedicalAppointmentsByPatientOrUserId/" + patientId);
  }
  getPastMedicalAppointmentsByPatientId(patientId: number): Observable<MedicalAppointmentPatientDoctorDto[]> {
    return this.http.get<MedicalAppointmentPatientDoctorDto[]>(this.MedicalAppointmentUrl + "/GetPastMedicalAppointmentsByPatientOrUserId/" + patientId);
  }
  getAllMedicalAppointmentsPatientsDoctors(): Observable<MedicalAppointmentPatientDoctorDto[]> {
    return this.http.get<MedicalAppointmentPatientDoctorDto[]>(this.MedicalAppointmentUrl + "/GetWithPatientsDoctors");
  }
  getMedicalAppointmentByIdPatient(appointmentId: number): Observable<MedicalAppointmentPatientDto> {
    return this.http.get<MedicalAppointmentPatientDto>(this.MedicalAppointmentUrl + "/GetMedicalAppointmentByIdWithPatient/" + appointmentId)
  }
  getMedicalAppointmentsForDoctor(doctorId: number): Observable<AllMedicalAppointments> {
    return this.http.get<AllMedicalAppointments>(this.MedicalAppointmentUrl + "/GetByDoctorId/" + doctorId)
  };
  getMedicalAppointmentsByPatientId(patientId: number): Observable<AllMedicalAppointments> {
    console.log(patientId);
    return this.http.get<AllMedicalAppointments>(this.MedicalAppointmentUrl + "/GetByPatientId/" + patientId);
  }
  getMedicalAppointmentsBySpecialisationId(specialisationId: number): Observable<MedicalAppointmentDoctorDto[]> {
    return this.http.get<MedicalAppointmentDoctorDto[]>(this.MedicalAppointmentUrl + "/GetBySpecialisation/" + specialisationId);
  }
  addMedicalAppointment(appointment: MedicalAppointment): Observable<MedicalAppointment> {
    return this.http.post<MedicalAppointment>(this.MedicalAppointmentUrl + "/create", appointment);
  }
  editMedicalAppointment(medicalAppointment: MedicalAppointmentPatientDoctorDto): Observable<MedicalAppointmentPatientDoctorDto> {
    return this.http.put<MedicalAppointmentPatientDoctorDto>(this.MedicalAppointmentUrl + "/updatePatientCancel", medicalAppointment);
  }
  editMedicalAppointmentReturnDto(selectedAppointment: MedicalAppointment): Observable<MedicalAppointment> {
    return this.http.put<MedicalAppointment>(this.MedicalAppointmentUrl + "/update", selectedAppointment)
  }
  editMedicalAppointmentCancel(medicalAppointment: MedicalAppointment): Observable<MedicalAppointment> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.put<MedicalAppointment>(this.MedicalAppointmentUrl + "/update", medicalAppointment, { headers });
  }
  finishMedicalAppointment(finishAppointmentDto: any): Observable<string> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<string>(this.MedicalAppointmentUrl + "/FinishMedicalAppointment", finishAppointmentDto, { headers })
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
  getAllAvailableDiagnosticTestTyoes(): Observable<DiagnosticTestType[]> {
    return this.http.get<DiagnosticTestType[]>(this.DiagnosticTestTypeUrl + "/GetAvailable");
  }
  updateDiagnosticTestType(diagnosticTesType: any): Observable<DiagnosticTestType> {
    return this.http.put<DiagnosticTestType>(this.DiagnosticTestTypeUrl + "/update", diagnosticTesType);
  }
  deleteDiagnosticTestType(testTypeId: number): Observable<string> {
    const url = `${this.DiagnosticTestTypeUrl}/TransferToArchive/${testTypeId}`;
    return this.http.put<string>(url, null, { responseType: 'text' as 'json' })
  }
  addDiagosticTestType(diagnosticTestType: any): Observable<DiagnosticTestType> {
    return this.http.post<DiagnosticTestType>(this.DiagnosticTestTypeUrl + "/create", diagnosticTestType)
  }
}
